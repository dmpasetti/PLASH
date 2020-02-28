using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlashClasses
{
    public class Buildup_Washoff
    {
        public LandUse STR_UseName;
        public double FLT_BMax; //Maximum Buildup, normalized (kg/km²)
        public double FLT_Nb; //Buildup Exponent  (-). Nb <= 1 
        public double FLT_Kb;/*Buildup rate constant (Varies.   Pow: kg/km²*d^-Nb; 
                                                                Exp: d^-1; 
                                                                Sat: d.)*/

        public double FLT_ThresholdFlow;
        public double FLT_Nw; //Washoff exponent (-)
        public double FLT_Kw; /*Washoff rate constante (Varies. Exp:  (mm/h)*h^-1
                                                                Rating: (Kg/h)*(L/h)^-Nw
                                                                EMC: mg/m³
        */




        public double[] FLT_Arr_SurfaceFlow;

        public double[] FLT_Arr_Washoff; //Washoff time series in the entire timestep (kg)
        public double[] FLT_Arr_Buildup; //Buildup time series, non-normalized (kg)
        public double[] FLT_Arr_EffectiveWashoff; //Washoff time series limited by available Buildup (kg)


        public BuildupMethod INT_BuMethod;
        public WashoffMethod INT_WoMethod;

        public double FLT_Timestep_h;
        public double FLT_Timestep_d;
        public double FLT_InitialBuildup;
        public double FLT_Area;

        public enum BuildupMethod
        {
            Pow = 1,
            Exp = 2,
            Sat = 3
        }

        public enum WashoffMethod
        {
            Exp = 1,
            Rating = 2,
            EMC = 3
        }

        #region Time From Buildup Equations
        public double TimeFromBuildup_Pow(double FLT_Buildup)
        {
            double Time = Math.Pow(FLT_Buildup / FLT_Kb, (1 / FLT_Nb));
            if (double.IsNaN(Time))
            {
                return 0;
            }
            else
            {
                return Time;
            }
        }

        public double TimeFromBuildup_Exp(double FLT_Buildup)
        {
            double Time = -Math.Log(1 - (FLT_Buildup / FLT_BMax)) / FLT_Kb;
            if (double.IsNaN(Time))
            {
                return 0;
            }
            else
            {
                return Time;
            }
        }

        public double TimeFromBuildup_Sat(double FLT_Buildup)
        {
            double Time = (FLT_Buildup * FLT_Kb) / (FLT_BMax - FLT_Buildup);
            if (double.IsNaN(Time))
            {
                return 0;
            }
            else
            {
                return Time;
            }
        }
        #endregion Time From Buildup Equations

        #region Buildup Equations

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
        #endregion BuildupEquations

        #region Washoff Equations
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FLT_SurfaceFlow"></param>
        /// Surface Flow computed from hydrological model. Unit: mm
        /// <param name="FLT_BuildupMass"></param>
        /// Total buildup in timestep. Unit: kg
        /// <returns></returns>
        public double Washoff_Exp(double FLT_SurfaceFlow, double FLT_BuildupMass)
        {
            return FLT_Kw * Math.Pow(FLT_SurfaceFlow, FLT_Nw) * FLT_BuildupMass;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FLT_SurfaceFlow"></param>
        /// Surface Flow computed from hydrological model. Unit: mm
        /// <param name="FLT_WatershedArea"></param>
        /// Area of the watershed. Should be input after considering land use fractions. Unit: km²
        /// <returns></returns>
        public double Washoff_Rating(double FLT_SurfaceFlow, double FLT_WatershedArea)
        {
            return FLT_Kw * Math.Pow(1000 * FLT_SurfaceFlow * FLT_WatershedArea, FLT_Nw);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FLT_SurfaceFlow"></param>
        /// Surface Flow computed from hydrological model. Unit: mm
        /// <param name="FLT_WatershedArea"></param>
        /// Area of the watershed. Should be input after considering land use fractions. Unit: km²
        /// <returns></returns>
        public double Washoff_EMC(double FLT_SurfaceFlow, double FLT_WatershedArea)
        {
            return FLT_Kw * FLT_SurfaceFlow * FLT_WatershedArea;
        }
        #endregion Washoff Equations



        public static void fncBuildupWashoffProcess(Buildup_Washoff Sim)
        {
            Sim.FLT_Arr_Washoff = new double[Sim.FLT_Arr_SurfaceFlow.Length];
            Sim.FLT_Arr_Buildup = new double[Sim.FLT_Arr_SurfaceFlow.Length];
            Sim.FLT_Arr_EffectiveWashoff = new double[Sim.FLT_Arr_SurfaceFlow.Length];
            Sim.FLT_Timestep_d = Sim.FLT_Timestep_h / 24;

            bool BOOL_Buildup;
            double FLT_BuildupSpecific = 0;
            double FLT_BuildupMass = Sim.FLT_InitialBuildup;
            double FLT_WashoffRate = 0;


            BuildupMethod BMethod = Sim.INT_BuMethod;
            WashoffMethod WMethod = Sim.INT_WoMethod;

            double FLT_BuildupTime = 0;
            switch (BMethod)
            {
                case BuildupMethod.Pow:
                    FLT_BuildupTime = Sim.TimeFromBuildup_Pow(Sim.FLT_InitialBuildup / Sim.FLT_Area);
                    break;
                case BuildupMethod.Exp:
                    FLT_BuildupTime = Sim.TimeFromBuildup_Exp(Sim.FLT_InitialBuildup / Sim.FLT_Area);
                    break;
                case BuildupMethod.Sat:
                    FLT_BuildupTime = Sim.TimeFromBuildup_Sat(Sim.FLT_InitialBuildup / Sim.FLT_Area);
                    break;
                default:
                    break;
            }


            for (int i = 0; i < Sim.FLT_Arr_Washoff.Length; i++) {
                if(i == 474)
                {
                    var dummy = 0;
                }
                BOOL_Buildup = Sim.FLT_Arr_SurfaceFlow[i] < Sim.FLT_ThresholdFlow;
                if (BOOL_Buildup)
                {
                    FLT_BuildupTime += Sim.FLT_Timestep_d;
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
                    FLT_BuildupMass = FLT_BuildupSpecific * Sim.FLT_Area;
                }
                else
                {
                    switch (WMethod)
                    {
                        case WashoffMethod.Exp:
                            FLT_WashoffRate = Sim.Washoff_Exp(Sim.FLT_Arr_SurfaceFlow[i], FLT_BuildupMass);
                            break;
                        case WashoffMethod.Rating:
                            FLT_WashoffRate = Sim.Washoff_Rating(Sim.FLT_Arr_SurfaceFlow[i], Sim.FLT_Area);
                            break;
                        case WashoffMethod.EMC:
                            FLT_WashoffRate = Sim.Washoff_EMC(Sim.FLT_Arr_SurfaceFlow[i], Sim.FLT_Area);
                            break;
                        default:
                            break;
                    }
                    FLT_BuildupSpecific = 0;
                    Sim.FLT_Arr_Washoff[i] = FLT_WashoffRate * Sim.FLT_Timestep_d;
                    Sim.FLT_Arr_EffectiveWashoff[i] = Math.Min(Sim.FLT_Arr_Washoff[i], FLT_BuildupMass);
                    FLT_BuildupMass = FLT_BuildupMass - Sim.FLT_Arr_EffectiveWashoff[i];

                    switch (BMethod)
                    {
                        case BuildupMethod.Pow:
                            FLT_BuildupTime = Sim.TimeFromBuildup_Pow(FLT_BuildupMass / Sim.FLT_Area);
                            break;
                        case BuildupMethod.Exp:
                            FLT_BuildupTime = Sim.TimeFromBuildup_Exp(FLT_BuildupMass / Sim.FLT_Area);
                            break;
                        case BuildupMethod.Sat:
                            FLT_BuildupTime = Sim.TimeFromBuildup_Sat(FLT_BuildupMass / Sim.FLT_Area);
                            break;
                        default:
                            break;
                    }
                }
                if (double.IsNaN(FLT_BuildupMass))
                {
                    var dummy = 1;
                }
                Sim.FLT_Arr_Buildup[i] = FLT_BuildupMass;
                //Console.WriteLine("i: {5}, Bool_Buildup: {0}, BuildupRate: {1}, Buildup Total: {2}, Washoff: {3}, Effective Washoff: {4}, Time: {5}",
                //    BOOL_Buildup,
                //    Math.Round(FLT_BuildupSpecific, 3),
                //    Math.Round(FLT_BuildupMass, 3),
                //    Math.Round(Sim.FLT_Arr_Washoff[i], 3),
                //    Math.Round(Sim.FLT_Arr_EffectiveWashoff[i], 3),
                //    Math.Round(FLT_BuildupTime, 3),
                //    i);
            }
        }

        public static Buildup_Washoff AggregateUses(List<Buildup_Washoff> lstUses, double FLT_WatershedArea)
        {
            //Recebo varios objetos BuWo
            //Cada um tem um double[] com as series de buildup e washoff
            //Preciso, para cada passo de tempo, agregar os valores (media ponderada) de cada uso

            int Arraylength = lstUses[0].FLT_Arr_Buildup.Length;

            double[] AverageBuildup = new double[Arraylength];
            double[] AverageWashoff = new double[Arraylength];
            double[] AverageEffectiveWashoff = new double[Arraylength];
            for (int i = 0; i < Arraylength; i++)
            {
                AverageBuildup[i] = lstUses.Sum(x => x.FLT_Arr_Buildup[i] * (x.FLT_Area / FLT_WatershedArea));
                AverageWashoff[i] = lstUses.Sum(x => x.FLT_Arr_Washoff[i] * (x.FLT_Area / FLT_WatershedArea));
                AverageEffectiveWashoff[i] = lstUses.Sum(x => x.FLT_Arr_EffectiveWashoff[i] * (x.FLT_Area / FLT_WatershedArea));
            }

            Buildup_Washoff AggregateObj = new Buildup_Washoff()
            {
                FLT_Area = FLT_WatershedArea,
                FLT_Arr_Buildup = AverageBuildup,
                FLT_Arr_Washoff = AverageWashoff,
                FLT_Arr_EffectiveWashoff = AverageEffectiveWashoff
            };

            return AggregateObj;
        }

        public static Buildup_Washoff Transport(Buildup_Washoff Upstream, Buildup_Washoff Downstream)
        {
            return new Buildup_Washoff()
            {
                FLT_Area = Upstream.FLT_Area + Downstream.FLT_Area,
                FLT_Arr_Buildup = Upstream.FLT_Arr_Buildup.Zip(Downstream.FLT_Arr_Buildup, (x, y) => x + y).ToArray(),
                FLT_Arr_EffectiveWashoff = Upstream.FLT_Arr_EffectiveWashoff.Zip(Downstream.FLT_Arr_EffectiveWashoff, (x, y) => x + y).ToArray()
            };
        }



        public static List<Buildup_Washoff> BuwoUpstreamList(double Timestep, double[] FLT_Arr_SurfaceFlowArray)
        {
            double BMax = 0.07;
            double Kb = 0.1;
            double Nb = 0.5;
            double Kw = 0.2;
            double Nw = 1;
            List<Buildup_Washoff> UsesUpstream = new List<Buildup_Washoff>();
            //Upstream Urban
            UsesUpstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.Urban,
                FLT_Timestep_h = Timestep,
                FLT_Area = 5.466,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            //Upstream Open Field
            UsesUpstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.OpenField,
                FLT_Timestep_h = Timestep,
                FLT_Area = 1.652,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            //Upstream Agriculture
            UsesUpstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.Agriculture,
                FLT_Timestep_h = Timestep,
                FLT_Area = 64.867,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            //Upstream Forest
            UsesUpstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.Forest,
                FLT_Timestep_h = Timestep,
                FLT_Area = 379.029,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            //Upstream Water Bodies
            UsesUpstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.WaterBody,
                FLT_Timestep_h = Timestep,
                FLT_Area = 4.254,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            //Upstream Pasture
            UsesUpstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.Pasture,
                FLT_Timestep_h = Timestep,
                FLT_Area = 308.261,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            //Upstream Silviculture
            UsesUpstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.Silviculture,
                FLT_Timestep_h = Timestep,
                FLT_Area = 99.649,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            return UsesUpstream;
        }

        public static List<Buildup_Washoff> BuwoDownstreamList(double Timestep, double[] FLT_Arr_SurfaceFlowArray)
        {
            double BMax = 0.07;
            double Kb = 0.1;
            double Nb = 0.5;
            double Kw = 0.2;
            double Nw = 1;

            List<Buildup_Washoff> UsesDownstream = new List<Buildup_Washoff>();
            //Downstream Urban
            UsesDownstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.Urban,
                FLT_Timestep_h = Timestep,
                FLT_Area = 1.379,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            //Downstream Open Field
            UsesDownstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.OpenField,
                FLT_Timestep_h = Timestep,
                FLT_Area = 26.987,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            //Downstream Agriculture
            UsesDownstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.Agriculture,
                FLT_Timestep_h = Timestep,
                FLT_Area = 102.357,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            //Downstream Forest
            UsesDownstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.Forest,
                FLT_Timestep_h = Timestep,
                FLT_Area = 130.849,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            //Downstream Water Bodies
            UsesDownstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.WaterBody,
                FLT_Timestep_h = Timestep,
                FLT_Area = 0.109,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            //Downstream Pasture
            UsesDownstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.Pasture,
                FLT_Timestep_h = Timestep,
                FLT_Area = 334.115,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });
            //Downstream Silviculture
            UsesDownstream.Add(new Buildup_Washoff()
            {
                STR_UseName = LandUse.Silviculture,
                FLT_Timestep_h = Timestep,
                FLT_Area = 133.222,
                FLT_BMax = BMax,
                FLT_Kb = Kb,
                FLT_Nb = Nb,
                FLT_Kw = Kw,
                FLT_Nw = Nw,
                FLT_InitialBuildup = 0,
                FLT_ThresholdFlow = 0.2,
                FLT_Arr_SurfaceFlow = FLT_Arr_SurfaceFlowArray,
                INT_BuMethod = Buildup_Washoff.BuildupMethod.Exp,
                INT_WoMethod = Buildup_Washoff.WashoffMethod.Exp
            });

            return UsesDownstream;

        }

        public enum LandUse
        {
            Urban,
            OpenField,
            Agriculture,
            Forest,
            WaterBody,
            Pasture,
            Silviculture
        }

        public static Dictionary<LandUse, double> ExportCoef = new Dictionary<LandUse, double>()
        {
            { LandUse.Urban, 0.034 },
            { LandUse.OpenField, 0.028 },
            {LandUse.Agriculture, 0.346 },
            {LandUse.Forest, 0.039 },
            {LandUse.WaterBody, 0 },
            {LandUse.Pasture, 0.05 },
            {LandUse.Silviculture, 0.039 }
        };        
        
    }
}


/*
 Calculo taxa de buildup e de washoff
 Buildup e washoff são mutualmente excludentes
 Se ocorrer buildup, acumular carga
 Se ocorrer washoff, lavar (subtrair) carga do valor existente
 Determinar o tempo equivalente: 
    
     
     */