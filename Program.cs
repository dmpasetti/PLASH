using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using OfficeOpenXml;
using System.Reflection;

using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;



namespace Plash
{   
    
    class Program
    {

        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            //Console.WriteLine("Hello World!");
            //Console.ReadKey();

            #region Excel Input
            FileInfo InputFile = new FileInfo(@"D:\InputPLASH.xlsx");
            List<double> InputPrecipUp = new List<double>();
            List<double> InputPrecipDown = new List<double>();
            List<double> InputQObs = new List<double>();
            List<double> InputEvap = new List<double>();
            using (ExcelPackage package = new ExcelPackage(InputFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int ColCount = worksheet.Dimension.End.Column;
                int RowCount = worksheet.Dimension.End.Row;
                for(int row = 2; row <= RowCount; row++)
                {
                    InputPrecipUp.Add(Convert.ToDouble(worksheet.Cells[row, 2].Value));
                    InputPrecipDown.Add(Convert.ToDouble(worksheet.Cells[row, 3].Value));
                    InputQObs.Add(Convert.ToDouble(worksheet.Cells[row, 4].Value));
                    InputEvap.Add(Convert.ToDouble(worksheet.Cells[row, 5].Value));
                    
                    //Console.WriteLine("PrecipUp: {0}, PrecipDown: {1}, Evap: {2}, QObos: {3}", Math.Round(InputPrecipUp[row - 2],3), Math.Round(InputPrecipDown[row - 2], 3), Math.Round(InputQObs[row - 2],3), Math.Round(InputEvap[row - 2],3));
                }
                
            }


            #endregion Excel Input


            #region PLASH Simulation
            int SimulationLength = InputPrecipUp.Count;
            double Timestep = 24;

            //PLASH SimUpstream = new PLASH()
            //{
            //    FLT_AD = 861.42252,    //Watershed Area (km2)
            //    FLT_AI = 0.02,     //Impervious Area Fraction (km2/km2)
            //    FLT_AP = 0.95,    //Pervious Area Fraction (km2/km2)
            //    FLT_TimeStep = 24,
            //    DTE_Arr_TimeSeries = new DateTime[SimulationLength],
            //    FLT_Arr_QtObsSeries = InputQObs.ToArray(),
            //    FLT_Arr_PrecipSeries = InputPrecipUp.ToArray(),
            //    FLT_Arr_EPSeries = InputEvap.ToArray(),

            //    //Parameters
            //    FLT_DI = 5,       //Maximum Impervious Detention (mm)                
            //    FLT_IP = 3,       //Maximum Interception (mm)
            //    FLT_DP = 6,       //Maximum Pervious Detention (mm)
            //    FLT_KSup = 180,    //Surface Reservoir Decay (h)
            //    FLT_CS = 10,      //Soil Saturation Capacity (mm)
            //    FLT_CC = 0.3,     //Field Capacity (%)
            //    FLT_CR = 0.1,    //Recharge Capacity (%)
            //    FLT_PP = 0.5,    //Deep Percolation (mm/h)
            //    FLT_KSub = 1080,   //Aquifer Reservoir Decay (d)
            //    FLT_KCan = 96,     //Channel Reservoir Decay (h)
            //    FLT_CH = 1.5,    //Hydraulic Conductivity (mm/h)
            //    FLT_FS = 500,      //Soil Capilarity Factor (mm)
            //    FLT_PS = 0.5,     //Soil Porosity (cm3/cm3)
            //    FLT_UI = 0.3     //Initial Moisture (cm3/cm3)

            //};

            //SimUpstream.DTE_Arr_TimeSeries[0] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);            
            //for(int i = 1; i < SimUpstream.DTE_Arr_TimeSeries.Length; i++)
            //{
            //    SimUpstream.DTE_Arr_TimeSeries[i] = SimUpstream.DTE_Arr_TimeSeries[0].AddHours(SimUpstream.FLT_TimeStep * i);            
            //}

            //PLASH.Run(SimUpstream, new double[SimulationLength], SimulationLength);


            //PLASH SimDownstream = new PLASH()
            //{
            //    FLT_AD = 727.8917,    //Watershed Area (km2)
            //    FLT_AI = 0.02,     //Impervious Area Fraction (km2/km2)
            //    FLT_AP = 0.95,    //Pervious Area Fraction (km2/km2)
            //    FLT_TimeStep = 24,
            //    DTE_Arr_TimeSeries = new DateTime[SimulationLength],
            //    FLT_Arr_QtObsSeries = InputQObs.ToArray(),
            //    FLT_Arr_PrecipSeries = InputPrecipDown.ToArray(),
            //    FLT_Arr_EPSeries = InputEvap.ToArray(),

            //    //Parameters
            //    FLT_DI = 5,       //Maximum Impervious Detention (mm)                
            //    FLT_IP = 3,       //Maximum Interception (mm)
            //    FLT_DP = 6,       //Maximum Pervious Detention (mm)
            //    FLT_KSup = 162,    //Surface Reservoir Decay (h)
            //    FLT_CS = 10,      //Soil Saturation Capacity (mm)
            //    FLT_CC = 0.3,     //Field Capacity (%)
            //    FLT_CR = 0.1,    //Recharge Capacity (%)
            //    FLT_PP = 0.5,    //Deep Percolation (mm/h)
            //    FLT_KSub = 720,   //Aquifer Reservoir Decay (d)
            //    FLT_KCan = 24,     //Channel Reservoir Decay (h)
            //    FLT_CH = 50,    //Hydraulic Conductivity (mm/h)
            //    FLT_FS = 500,      //Soil Capilarity Factor (mm)
            //    FLT_PS = 0.5,     //Soil Porosity (cm3/cm3)
            //    FLT_UI = 0.3     //Initial Moisture (cm3/cm3)

            //};


            //SimDownstream.DTE_Arr_TimeSeries[0] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //for (int i = 1; i < SimDownstream.DTE_Arr_TimeSeries.Length; i++)
            //{
            //    SimDownstream.DTE_Arr_TimeSeries[i] = SimUpstream.DTE_Arr_TimeSeries[0].AddHours(SimUpstream.FLT_TimeStep * i);
            //}

            //Muskingum DampenedUpstream = new Muskingum()
            //{
            //    FLT_K = 24,
            //    FLT_X = 0.01,
            //    FLT_Timestep = 24,
            //    FLT_Arr_InputFlow = SimUpstream.FLT_Arr_Qt_Calc
            //};

            //DampenedUpstream.FLT_Arr_OutputFlow = Muskingum.ProcessDamping(DampenedUpstream);

            ////for(int i = 0; i < DampenedUpstream.FLT_Arr_InputFlow.Length; i++)
            ////{
            ////    Console.WriteLine("Inflow: {0}, Outflow: {1}", DampenedUpstream.FLT_Arr_InputFlow[i], DampenedUpstream.FLT_Arr_OutputFlow[i]);
            ////}
            ////Console.ReadKey();
            //PLASH.Run(SimDownstream, DampenedUpstream.FLT_Arr_OutputFlow, SimulationLength);

            bool Teste = false;
           
            var chromosome = new FloatingPointChromosome(
                new double[] { 0, 0, 0, 24, 0,
                               0, 0, 0, 120, 6,
                               0, 0, 0, 0,

                               0, 0, 0, 24, 0,
                               0, 0, 0, 120, 6,
                               0, 0, 0, 0,

                               0, 0
                },
                new double[] {
                    10, 10, 10, 360, 100,
                    0.5, 0.5, 10, 3600, 240,
                    3, 500, 1, 1,

                    10, 10, 10, 360, 100,
                    0.5, 0.5, 10, 3600, 240,
                    30, 200, 1, 1,

                    240, 0.5
                },
                new int[] { 64, 64, 64, 64, 64,
                            64, 64, 64, 64, 64,
                            64, 64, 64, 64,

                            64, 64, 64, 64, 64,
                            64, 64, 64, 64, 64,
                            64, 64, 64, 64,

                            64, 64
                },
                new int[] { 5, 5, 5, 5, 5,
                            5, 5, 5, 5, 5,
                            5, 5, 5, 5,

                            5, 5, 5, 5, 5,
                            5, 5, 5, 5, 5,
                            5, 5, 5, 5,

                            5, 5

                });

            var population = new Population(50, 100, chromosome);

            var fitness = new FuncFitness((c) =>
            {
                var fc = c as FloatingPointChromosome;

                var values = fc.ToFloatingPoints();

                DateTime[] TimeSeries = new DateTime[SimulationLength];
                TimeSeries[0] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                for (int i = 1; i < TimeSeries.Length; i++)
                {
                    TimeSeries[i] = TimeSeries[0].AddHours(Timestep* i);
                }

                PLASHInput InputUp = new PLASHInput
                {
                    DTE_Arr_TimeSeries = TimeSeries,
                    FLT_Arr_PrecipSeries = InputPrecipUp.ToArray(),
                    FLT_Arr_EPSeries = InputEvap.ToArray(),
                    FLT_Arr_QtObsSeries = InputQObs.ToArray(),
                    FLT_Arr_QtUpstream = new double[SimulationLength]
                };

                PLASHParameters ParamUp = new PLASHParameters
                {
                    FLT_AD = 861.42252,
                    FLT_AI = 0.02,     
                    FLT_AP = 0.95,    
                    FLT_TimeStep = 24,

                    FLT_DI = values[0],
                    FLT_IP = values[1],
                    FLT_DP = values[2],
                    FLT_KSup = values[3],
                    FLT_CS = values[4],
                    FLT_CC = values[5],
                    FLT_CR = values[6],
                    FLT_PP = values[7],
                    FLT_KSub = values[8],
                    FLT_KCan = values[9],
                    FLT_CH = values[10],
                    FLT_FS = values[11],
                    FLT_PS = values[12],
                    FLT_UI = values[13]
                };

                PLASHReservoir ReservoirUp = new PLASHReservoir();

                PLASHOutput OutputUp = new PLASHOutput();

                PLASH.Run(InputUp, ParamUp, ReservoirUp, OutputUp);


                Muskingum Musk = new Muskingum()
                {
                    FLT_K = values[28],
                    FLT_X = values[29],
                    FLT_Timestep = Timestep,
                    FLT_Arr_InputFlow = OutputUp.FLT_Arr_Qt_Calc
                };

                
                PLASHInput InputDown = new PLASHInput()
                {
                    DTE_Arr_TimeSeries = TimeSeries,
                    FLT_Arr_PrecipSeries = InputPrecipDown.ToArray(),
                    FLT_Arr_EPSeries = InputEvap.ToArray(),
                    FLT_Arr_QtObsSeries = InputQObs.ToArray(),
                    FLT_Arr_QtUpstream = Muskingum.ProcessDamping(Musk)
                };

                PLASHParameters ParamDown = new PLASHParameters
                {
                    FLT_AD = 727.8917,      //Watershed Area (km2)
                    FLT_AI = 0.02,          //Impervious Area Fraction (km2/km2)
                    FLT_AP = 0.95,          //Pervious Area Fraction (km2/km2)
                    FLT_TimeStep = 24,


                    FLT_DI = values[14],  //Maximum Impervious Detention (mm)                
                    FLT_IP = values[15],    //Maximum Interception (mm)
                    FLT_DP = values[16],    //Maximum Pervious Detention (mm)
                    FLT_KSup = values[17],  //Surface Reservoir Decay (h)
                    FLT_CS = values[18],    //Soil Saturation Capacity (mm)
                    FLT_CC = values[19],    //Field Capacity (%)
                    FLT_CR = values[20],    //Recharge Capacity (%)
                    FLT_PP = values[21],    //Deep Percolation (mm/h)
                    FLT_KSub = values[22],  //Aquifer Reservoir Decay (d)
                    FLT_KCan = values[23],  //Channel Reservoir Decay (h)
                    FLT_CH = values[24],    //Hydraulic Conductivity (mm/h)
                    FLT_FS = values[25],    //Soil Capilarity Factor (mm)
                    FLT_PS = values[26],    //Soil Porosity (cm3/cm3)
                    FLT_UI = values[27]     //Initial Moisture (cm3/cm3)
                };

                PLASHReservoir ReservoirDown = new PLASHReservoir();

                PLASHOutput OutputDown = new PLASHOutput();

                PLASH.Run(InputDown, ParamDown, ReservoirDown, OutputDown);

                
                double objectiveNashSut = 1;   

                double MeanSimulatedFlow = OutputDown.FLT_Arr_Qt_Calc.Average();

                double NashSutUpper = 0;
                double NashSutLower = 0;


                for(int i = 0; i < OutputDown.FLT_Arr_Qt_Calc.Length; i++)
                {
                    NashSutUpper += Math.Pow(OutputDown.FLT_Arr_Qt_Calc[i] - InputDown.FLT_Arr_QtObsSeries[i], 2);
                    NashSutLower += Math.Pow(InputDown.FLT_Arr_QtObsSeries[i] - MeanSimulatedFlow, 2);
                }

                objectiveNashSut -= (NashSutUpper / NashSutLower);

                double objectiveSquareSum = 0;
                for (int i = 0; i < OutputDown.FLT_Arr_Qt_Calc.Length; i++)
                {
                    objectiveSquareSum += Math.Pow(OutputDown.FLT_Arr_Qt_Calc[i] - InputDown.FLT_Arr_QtObsSeries[i],2);
                }

                double objectiveAbsSum = 0;
                for(int i = 0; i < OutputDown.FLT_Arr_Qt_Calc.Length; i++)
                {
                    objectiveAbsSum +=  Math.Abs(OutputDown.FLT_Arr_Qt_Calc[i] - InputDown.FLT_Arr_QtObsSeries[i]);
                }

                return objectiveAbsSum * -1;

                //return objectiveSquareSum * -1;

                //return objectiveNashSut;


            });

            var selection = new EliteSelection();
            var crossover = new UniformCrossover(0.5f);
            var mutation = new FlipBitMutation();
            var termination = new FitnessStagnationTermination(100);

            var ga = new GeneticAlgorithm(
                population,
                fitness,
                selection,
                crossover,
                mutation);

            ga.Termination = termination;

            //Console.WriteLine("Generation: (x1, y1), (x2, y2) = distance");
            Console.WriteLine("Genetic algorithm tests");
            var latestFitness = 0.0;

            ga.GenerationRan += (sender, e) =>
            {
                var bestChromosome = ga.BestChromosome as FloatingPointChromosome;
                var bestFitness = bestChromosome.Fitness.Value;

                if (bestFitness != latestFitness)
                {
                    latestFitness = bestFitness;
                    var phenotype = bestChromosome.ToFloatingPoints();

                    Console.WriteLine("Generation {0}: {1}",

                        ga.GenerationsNumber,
                        bestFitness);
                    
                    //Console.WriteLine(
                    //    "Generation {0,2}: ({1},{2}),({3},{4}) = {5}",
                    //    ga.GenerationsNumber,
                    //    phenotype[0],
                    //    phenotype[1],
                    //    phenotype[2],
                    //    phenotype[3],
                    //    bestFitness
                    //);
                }
            };

            ga.Start();

            Console.WriteLine("GA Over!");

            var bestChrom = ga.BestChromosome as FloatingPointChromosome;
            var bestVal = bestChrom.ToFloatingPoints();

            foreach(double Param in bestVal)
            {
                Console.WriteLine(Param);
            }
            



            PLASH SimUpstream = new PLASH()
            {
                FLT_AD = 861.42252,    //Watershed Area (km2)
                FLT_AI = 0.02,     //Impervious Area Fraction (km2/km2)
                FLT_AP = 0.95,    //Pervious Area Fraction (km2/km2)
                FLT_TimeStep = 24,
                DTE_Arr_TimeSeries = new DateTime[SimulationLength],
                FLT_Arr_QtObsSeries = InputQObs.ToArray(),
                FLT_Arr_PrecipSeries = InputPrecipUp.ToArray(),
                FLT_Arr_EPSeries = InputEvap.ToArray(),

                //Parameters
                FLT_DI = bestVal[0],        //Maximum Impervious Detention (mm)                
                FLT_IP = bestVal[1],        //Maximum Interception (mm)
                FLT_DP = bestVal[2],           //Maximum Pervious Detention (mm)
                FLT_KSup = bestVal[3],         //Surface Reservoir Decay (h)
                FLT_CS = bestVal[4],         //Soil Saturation Capacity (mm)
                FLT_CC = bestVal[5],          //Field Capacity (%)
                FLT_CR = bestVal[6],          //Recharge Capacity (%)
                FLT_PP = bestVal[7],          //Deep Percolation (mm/h)
                FLT_KSub = bestVal[8],         //Aquifer Reservoir Decay (d)
                FLT_KCan = bestVal[9],        //Channel Reservoir Decay (h)
                FLT_CH = bestVal[10],        //Hydraulic Conductivity (mm/h)
                FLT_FS = bestVal[11],          //Soil Capilarity Factor (mm)
                FLT_PS = bestVal[12],          //Soil Porosity (cm3/cm3)
                FLT_UI = bestVal[13]          //Initial Moisture (cm3/cm3)

            };

            SimUpstream.DTE_Arr_TimeSeries[0] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            for (int i = 1; i < SimUpstream.DTE_Arr_TimeSeries.Length; i++)
            {
                SimUpstream.DTE_Arr_TimeSeries[i] = SimUpstream.DTE_Arr_TimeSeries[0].AddHours(SimUpstream.FLT_TimeStep * i);
            }

            PLASH.Run(SimUpstream, new double[SimulationLength], SimulationLength);


            PLASH SimDownstream = new PLASH()
            {
                FLT_AD = 727.8917,    //Watershed Area (km2)
                FLT_AI = 0.02,     //Impervious Area Fraction (km2/km2)
                FLT_AP = 0.95,    //Pervious Area Fraction (km2/km2)
                FLT_TimeStep = 24,
                DTE_Arr_TimeSeries = new DateTime[SimulationLength],
                FLT_Arr_QtObsSeries = InputQObs.ToArray(),
                FLT_Arr_PrecipSeries = InputPrecipDown.ToArray(),
                FLT_Arr_EPSeries = InputEvap.ToArray(),

                //Parameters
                FLT_DI = bestVal[14],        //Maximum Impervious Detention (mm)                
                FLT_IP = bestVal[15],        //Maximum Interception (mm)
                FLT_DP = bestVal[16],           //Maximum Pervious Detention (mm)
                FLT_KSup = bestVal[17],         //Surface Reservoir Decay (h)
                FLT_CS = bestVal[18],         //Soil Saturation Capacity (mm)
                FLT_CC = bestVal[19],          //Field Capacity (%)
                FLT_CR = bestVal[20],          //Recharge Capacity (%)
                FLT_PP = bestVal[21],          //Deep Percolation (mm/h)
                FLT_KSub = bestVal[22],         //Aquifer Reservoir Decay (d)
                FLT_KCan = bestVal[23],        //Channel Reservoir Decay (h)
                FLT_CH = bestVal[24],        //Hydraulic Conductivity (mm/h)
                FLT_FS = bestVal[25],          //Soil Capilarity Factor (mm)
                FLT_PS = bestVal[26],          //Soil Porosity (cm3/cm3)
                FLT_UI = bestVal[27]         //Initial Moisture (cm3/cm3)

            };


            SimDownstream.DTE_Arr_TimeSeries[0] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            for (int i = 1; i < SimDownstream.DTE_Arr_TimeSeries.Length; i++)
            {
                SimDownstream.DTE_Arr_TimeSeries[i] = SimDownstream.DTE_Arr_TimeSeries[0].AddHours(SimDownstream.FLT_TimeStep * i);
            }

            Muskingum DampenedUpstream = new Muskingum()
            {
                FLT_K = bestVal[28],
                FLT_X = bestVal[29],
                FLT_Timestep = 24,
                FLT_Arr_InputFlow = SimUpstream.FLT_Arr_Qt_Calc
            };

            DampenedUpstream.FLT_Arr_OutputFlow = Muskingum.ProcessDamping(DampenedUpstream);

            //for(int i = 0; i < DampenedUpstream.FLT_Arr_InputFlow.Length; i++)
            //{
            //    Console.WriteLine("Inflow: {0}, Outflow: {1}", DampenedUpstream.FLT_Arr_InputFlow[i], DampenedUpstream.FLT_Arr_OutputFlow[i]);
            //}
            //Console.ReadKey();
            PLASH.Run(SimDownstream, DampenedUpstream.FLT_Arr_OutputFlow, SimulationLength);
            

            Console.ReadKey();



            //Console.WriteLine("");
            //Console.ReadKey();

            #endregion PLASH Simulation

            #region Buwo Simulation
            Buildup_Washoff BuWoModel = new Buildup_Washoff
            {
                FLT_BMax = 10,
                FLT_Kb = 0.1,
                FLT_Nb = 1,
                FLT_Kw = 0.1,
                FLT_Nw = 1
            };

            Buildup_Washoff.fncBuildupWashoffProcess(BuWoModel, SimUpstream.FLT_Arr_ESSup, SimUpstream.FLT_AD, 0, 2, 1, SimUpstream.FLT_TimeStep, 0.5);

            //double[] FLT_Arr_WashoffResults = BuWoModel.fncBuildupWashoffProcess(Sim.FLT_Arr_PrecipSeries, Sim.FLT_Arr_ESSup, Sim.FLT_AD, 0, 2, 1, Sim.FLT_TimeStep);

            //Console.ReadKey();

            #endregion Buwo Simulation



            //#region Excel Output
            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Param_PLASHUp");
                excel.Workbook.Worksheets.Add("Param_PLASHDown");
                excel.Workbook.Worksheets.Add("Results_PLASHUp");
                excel.Workbook.Worksheets.Add("Results_PLASHDown");
                excel.Workbook.Worksheets.Add("Results_Muskingum");
                excel.Workbook.Worksheets.Add("Results_BuWo");

                var HeaderRowPLASHParam = new List<string[]>()
                {
                    new string[]
                    {
                        "DI", "IP", "DP", "KSup", "CS", "CC", "CR", "PP", "Ksub", "KCan", "CH", "FS", "PS", "UI"
                    }
                };

                string headerRangePLASHParam = "A1:" + Char.ConvertFromUtf32(HeaderRowPLASHParam[0].Length + 64) + 1;

                var worksheet = excel.Workbook.Worksheets["Param_PLASHUp"];

                worksheet.Cells[headerRangePLASHParam].LoadFromArrays(HeaderRowPLASHParam);

                List<object[]> cellDataPLASHParamUP = new List<object[]>();

                cellDataPLASHParamUP.Add(new object[]
                {
                    SimUpstream.FLT_DI, SimUpstream.FLT_IP, SimUpstream.FLT_DP, SimUpstream.FLT_KSup, SimUpstream.FLT_CS,
                    SimUpstream.FLT_CC, SimUpstream.FLT_CR, SimUpstream.FLT_PP, SimUpstream.FLT_KSub, SimUpstream.FLT_KCan,
                    SimUpstream.FLT_CH, SimUpstream.FLT_FS, SimUpstream.FLT_PS, SimUpstream.FLT_UI
                });

                worksheet.Cells[2, 1].LoadFromArrays(cellDataPLASHParamUP);

                worksheet = excel.Workbook.Worksheets["Param_PLASHDown"];

                worksheet.Cells[headerRangePLASHParam].LoadFromArrays(HeaderRowPLASHParam);

                List<object[]> cellDataPLASHParamDown = new List<object[]>();

                cellDataPLASHParamDown.Add(new object[]
                {
                    SimDownstream.FLT_DI, SimDownstream.FLT_IP, SimDownstream.FLT_DP, SimDownstream.FLT_KSup, SimDownstream.FLT_CS,
                    SimDownstream.FLT_CC, SimDownstream.FLT_CR, SimDownstream.FLT_PP, SimDownstream.FLT_KSub, SimDownstream.FLT_KCan,
                    SimDownstream.FLT_CH, SimDownstream.FLT_FS, SimDownstream.FLT_PS, SimDownstream.FLT_UI
                });

                worksheet.Cells[2, 1].LoadFromArrays(cellDataPLASHParamDown);



                //PLASH Upstream
                var HeaderRowPLASH = new List<string[]>()
                {
                    new string[] {"Precipitation", "Evapotranspiration", "Observed Flow",
                        "Impervious Reservoir", "Interception Reservoir", "Surface Reservoir", "Soil Reservoir", "Aquifer Reservoir", "Channel Reservoir",
                    "Calculated Basic Flow", "Calculated Surface Flow", "Calculated Total Flow"},
                };

                string headerRangePLASH = "A1:" + Char.ConvertFromUtf32(HeaderRowPLASH[0].Length + 64) + 1;

                worksheet = excel.Workbook.Worksheets["Results_PLASHUp"];

                worksheet.Cells[headerRangePLASH].LoadFromArrays(HeaderRowPLASH);

                List<object[]> cellDataPLASHUp = new List<object[]>();

                for (int i = 0; i < SimulationLength; i++)
                {
                    cellDataPLASHUp.Add(new object[] { SimUpstream.FLT_Arr_PrecipSeries[i], SimUpstream.FLT_Arr_EPSeries[i], SimUpstream.FLT_Arr_QtObsSeries[i],
                    SimUpstream.FLT_Arr_RImp[i], SimUpstream.FLT_Arr_RInt[i], SimUpstream.FLT_Arr_RSup[i], SimUpstream.FLT_Arr_RSol[i], SimUpstream.FLT_Arr_RSub[i], SimUpstream.FLT_Arr_RCan[i],
                    SimUpstream.FLT_Arr_QBas_Calc[i], SimUpstream.FLT_Arr_QSup_Calc[i], SimUpstream.FLT_Arr_Qt_Calc[i]});
                }

                worksheet.Cells[2, 1].LoadFromArrays(cellDataPLASHUp);

                //PLASH Downstream

                worksheet = excel.Workbook.Worksheets["Results_PLASHDown"];

                worksheet.Cells[headerRangePLASH].LoadFromArrays(HeaderRowPLASH);

                List<object[]> cellDataPLASHDown = new List<object[]>();


                for (int i = 0; i < SimulationLength; i++)
                {
                    cellDataPLASHDown.Add(new object[] { SimDownstream.FLT_Arr_PrecipSeries[i], SimDownstream.FLT_Arr_EPSeries[i], SimDownstream.FLT_Arr_QtObsSeries[i],
                    SimDownstream.FLT_Arr_RImp[i], SimDownstream.FLT_Arr_RInt[i], SimDownstream.FLT_Arr_RSup[i], SimDownstream.FLT_Arr_RSol[i], SimDownstream.FLT_Arr_RSub[i], SimUpstream.FLT_Arr_RCan[i],
                    SimDownstream.FLT_Arr_QBas_Calc[i], SimDownstream.FLT_Arr_QSup_Calc[i], SimDownstream.FLT_Arr_Qt_Calc[i]});
                }

                worksheet.Cells[2, 1].LoadFromArrays(cellDataPLASHDown);

                //Muskingum

                var HeaderRowMuskingum = new List<string[]>()
                {
                    new string[] { "Upstream Flow", "Downstream Flow"},
                };

                string headerRangeMuskingum = "A1:" + Char.ConvertFromUtf32(HeaderRowMuskingum[0].Length + 64) + 1;


                worksheet = excel.Workbook.Worksheets["Results_Muskingum"];

                worksheet.Cells[headerRangeMuskingum].LoadFromArrays(HeaderRowMuskingum);

                List<object[]> cellDataMuskingum = new List<object[]>();


                for (int i = 0; i < SimulationLength; i++)
                {
                    cellDataMuskingum.Add(new object[] { DampenedUpstream.FLT_Arr_InputFlow[i], DampenedUpstream.FLT_Arr_OutputFlow[i] });
                }

                worksheet.Cells[2, 1].LoadFromArrays(cellDataMuskingum);



                //Buwo

                var HeaderRow2 = new List<string[]>()
                {
                    new string[] {"Precipitation", "Surface Flow", "Buildup", "Washoff"},
                };

                string headerRange2 = "A1:" + Char.ConvertFromUtf32(HeaderRow2[0].Length + 64) + 1;

                worksheet = excel.Workbook.Worksheets["Results_BuWo"];

                worksheet.Cells[headerRange2].LoadFromArrays(HeaderRow2);

                List<object[]> cellDataBuwo = new List<object[]>();

                for (int i = 0; i < SimulationLength; i++)
                {
                    cellDataBuwo.Add(new object[] { SimUpstream.FLT_Arr_PrecipSeries[i], SimUpstream.FLT_Arr_ESSup[i], BuWoModel.FLT_Arr_Buildup[i], BuWoModel.FLT_Arr_Washoff[i] });
                }

                worksheet.Cells[2, 1].LoadFromArrays(cellDataBuwo);

                FileInfo excelFile = new FileInfo(@"D:\data.xlsx");
                excel.SaveAs(excelFile);
            }

            ////Console.WriteLine("Excel processed");

            //#endregion Excel Output

            //Console.ReadKey();



        }
    }
}
