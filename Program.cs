using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plash
{   


    class Program
    {
        
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            PLASH Sim = new PLASH();

            int SimulationLength = 100;
            Sim.DTE_Arr_TimeSeries = new DateTime[SimulationLength];
            
            Sim.DTE_Arr_TimeSeries[0] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Console.WriteLine("Data Inicial: {0}", Sim.DTE_Arr_TimeSeries[0]);
            for(int i = 1; i < Sim.DTE_Arr_TimeSeries.Length; i++)
            {
                Sim.DTE_Arr_TimeSeries[i] = Sim.DTE_Arr_TimeSeries[0].AddHours(Sim.FLT_TimeStep * i);
                //Console.WriteLine("i: {0}, Data: {1}", i, Sim.DTE_Arr_TimeSeries[i]);
            }

            PLASH.Run(Sim, SimulationLength);
            
           
            Console.WriteLine("");
            Console.ReadKey();

            Buildup_Washoff BuWoModel = new Buildup_Washoff
            {
                FLT_BMax = 10,
                FLT_Kb = 0.1,
                FLT_Nb = 1,
                FLT_Kw = 1,
                FLT_Nw = 1
            };

            Buildup_Washoff.fncBuildupWashoffProcess(BuWoModel, Sim.FLT_Arr_PrecipSeries, Sim.FLT_Arr_ESSup, Sim.FLT_AD, 0, 2, 1, Sim.FLT_TimeStep);

            //double[] FLT_Arr_WashoffResults = BuWoModel.fncBuildupWashoffProcess(Sim.FLT_Arr_PrecipSeries, Sim.FLT_Arr_ESSup, Sim.FLT_AD, 0, 2, 1, Sim.FLT_TimeStep);

            Console.ReadKey();


            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
