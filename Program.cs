using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plash
{    
    class Simulation
    {
        //TIME REFERENCE: HOUR////

        //Series Inputs
        public DateTime[] DTE_Arr_TimeSeries;
        public double[] FLT_Arr_PrecipSeries = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.8, 0.2, 0.2, 0.8, 0.8, 1.4, 1, 5, 3.8, 2.6, 1.8, 1, 1, 1.8, 0.4, 1.6, 5.4, 7, 5.6, 3, 6.6, 5.2, 4.6, 5.4, 5, 6.8, 7.8, 8.4, 7.8, 8.6, 6.2, 3, 1.8, 6, 12.4, 6.2, 2.2, 2, 2.8, 1, 1, 0.6, 1.2, 0.2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public double[] FLT_Arr_EPSeries = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };
        public double[] FLT_Arr_QtObsSeries = { 1.457, 1.174, 1.052, 1.054, 1.147, 1.159, 1.067, 1.131, 1.159, 1.248, 1.323, 1.381, 1.409, 1.368, 1.397, 1.427, 1.427, 1.414, 1.381, 1.422, 1.381, 1.381, 1.464, 1.562, 1.755, 1.848, 1.936, 2.14, 2.054, 2.23, 2.646, 4.876, 7.524, 9.872, 12.405, 15.05, 16.37, 17.808, 18.506, 20.065, 22.038, 24.759, 26.728, 27.982, 28.024, 25.961, 22.174, 21.46, 26.11, 29.513, 29.202, 26.018, 21.255, 17.54, 15.265, 12.94, 11.815, 9.886, 8.652, 7.921, 6.646, 5.869, 5.612, 4.946, 4.396, 4.265, 4.645, 5.239, 4.657, 4.326, 4.394, 4.14, 3.877, 3.132, 2.198, 2.565, 2.275, 2.311, 2.402, 2.208, 2.128, 2.168, 2.556, 2.669, 2.653, 2.745, 2.498, 2.33, 2.554, 2.588, 2.62, 2.323, 2.236, 2.404, 2.363, 2.586, 2.107, 2.013, 1.85, 1.727 };

        //Reservoirs
        public double[] FLT_Arr_RImp; //Impervious Reservoir Level
        public double[] FLT_Arr_RInt; //Interception Reservoir Level
        public double[] FLT_Arr_RSup; //Surface Reservoir Level
        public double[] FLT_Arr_RSol; //Soil Reservoir Level
        public double[] FLT_Arr_RSub; //Aquifer Reservoir Level
        public double[] FLT_Arr_RCan; //Channel Reservoir Level

        //Impervious Detention Reservoir
        public double[] FLT_Arr_ERImp; //Real Evapotranspiration
        public double[] FLT_Arr_ESImp; //Downstream Flow

        //Pervious Interception Reservoir
        public double[] FLT_Arr_ERInt; //Real Evapotranspiration
        public double[] FLT_Arr_ESInt; // Downstream Flow

        //Pervious Detention Reservoir (Surface Reservoir)
        public double[] FLT_Arr_EPSup; //Potential Evapotranspiration
        public double[] FLT_Arr_EESup; //Upstream Flow
        public double[] FLT_Arr_ERSup; //Real Evapotranspiration
        public double[] FLT_Arr_ESSup; //Downstream Flow
        public double[] FLT_Arr_Infiltration; //Infiltration in Time Step
        public double[] FLT_Arr_Infiltration_Cumulative; //Total Infiltration

        //Shallow Soil Reservoir
        public double[] FLT_Arr_EPSol; //Potential Evapotranspiration
        public double[] FLT_Arr_EESol; //Upstream Flow (Infiltration)
        public double[] FLT_Arr_ERSol; //Real Evapotranspiration
        public double[] FLT_Arr_ESSol; //Downstream Flow

        //Aquifer Reservoir
        public double[] FLT_Arr_EESub; //Upstream Flow (Recharge)
        public double[] FLT_Arr_PPSub; //Deep Percolation
        public double[] FLT_Arr_ESSub; //Downstream Flow

        //Channel Reservoir
        public double[] FLT_Arr_EPCan; //Potential Evapotranspiration
        public double[] FLT_Arr_ERCan; //Real Evapotranspiration
        public double[] FLT_Arr_EECan; //Upstream Flow
        public double[] FLT_ARR_ESCan; //Downstream Flow

        //Resulting Flows
        public double[] FLT_Arr_QBas_Calc;
        public double[] FLT_Arr_QSup_Calc;
        public double[] FLT_Arr_Qt_Calc;

        //Parameters
        public double FLT_TimeStep = 1;
        public double FLT_AD = 18.5;    //Watershed Area (km2)
        public double FLT_AI = 0.1;     //Impervious Area Fraction (km2/km2)
        public double FLT_DI = 5;       //Maximum Impervious Detention (mm)
        public double FLT_AP = 0.95;    //Pervious Area Fraction (km2/km2)
        public double FLT_IP = 3;       //Maximum Interception (mm)
        public double FLT_DP = 6;       //Maximum Pervious Detention (mm)
        public double FLT_KSup = 50;    //Surface Reservoir Decay (h)
        public double FLT_CS = 10;      //Soi Saturation Capacity (mm)
        public double FLT_CC = 0.3;     //Field Capacity (%)
        public double FLT_CR = 0.05;    //Recharge Capacity (%)
        public double FLT_PP = 0.02;    //Deep Percolation (mm/h)
        public double FLT_KSub = 180;   //Aquifer Reservoir Decay (d)
        public double FLT_KCan = 2;     //Channel Reservoir Decay (h)
        public double FLT_CH = 0.05;    //Hydraulic Conductivity (mm/h)
        public double FLT_FS = 50;      //Soil Capilarity Factor (mm)
        public double FLT_PS = 0.5;     //Soil Porosity (cm3/cm3)
        public double FLT_UI = 0.2;     //Initial Moisture (cm3/cm3)

        public double FLT_kSup;
        public double FLT_kSub;
        public double FLT_kCan;
        public double FLT_pp;


        public void AuxiliaryParameters()
        {
            FLT_kSup = Math.Pow((Math.Pow(0.5, 1 / FLT_KSup)), FLT_TimeStep);
            FLT_kSub = Math.Pow((Math.Pow(0.5, 1 / (FLT_KSub * 24))), FLT_TimeStep);
            FLT_kCan = Math.Pow((Math.Pow(0.5, 1 / FLT_KCan)), FLT_TimeStep);
            FLT_pp = FLT_PP * (FLT_TimeStep / 24);
        }

        public void SetArrays(int length)
        {
            FLT_Arr_RImp = new double[length];
            FLT_Arr_RImp = new double[length];
            FLT_Arr_RInt = new double[length];
            FLT_Arr_RSup = new double[length];
            FLT_Arr_RSol = new double[length];
            FLT_Arr_RSub = new double[length];
            FLT_Arr_RCan = new double[length];
                        
            FLT_Arr_ERImp = new double[length];
            FLT_Arr_ESImp = new double[length];

            
            FLT_Arr_ERInt = new double[length];
            FLT_Arr_ESInt = new double[length];

            
            FLT_Arr_EPSup = new double[length];
            FLT_Arr_EESup = new double[length];
            FLT_Arr_ERSup = new double[length];
            FLT_Arr_ESSup = new double[length];
            FLT_Arr_Infiltration = new double[length];
            FLT_Arr_Infiltration_Cumulative = new double[length];

            
            FLT_Arr_EPSol = new double[length];
            FLT_Arr_EESol = new double[length];
            FLT_Arr_ERSol = new double[length];
            FLT_Arr_ESSol = new double[length];

            
            FLT_Arr_EESub = new double[length];
            FLT_Arr_PPSub = new double[length];
            FLT_Arr_ESSub = new double[length];

            
            FLT_Arr_EPCan = new double[length];
            FLT_Arr_ERCan = new double[length];
            FLT_Arr_EECan = new double[length];
            FLT_ARR_ESCan = new double[length];

            
            FLT_Arr_QBas_Calc = new double[length];
            FLT_Arr_QSup_Calc = new double[length];
            FLT_Arr_Qt_Calc = new double[length];




        }

        static void GreenAmpt()
        {

        }

    }


    class Program
    {
        
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            Simulation Sim = new Simulation();

            Console.WriteLine("Parametros Originais pre:");
            Console.WriteLine("KSup = {0}", Sim.FLT_KSup);
            Console.WriteLine("KSub = {0}", Sim.FLT_KSub);
            Console.WriteLine("KCan = {0}", Sim.FLT_KCan);
            Console.WriteLine("PP = {0}", Sim.FLT_PP);

            Console.WriteLine("Parametros Auxiliares pre:");
            Console.WriteLine("kSup = {0}", Sim.FLT_kSup);
            Console.WriteLine("kSub = {0}", Sim.FLT_kSub);
            Console.WriteLine("kCan = {0}", Sim.FLT_kCan);
            Console.WriteLine("pp = {0}", Sim.FLT_pp);
                        
            Sim.AuxiliaryParameters();

            Console.WriteLine("Parametros Originais pos:");
            Console.WriteLine("KSup = {0}", Sim.FLT_KSup);
            Console.WriteLine("KSub = {0}", Sim.FLT_KSub);
            Console.WriteLine("KCan = {0}", Sim.FLT_KCan);
            Console.WriteLine("PP = {0}", Sim.FLT_PP);

            Console.WriteLine("Parametros Auxiliares pos:");
            Console.WriteLine("kSup = {0}", Sim.FLT_kSup);
            Console.WriteLine("kSub = {0}", Sim.FLT_kSub);
            Console.WriteLine("kCan = {0}", Sim.FLT_kCan);
            Console.WriteLine("pp = {0}", Sim.FLT_pp);

            Console.ReadKey();

            Sim.DTE_Arr_TimeSeries = new DateTime[100];
            //Sim.FLT_Arr_PrecipSeries = new double[20];
            //Sim.FLT_Arr_EPSeries = new double[20];
            //Sim.FLT_Arr_QtObsSeries = new double[20];

            Sim.SetArrays(100);

            //Initializations
            Sim.FLT_Arr_RImp[0] = 0;
            Sim.FLT_Arr_RInt[0] = 0;
            Sim.FLT_Arr_RSup[0] = 0;
            Sim.FLT_Arr_RSol[0] = Sim.FLT_UI * Sim.FLT_CS;
            Sim.FLT_Arr_RSub[0] = (Sim.FLT_Arr_QtObsSeries[0] / (1 - Sim.FLT_kSub)) * (3.6 / Sim.FLT_AD);
            Sim.FLT_Arr_RCan[0] = 0;


            Sim.DTE_Arr_TimeSeries[0] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Console.WriteLine("Data Inicial: {0}", Sim.DTE_Arr_TimeSeries[0]);
            for(int i = 1; i < Sim.DTE_Arr_TimeSeries.Length; i++)
            {
                Sim.DTE_Arr_TimeSeries[i] = Sim.DTE_Arr_TimeSeries[0].AddHours(Sim.FLT_TimeStep * i);
                //Console.WriteLine("i: {0}, Data: {1}", i, Sim.DTE_Arr_TimeSeries[i]);
            }
            for (int i = 0; i < 100; i++)
            {
                //Console.WriteLine("Data: {0}, Prec: {1}; EP: {2}; Q Obs: {3}", Sim.DTE_Arr_TimeSeries[i], Sim.FLT_Arr_PrecipSeries[i], Sim.FLT_Arr_EPSeries[i], Sim.FLT_Arr_QtObsSeries[i]);

                //Impervious Reservoir                
                Sim.FLT_Arr_ERImp[i] = Math.Min( (i > 0 ? Sim.FLT_Arr_RImp[i - 1] : Sim.FLT_Arr_RImp[i]) + Sim.FLT_Arr_PrecipSeries[i], Sim.FLT_Arr_EPSeries[i]);
                Sim.FLT_Arr_ESImp[i] = Math.Max((i > 0 ? Sim.FLT_Arr_RImp[i - 1] : Sim.FLT_Arr_RImp[i]) + Sim.FLT_Arr_PrecipSeries[i] - Sim.FLT_Arr_ERImp[i] - Sim.FLT_DI, 0);
                if(i > 0)
                {
                    Sim.FLT_Arr_RImp[i] = Sim.FLT_Arr_RImp[i-1] + Sim.FLT_Arr_PrecipSeries[i] - Sim.FLT_Arr_ERImp[i] - Sim.FLT_Arr_ESImp[i];
                }

                
                //Console.WriteLine("ERImp: {0} mm, ESImp: {1} mm, RImp: {2} mm", Sim.FLT_Arr_ERImp[i], Sim.FLT_Arr_ESImp[i], Sim.FLT_Arr_RImp[i]);

                //Interception Reservoir
                Sim.FLT_Arr_ERInt[i] = Math.Min((i > 0 ? Sim.FLT_Arr_RInt[i - 1] : Sim.FLT_Arr_RInt[i]) + Sim.FLT_Arr_PrecipSeries[i], Sim.FLT_Arr_EPSeries[i]);
                Sim.FLT_Arr_ESInt[i] = Math.Max((i > 0 ? Sim.FLT_Arr_RInt[i - 1] : Sim.FLT_Arr_RInt[i]) + Sim.FLT_Arr_PrecipSeries[i] - Sim.FLT_Arr_ERInt[i] - Sim.FLT_IP, 0);
                if(i > 0)
                {
                    Sim.FLT_Arr_RInt[i] = Sim.FLT_Arr_RInt[i - 1] + Sim.FLT_Arr_PrecipSeries[i] - Sim.FLT_Arr_ERInt[i] - Sim.FLT_Arr_ESInt[i];
                }

                //Console.WriteLine("ERInt: {0} mm, ESInt: {1} mm, RInt: {2} mm", Sim.FLT_Arr_ERInt[i], Sim.FLT_Arr_ESInt[i], Sim.FLT_Arr_RInt[i]);

                //Surface Reservoir
                Sim.FLT_Arr_EESup[i] = Sim.FLT_Arr_ESImp[i] * (Sim.FLT_AI / Sim.FLT_AP) + Sim.FLT_Arr_ESInt[i];
                Sim.FLT_Arr_EPSup[i] = Sim.FLT_Arr_EPSeries[i] - Sim.FLT_Arr_ERInt[i];
                Sim.FLT_Arr_ERSup[i] = Math.Min((i > 0 ? Sim.FLT_Arr_RSup[i - 1] : Sim.FLT_Arr_RSup[i]) + Sim.FLT_Arr_EESup[i], Sim.FLT_Arr_EPSup[i]);
                Sim.FLT_Arr_Infiltration[i] = 0; //METODO DA INFILTRAÇÃO
                Sim.FLT_Arr_ESSup[i] = Math.Max((i > 0 ? Sim.FLT_Arr_RSup[i - 1] : Sim.FLT_Arr_RSup[i]) + Sim.FLT_Arr_EESup[i] - Sim.FLT_Arr_ERSup[i] - Sim.FLT_Arr_Infiltration[i] - Sim.FLT_DP, 0) * (1 - Sim.FLT_kSup);
                if (i > 0)
                {
                    Sim.FLT_Arr_RSup[i] = Sim.FLT_Arr_RSup[i - 1] + Sim.FLT_Arr_EESup[i] - Sim.FLT_Arr_ERSup[i] - Sim.FLT_Arr_Infiltration[i] - Sim.FLT_Arr_ESSup[i];
                }


                Console.WriteLine("EESup: {0}, EPSup: {1}, ERSup: {2}, I: {3}, ESSup: {4}, RSup: {5}", Sim.FLT_Arr_EESup[i], Sim.FLT_Arr_EPSup[i], Sim.FLT_Arr_ERSup[i], Sim.FLT_Arr_Infiltration[i], Sim.FLT_Arr_ESSup[i], Sim.FLT_Arr_RSup[i]);




            }
            





            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
