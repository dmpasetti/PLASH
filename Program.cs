using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using OfficeOpenXml;


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
                FLT_Kw = 0.1,
                FLT_Nw = 1
            };

            Buildup_Washoff.fncBuildupWashoffProcess(BuWoModel, Sim.FLT_Arr_ESSup, Sim.FLT_AD, 0, 2, 1, Sim.FLT_TimeStep, 0.5);

            //double[] FLT_Arr_WashoffResults = BuWoModel.fncBuildupWashoffProcess(Sim.FLT_Arr_PrecipSeries, Sim.FLT_Arr_ESSup, Sim.FLT_AD, 0, 2, 1, Sim.FLT_TimeStep);

            Console.ReadKey();

            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Results_PLASH");
                excel.Workbook.Worksheets.Add("Results_BuWo");

                var HeaderRow1 = new List<string[]>()
                {
                    new string[] {"Precipitation", "Evapotranspiration", "Observed Flow",
                        "Impervious Reservoir", "Interception Reservoir", "Surface Reservoir", "Soil Reservoir", "Aquifer Reservoir", "Channel Reservoir",
                    "Calculated Basic Flow", "Calculated Surface Flow", "Calculated Total Flow"},
                };

                string headerRange1 = "A1:" + Char.ConvertFromUtf32(HeaderRow1[0].Length + 64) + 1;

                var worksheet = excel.Workbook.Worksheets["Results_PLASH"];

                worksheet.Cells[headerRange1].LoadFromArrays(HeaderRow1);

                List<object[]> cellData1 = new List<object[]>();

                for (int i = 0; i < SimulationLength; i++)
                {
                    cellData1.Add(new object[] { Sim.FLT_Arr_PrecipSeries[i], Sim.FLT_Arr_EPSeries[i], Sim.FLT_Arr_QtObsSeries[i],
                    Sim.FLT_Arr_RImp[i], Sim.FLT_Arr_RInt[i], Sim.FLT_Arr_RSup[i], Sim.FLT_Arr_RSol[i], Sim.FLT_Arr_RSub[i], Sim.FLT_Arr_RCan[i],
                    Sim.FLT_Arr_QBas_Calc[i], Sim.FLT_Arr_QSup_Calc[i], Sim.FLT_Arr_Qt_Calc[i]});
                }

                worksheet.Cells[2, 1].LoadFromArrays(cellData1);


                var HeaderRow2 = new List<string[]>()
                {
                    new string[] {"Precipitation", "Surface Flow", "Buildup", "Washoff"},
                };

                string headerRange2 = "A1:" + Char.ConvertFromUtf32(HeaderRow2[0].Length + 64) + 1;

                worksheet = excel.Workbook.Worksheets["Results_BuWo"];

                worksheet.Cells[headerRange2].LoadFromArrays(HeaderRow2);

                List<object[]> cellData2 = new List<object[]>();

                for (int i = 0; i < SimulationLength; i++)
                {
                    cellData2.Add(new object[] { Sim.FLT_Arr_PrecipSeries[i], Sim.FLT_Arr_ESSup[i], BuWoModel.FLT_Arr_Buildup[i], BuWoModel.FLT_Arr_Washoff[i] });
                }

                worksheet.Cells[2, 1].LoadFromArrays(cellData2);

                FileInfo excelFile = new FileInfo(@"D:\data.xlsx");
                excel.SaveAs(excelFile);
            }

            Console.WriteLine("Excel processed");

            Console.ReadKey();



            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
