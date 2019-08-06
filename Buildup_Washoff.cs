using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plash
{
    class Buildup_Washoff
    {
        public double FLT_BMax;
        public double FLT_Nb;
        public double FLT_Kb;

        public double FLT_Nw;
        public double FLT_Kw;

        public double[] FLT_Arr_Washoff;
        public double[] FLT_Arr_Buildup;


        private enum BuildupMethod
        {
            Pow = 1,
            Exp = 2,
            Sat = 3
        }

        private enum WashoffMethod
        {
            Exp = 1,
            Rating = 2,
            EMC = 3
        }


        public double TimeFromBuildup_Pow(double FLT_Buildup)
        {
            return Math.Pow(FLT_Buildup / FLT_Kb, (1 / FLT_Nb));
        }

        public double TimeFromBuildup_Exp(double FLT_Buildup)
        {
            return -Math.Log(1 - (FLT_Buildup / FLT_BMax)) / FLT_Kb;
        }

        public double TimeFromBuildup_Sat(double FLT_Buildup)
        {
            return (FLT_Buildup * FLT_Kb) / (FLT_BMax - FLT_Buildup);
        }

        public double Buildup_Pow(double FLT_Time)
        {
            return Math.Min(FLT_BMax, FLT_Kb * Math.Pow(FLT_Time, FLT_Nb));
        }

        public double Buildup_Exp(double FLT_Time)
        {
            return FLT_BMax * (1 - Math.Exp(-FLT_Kb * FLT_Time));
        }

        public double Buildup_Sat(double FLT_Time)
        {
            return (FLT_BMax * FLT_Time) / (FLT_Kb + FLT_Time);
        }


        public double Washoff_Exp(double FLT_SurfaceFlow, double FLT_BuildupMass)
        {
            return FLT_Kw * Math.Pow(FLT_SurfaceFlow, FLT_Nw) * FLT_BuildupMass;
        }

        public double Washoff_Rating(double FLT_SurfaceFlow, double FLT_WatershedArea)
        {
            return FLT_Kw * Math.Pow(FLT_SurfaceFlow * FLT_WatershedArea, FLT_Nw);
        }

        public double Washoff_EMC(double FLT_SurfaceFlow, double FLT_WatershedArea)
        {
            return FLT_Kw * FLT_SurfaceFlow * FLT_WatershedArea;
        }




        public static void fncBuildupWashoffProcess(Buildup_Washoff Sim, double[] FLT_Arr_SurfaceFlow, double FLT_WatershedArea, double FLT_InitialBuildup, int ID_BuildupMethod, int ID_WashoffMethod, double FLT_Timestep, double FLT_BuildupThreshold)
        {
            Sim.FLT_Arr_Washoff = new double[FLT_Arr_SurfaceFlow.Length];
            Sim.FLT_Arr_Buildup = new double[FLT_Arr_SurfaceFlow.Length];


            bool BOOL_Buildup;
            double FLT_BuildupSpecific = 0;
            double FLT_BuildupMass = FLT_InitialBuildup;
            double FLT_WashoffRate = 0;
            

            BuildupMethod BMethod = (BuildupMethod)ID_BuildupMethod;
            WashoffMethod WMethod = (WashoffMethod)ID_WashoffMethod;

            double FLT_BuildupTime = 0;
            switch (BMethod)
            {
                case BuildupMethod.Pow:
                    FLT_BuildupTime = Sim.TimeFromBuildup_Pow(FLT_InitialBuildup / FLT_WatershedArea);
                    break;
                case BuildupMethod.Exp:
                    FLT_BuildupTime = Sim.TimeFromBuildup_Exp(FLT_InitialBuildup / FLT_WatershedArea);
                    break;
                case BuildupMethod.Sat:
                    FLT_BuildupTime = Sim.TimeFromBuildup_Sat(FLT_InitialBuildup / FLT_WatershedArea);
                    break;
                default:
                    break;
            }


            for (int i = 0; i < Sim.FLT_Arr_Washoff.Length; i++) {
                BOOL_Buildup = FLT_Arr_SurfaceFlow[i] > FLT_BuildupThreshold ? false : true;
                if (BOOL_Buildup)
                {
                    FLT_BuildupTime += FLT_Timestep;
                    Sim.FLT_Arr_Washoff[i] = 0;
                    switch (BMethod)
                    {
                        case BuildupMethod.Pow:
                            FLT_BuildupSpecific = Sim.Buildup_Pow(FLT_BuildupTime);
                            break;
                        case BuildupMethod.Exp:
                            FLT_BuildupSpecific = Sim.Buildup_Exp(FLT_BuildupTime);
                            break;
                        case BuildupMethod.Sat:
                            FLT_BuildupSpecific = Sim.Buildup_Sat(FLT_BuildupTime);
                            break;
                        default:
                            break;                        
                    }
                    FLT_BuildupMass = FLT_BuildupSpecific * FLT_WatershedArea;           
                }
                else
                {
                    switch (WMethod)
                    {
                        case WashoffMethod.Exp:
                            FLT_WashoffRate = Sim.Washoff_Exp(FLT_Arr_SurfaceFlow[i], FLT_BuildupMass);
                            break;
                        case WashoffMethod.Rating:
                            FLT_WashoffRate = Sim.Washoff_Rating(FLT_Arr_SurfaceFlow[i], FLT_WatershedArea);
                            break;
                        case WashoffMethod.EMC:
                            FLT_WashoffRate = Sim.Washoff_EMC(FLT_Arr_SurfaceFlow[i], FLT_WatershedArea);
                            break;
                        default:
                            break;
                    }
                    FLT_BuildupSpecific = 0;
                    Sim.FLT_Arr_Washoff[i] = FLT_WashoffRate * FLT_Timestep;
                    FLT_BuildupMass = Math.Max(FLT_BuildupMass - FLT_WashoffRate * FLT_Timestep, 0);

                    switch (BMethod)
                    {
                        case BuildupMethod.Pow:
                            FLT_BuildupTime = Sim.TimeFromBuildup_Pow(FLT_BuildupMass / FLT_WatershedArea);
                            break;
                        case BuildupMethod.Exp:
                            FLT_BuildupTime = Sim.TimeFromBuildup_Exp(FLT_BuildupMass / FLT_WatershedArea);
                            break;
                        case BuildupMethod.Sat:
                            FLT_BuildupTime = Sim.TimeFromBuildup_Sat(FLT_BuildupMass / FLT_WatershedArea);
                            break;
                        default:
                            break;
                    }
                }
                Sim.FLT_Arr_Buildup[i] = FLT_BuildupMass;
                Console.WriteLine("i: {5}, Bool_Buildup: {0}, BuildupRate: {1}, Buildup Total: {2}, Washoff: {3}, Time: {4}", 
                    BOOL_Buildup, 
                    Math.Round(FLT_BuildupSpecific,3), 
                    Math.Round(FLT_BuildupMass, 3),
                    Math.Round(Sim.FLT_Arr_Washoff[i], 3),
                    Math.Round(FLT_BuildupTime, 3),
                    i);
            }
        }



    }
}


/*
 Calculo taxa de buildup e de washoff
 Buildup e washoff são mutualmente excludentes
 Se ocorrer buildup, acumular carga
 Se ocorrer washoff, lavar (subtrair) carga do valor existente
 Determinar o tempo equivalente: 
    
     
     */