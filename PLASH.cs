using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plash
{
    class PLASH
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
        public double[] FLT_Arr_IAE;
        public double[] FLT_Arr_TP;
        public double[] FLT_Arr_IAEAdim;
        public double[] FLT_Arr_TPAdim;

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
        public double FLT_AI = 0.05;     //Impervious Area Fraction (km2/km2)
        public double FLT_DI = 5;       //Maximum Impervious Detention (mm)
        public double FLT_AP = 0.9;    //Pervious Area Fraction (km2/km2)
        public double FLT_IP = 3;       //Maximum Interception (mm)
        public double FLT_DP = 6;       //Maximum Pervious Detention (mm)
        public double FLT_KSup = 12;    //Surface Reservoir Decay (h)
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
            FLT_Arr_IAE = new double[length];
            FLT_Arr_TP = new double[length];
            FLT_Arr_IAEAdim = new double[length];
            FLT_Arr_TPAdim = new double[length];

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


        


        public static void Run(PLASH Sim, int INT_SimulationLength)
        {
            Sim.AuxiliaryParameters();
            Sim.SetArrays(INT_SimulationLength);

            Console.WriteLine("Parametros Originais:");
            Console.WriteLine("KSup = {0}", Sim.FLT_KSup);
            Console.WriteLine("KSub = {0}", Sim.FLT_KSub);
            Console.WriteLine("KCan = {0}", Sim.FLT_KCan);
            Console.WriteLine("PP = {0}", Sim.FLT_PP);

            Console.WriteLine("Parametros Auxiliares:");
            Console.WriteLine("kSup = {0}", Sim.FLT_kSup);
            Console.WriteLine("kSub = {0}", Sim.FLT_kSub);
            Console.WriteLine("kCan = {0}", Sim.FLT_kCan);
            Console.WriteLine("pp = {0}", Sim.FLT_pp);

            Console.ReadKey();

            double RImp0 = 0;
            double RInt0 = 0;
            double RSup0 = 0;
            double RSol0 = Sim.FLT_UI * Sim.FLT_CS;
            double RSub0 = (Sim.FLT_Arr_QtObsSeries[0] / (1 - Sim.FLT_kSub)) * (3.6 / Sim.FLT_AD);
            double RCan0 = 0;

            Sim.FLT_Arr_RImp[0] = RImp0;
            Sim.FLT_Arr_RInt[0] = RInt0;
            Sim.FLT_Arr_RSup[0] = RSup0;
            Sim.FLT_Arr_RSol[0] = RSol0;
            Sim.FLT_Arr_RSub[0] = RSub0;
            Sim.FLT_Arr_RCan[0] = RCan0;

            for (int i = 0; i < INT_SimulationLength; i++)
            {
                
                #region Impervious Reservoir
                //Impervious Reservoir                
                Sim.FLT_Arr_ERImp[i] = Math.Min((i > 0 ? Sim.FLT_Arr_RImp[i - 1] : RImp0) + Sim.FLT_Arr_PrecipSeries[i], Sim.FLT_Arr_EPSeries[i]);
                Sim.FLT_Arr_ESImp[i] = Math.Max((i > 0 ? Sim.FLT_Arr_RImp[i - 1] : RImp0) + Sim.FLT_Arr_PrecipSeries[i] - Sim.FLT_Arr_ERImp[i] - Sim.FLT_DI, 0);
                if (i > 0)
                {
                    Sim.FLT_Arr_RImp[i] = Sim.FLT_Arr_RImp[i - 1] + Sim.FLT_Arr_PrecipSeries[i] - Sim.FLT_Arr_ERImp[i] - Sim.FLT_Arr_ESImp[i];
                }
                #endregion Impervious Reservoir

                #region Interception Reservoir
                //Interception Reservoir
                Sim.FLT_Arr_ERInt[i] = Math.Min((i > 0 ? Sim.FLT_Arr_RInt[i - 1] : RInt0) + Sim.FLT_Arr_PrecipSeries[i], Sim.FLT_Arr_EPSeries[i]);
                Sim.FLT_Arr_ESInt[i] = Math.Max((i > 0 ? Sim.FLT_Arr_RInt[i - 1] : RInt0) + Sim.FLT_Arr_PrecipSeries[i] - Sim.FLT_Arr_ERInt[i] - Sim.FLT_IP, 0);
                if (i > 0)
                {
                    Sim.FLT_Arr_RInt[i] = Sim.FLT_Arr_RInt[i - 1] + Sim.FLT_Arr_PrecipSeries[i] - Sim.FLT_Arr_ERInt[i] - Sim.FLT_Arr_ESInt[i];
                }
                #endregion Interception Reservoir

                #region Surface Reservoir
                //Surface Reservoir
                Sim.FLT_Arr_EESup[i] = Sim.FLT_Arr_ESImp[i] * (Sim.FLT_AI / Sim.FLT_AP) + Sim.FLT_Arr_ESInt[i];
                Sim.FLT_Arr_EPSup[i] = Sim.FLT_Arr_EPSeries[i] - Sim.FLT_Arr_ERInt[i];
                Sim.FLT_Arr_ERSup[i] = Math.Min((i > 0 ? Sim.FLT_Arr_RSup[i - 1] : RSup0) + Sim.FLT_Arr_EESup[i], Sim.FLT_Arr_EPSup[i]);

                #region Infiltration

                //Infiltration
                double FLT_S2 = 2 * Sim.FLT_CH * (Sim.FLT_PS - Sim.FLT_UI) * (Sim.FLT_FS + (i > 0 ? Sim.FLT_Arr_RSup[i - 1] : RSup0) + Sim.FLT_Arr_EESup[i] - Sim.FLT_Arr_ERSup[i]);
                double FLT_it = (FLT_S2 / (2 * (i > 0 ? Sim.FLT_Arr_Infiltration_Cumulative[i - 1] : 0))) + Sim.FLT_CH;
                double FLT_it1 = (FLT_S2 / (2 * ((i > 0 ? Sim.FLT_Arr_Infiltration_Cumulative[i - 1] : 0) + Sim.FLT_Arr_PrecipSeries[i]))) + Sim.FLT_CH;
                double FLT_Puddling = (Sim.FLT_Arr_EESup[i] - Sim.FLT_Arr_ERSup[i]) / Sim.FLT_TimeStep;
                if (FLT_it <= FLT_Puddling)
                {
                    Sim.FLT_Arr_IAE[i] = i > 0 ? Sim.FLT_Arr_IAE[i - 1] : 0;
                    Sim.FLT_Arr_TP[i] = 0;
                }
                if (FLT_it1 < FLT_Puddling)
                {
                    Sim.FLT_Arr_IAE[i] = FLT_S2 / (2 * (FLT_Puddling - Sim.FLT_CH));
                    Sim.FLT_Arr_TP[i] = Sim.FLT_Arr_IAE[i] - (i > 0 ? Sim.FLT_Arr_Infiltration_Cumulative[i - 1] : 0) / FLT_Puddling;
                }
                Sim.FLT_Arr_IAEAdim[i] = 2 * Sim.FLT_CH * Sim.FLT_Arr_IAE[i] / FLT_S2;
                Sim.FLT_Arr_TPAdim[i] = 2 * Math.Pow(Sim.FLT_CH, 2) * (Sim.FLT_TimeStep - Sim.FLT_Arr_TP[i]) / FLT_S2;
                double FLT_Sigma = Math.Sqrt(2 * (Sim.FLT_Arr_TPAdim[i] + Sim.FLT_Arr_IAEAdim[i] - Math.Log(1 + Sim.FLT_Arr_IAEAdim[i])));
                double FLT_Sigma_1 = (Math.Pow(FLT_Sigma, 2) / 2);
                double FLT_Sigma_2 = Math.Pow((1 + FLT_Sigma / 6), -1);
                double FLT_W_1 = (FLT_Sigma_1 + Math.Log(1 + FLT_Sigma_1 + FLT_Sigma * FLT_Sigma_2)) / (Math.Pow((1 + FLT_Sigma_1 + FLT_Sigma * FLT_Sigma_2), -1) - 1);

                if (FLT_it1 > FLT_Puddling)
                {
                    Sim.FLT_Arr_Infiltration_Cumulative[i] = (i > 0 ? Sim.FLT_Arr_Infiltration_Cumulative[i - 1] : 0) + Sim.FLT_Arr_EESup[i] - Sim.FLT_Arr_ERSup[i];
                }
                else
                {
                    Sim.FLT_Arr_Infiltration_Cumulative[i] = (FLT_S2 * (-1 - FLT_W_1)) / (2 * Sim.FLT_CH);
                }
                Sim.FLT_Arr_Infiltration[i] = Math.Max(Sim.FLT_Arr_Infiltration_Cumulative[i] - (i > 0 ? Sim.FLT_Arr_Infiltration_Cumulative[i - 1] : 0), 0);                
                //END Infiltration
                #endregion Infiltration

                Sim.FLT_Arr_ESSup[i] = Math.Max((i > 0 ? Sim.FLT_Arr_RSup[i - 1] : RSup0) + Sim.FLT_Arr_EESup[i] - Sim.FLT_Arr_ERSup[i] - Sim.FLT_Arr_Infiltration[i] - Sim.FLT_DP, 0) * (1 - Sim.FLT_kSup);
                if (i > 0)
                {
                    Sim.FLT_Arr_RSup[i] = Sim.FLT_Arr_RSup[i - 1] + Sim.FLT_Arr_EESup[i] - Sim.FLT_Arr_ERSup[i] - Sim.FLT_Arr_Infiltration[i] - Sim.FLT_Arr_ESSup[i];
                }
                #endregion Surface Reservoir

                #region Soil Reservoir
                //Soil Reservoir
                Sim.FLT_Arr_EESol[i] = Sim.FLT_Arr_Infiltration[i];
                Sim.FLT_Arr_EPSol[i] = Sim.FLT_Arr_EPSup[i] - Sim.FLT_Arr_ERSup[i];
                Sim.FLT_Arr_ERSol[i] = Sim.FLT_Arr_EPSol[i] * ((i > 0 ? Sim.FLT_Arr_RSol[i - 1] : RSol0) / Sim.FLT_CS);
                Sim.FLT_Arr_ESSol[i] = Math.Max((i > 0 ? Sim.FLT_Arr_RSol[i - 1] : RSol0) - Sim.FLT_CC * Sim.FLT_CS, 0) * (Sim.FLT_CR * ((i > 0 ? Sim.FLT_Arr_RSol[i - 1] : RSol0) / Sim.FLT_CS));
                if (i > 0)
                {
                    Sim.FLT_Arr_RSol[i] = Sim.FLT_Arr_RSol[i - 1] + Sim.FLT_Arr_EESol[i] - Sim.FLT_Arr_ERSol[i] - Sim.FLT_Arr_ESSol[i];
                }
                #endregion Soil Reservoir

                #region Aquifer Reservoir
                //Aquifer Reservoir
                Sim.FLT_Arr_EESub[i] = Sim.FLT_Arr_ESSol[i] * Sim.FLT_AP;
                Sim.FLT_Arr_PPSub[i] = Math.Min((i > 0 ? Sim.FLT_Arr_RSub[i - 1] : RSub0) + Sim.FLT_Arr_EESub[i], Sim.FLT_pp);
                Sim.FLT_Arr_ESSub[i] = ((i > 0 ? Sim.FLT_Arr_RSub[i - 1] : RSub0) + Sim.FLT_Arr_EESub[i] - Sim.FLT_Arr_PPSub[i]) * (1 - Sim.FLT_kSub);
                if (i > 0)
                {
                    Sim.FLT_Arr_RSub[i] = Sim.FLT_Arr_RSub[i - 1] + Sim.FLT_Arr_EESub[i] - Sim.FLT_Arr_PPSub[i] - Sim.FLT_Arr_ESSub[i];
                }
                #endregion Aquifer Reservoir

                #region Channel Reservoir

                //Channel Reservoir
                Sim.FLT_Arr_EECan[i] = Sim.FLT_Arr_PrecipSeries[i] + Sim.FLT_Arr_ESSup[i] * (Sim.FLT_AP / (1 - Sim.FLT_AP - Sim.FLT_AI));
                Sim.FLT_Arr_ERCan[i] = Math.Min((i > 0 ? Sim.FLT_Arr_RCan[i - 1] : RCan0) + Sim.FLT_Arr_EECan[i], Sim.FLT_Arr_EPSeries[i]);
                Sim.FLT_ARR_ESCan[i] = Math.Max((i > 0 ? Sim.FLT_Arr_RCan[i - 1] : RCan0) + Sim.FLT_Arr_EECan[i] - Sim.FLT_Arr_ERCan[i], 0) * (1 - Sim.FLT_kCan);
                if (i > 0)
                {
                    Sim.FLT_Arr_RCan[i] = Sim.FLT_Arr_RCan[i - 1] + Sim.FLT_Arr_EECan[i] - Sim.FLT_Arr_ERCan[i] - Sim.FLT_ARR_ESCan[i];
                }
                #endregion Channel Reservoir

                #region Total Flow
                Sim.FLT_Arr_QBas_Calc[i] = Sim.FLT_Arr_ESSub[i] * (Sim.FLT_AD / (3.6 * Sim.FLT_TimeStep));
                Sim.FLT_Arr_QSup_Calc[i] = Sim.FLT_ARR_ESCan[i] * (Sim.FLT_AD / (3.6 * Sim.FLT_TimeStep)) * (1 - Sim.FLT_AP - Sim.FLT_AI);
                Sim.FLT_Arr_Qt_Calc[i] = Sim.FLT_Arr_QSup_Calc[i] + Sim.FLT_Arr_QBas_Calc[i];

                #endregion Total Flow

                #region Results Output
                //Console.WriteLine("Data: {0}, Prec: {1}; EP: {2}; Q Obs: {3}", Sim.DTE_Arr_TimeSeries[i], Sim.FLT_Arr_PrecipSeries[i], Sim.FLT_Arr_EPSeries[i], Sim.FLT_Arr_QtObsSeries[i]);
                //Console.WriteLine("ERImp: {0} mm, ESImp: {1} mm, RImp: {2} mm", Sim.FLT_Arr_ERImp[i], Sim.FLT_Arr_ESImp[i], Sim.FLT_Arr_RImp[i]);
                //Console.WriteLine("ERInt: {0} mm, ESInt: {1} mm, RInt: {2} mm", Sim.FLT_Arr_ERInt[i], Sim.FLT_Arr_ESInt[i], Sim.FLT_Arr_RInt[i]);
                //Console.WriteLine("S2: {0}, it: {1}, it+1: {2}, puddling: {3}, IAE: {4}, TP: {5}, IAEAdim: {6}, TPAdim: {7}, Sigma: {8}, W1: {9}, IA: {10}, I: {11}", 
                //    Math.Round(FLT_S2,3), 
                //    Math.Round(FLT_it, 3), 
                //    Math.Round(FLT_it1, 3), 
                //    Math.Round(FLT_Puddling, 3), 
                //    Math.Round(Sim.FLT_Arr_IAE[i], 3), 
                //    Math.Round(Sim.FLT_Arr_TP[i], 3), 
                //    Math.Round(Sim.FLT_Arr_IAEAdim[i], 3), 
                //    Math.Round(Sim.FLT_Arr_TPAdim[i],3),
                //    Math.Round(FLT_Sigma,3),
                //    Math.Round(FLT_W_1, 3),
                //    Math.Round(Sim.FLT_Arr_Infiltration_Cumulative[i], 3),
                //    Math.Round(Sim.FLT_Arr_Infiltration[i],3));
                //Console.WriteLine("EESup: {0}, EPSup: {1}, ERSup: {2}, I: {3}, ESSup: {4}, RSup: {5}", Sim.FLT_Arr_EESup[i], Sim.FLT_Arr_EPSup[i], Sim.FLT_Arr_ERSup[i], Sim.FLT_Arr_Infiltration[i], Sim.FLT_Arr_ESSup[i], Sim.FLT_Arr_RSup[i]);
                //Console.WriteLine("EESol: {0}, EPSol: {1}, ERSol: {2}, ESSol: {3}, RSol: {4}", Sim.FLT_Arr_EESol[i], Sim.FLT_Arr_EPSol[i], Sim.FLT_Arr_ERSol[i], Sim.FLT_Arr_ESSol[i], Sim.FLT_Arr_RSol[i]);
                //Console.WriteLine("EESub: {0}, PPSub: {1}, ESSub: {2}, RSub: {3}, QSub: {4}", Sim.FLT_Arr_EESub[i], Sim.FLT_Arr_PPSub[i], Sim.FLT_Arr_ESSub[i], Sim.FLT_Arr_RSub[i], Sim.FLT_Arr_ESSub[i] * (Sim.FLT_AD / (3.6 * Sim.FLT_TimeStep)));
                //Console.WriteLine("EECan: {0}, ERCan: {1}, ESCan: {2}, RCan: {3}", Sim.FLT_Arr_EECan[i], Sim.FLT_Arr_ERCan[i], Sim.FLT_ARR_ESCan[i], Sim.FLT_Arr_RCan[i]);
                Console.WriteLine("QBas: {0}, QSup: {1}, Qt: {2}", Math.Round(Sim.FLT_Arr_QBas_Calc[i], 3), Math.Round(Sim.FLT_Arr_QSup_Calc[i], 3), Math.Round(Sim.FLT_Arr_Qt_Calc[i], 3));
                //Console.WriteLine("Diferenca Absoluta:{0}", Math.Round(Sim.FLT_Arr_Qt_Calc[i] - Sim.FLT_Arr_QtObsSeries[i], 3));
                //Console.WriteLine("Precip: {0}, Surface Flow: {1}", Sim.FLT_Arr_PrecipSeries[i], Sim.FLT_Arr_ESSup[i]);
                #endregion Results Output

            }


        }
    }
}
