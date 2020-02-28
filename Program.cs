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



namespace PlashClasses
{   
    
    class Program
    {

        static void Main(string[] args)
        {
            
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

            #region Genetic Algorithm
            int SimulationLength = InputPrecipUp.Count;
            double Timestep = 24;
            
            var chromosome = new FloatingPointChromosome(
                new double[] { 0, 0, 2, 24, 0, //Param Upstream
                               0, 0, 0, 120, 6,
                               0, 0, 0, 0,

                               0, 0, 0, 0, //Initial Upstream

                               0, 0, 2, 24, 0, //Param Downstream
                               0, 0, 0, 120, 6,
                               0, 0, 0, 0,

                               0, 0, 0, 0, //Initial Downstream

                               12, 0.01 //Param Muskingum
                },
                new double[] {
                    10, 10, 10, 240, 300,
                    0.5, 0.5, 10, 3600, 120,
                    3, 500, 1, 1,

                    10, 10, 10, 10,

                    10, 10, 10, 240, 300,
                    0.5, 0.5, 10, 3600, 120,
                    20, 200, 1, 1,

                    10, 10, 10, 10,

                    180, 0.5
                },
                new int[] { 64, 64, 64, 64, 64,
                            64, 64, 64, 64, 64,
                            64, 64, 64, 64,

                            64, 64, 64, 64,

                            64, 64, 64, 64, 64,
                            64, 64, 64, 64, 64,
                            64, 64, 64, 64,

                            64, 64, 64, 64,

                            64, 64
                },
                new int[] { 3, 3, 3, 3, 3,
                            3, 3, 3, 3, 3,
                            3, 3, 3, 3,

                            3, 3, 3, 3,

                            3, 3, 3, 3, 3,
                            3, 3, 3, 3, 3,
                            3, 3, 3, 3,

                            3, 3, 3, 3,

                            3, 3

                });

            var population = new Population(50, 100, chromosome);

            var fitness = new FuncFitness((c) =>
            {
                var fc = c as FloatingPointChromosome;

                var values = fc.ToFloatingPoints();
                if(values[12] < values[13] || values[30] < values[31])
                {
                    return double.NegativeInfinity;
                }
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

                PLASHInitialConditions Res0Up = new PLASHInitialConditions()
                {
                    RImp0 = values[14],
                    RInt0 = values[15],
                    RSup0 = values[16],
                    RCan0 = values[17]
                };

                PLASHReservoir ReservoirUp = new PLASHReservoir();

                PLASHOutput OutputUp = new PLASHOutput();

                PLASH.Run(InputUp, ParamUp, Res0Up, ReservoirUp, OutputUp);


                Muskingum Musk = new Muskingum()
                {
                    FLT_K = values[36],
                    FLT_X = values[37],
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


                    FLT_DI = values[18],  //Maximum Impervious Detention (mm)                
                    FLT_IP = values[19],    //Maximum Interception (mm)
                    FLT_DP = values[20],    //Maximum Pervious Detention (mm)
                    FLT_KSup = values[21],  //Surface Reservoir Decay (h)
                    FLT_CS = values[22],    //Soil Saturation Capacity (mm)
                    FLT_CC = values[23],    //Field Capacity (%)
                    FLT_CR = values[24],    //Recharge Capacity (%)
                    FLT_PP = values[25],    //Deep Percolation (mm/h)
                    FLT_KSub = values[26],  //Aquifer Reservoir Decay (d)
                    FLT_KCan = values[27],  //Channel Reservoir Decay (h)
                    FLT_CH = values[28],    //Hydraulic Conductivity (mm/h)
                    FLT_FS = values[29],    //Soil Capilarity Factor (mm)
                    FLT_PS = values[30],    //Soil Porosity (cm3/cm3)
                    FLT_UI = values[31]     //Initial Moisture (cm3/cm3)
                };

                PLASHInitialConditions Res0Down = new PLASHInitialConditions()
                {
                    RImp0 = values[32],
                    RInt0 = values[33],
                    RSup0 = values[34],
                    RCan0 = values[35]
                };

                PLASHReservoir ReservoirDown = new PLASHReservoir();

                PLASHOutput OutputDown = new PLASHOutput();

                PLASH.Run(InputDown, ParamDown, Res0Down, ReservoirDown, OutputDown);

                if(ReservoirDown.FLT_Arr_ESSup.Sum() < 30 || ReservoirUp.FLT_Arr_ESSup.Sum() < 30)
                {
                    return double.NegativeInfinity;
                }

                //double objectiveNashSut = 1;   

                //double MeanSimulatedFlow = OutputDown.FLT_Arr_Qt_Calc.Average();

                //double NashSutUpper = 0;
                //double NashSutLower = 0;


                //for(int i = 0; i < OutputDown.FLT_Arr_Qt_Calc.Length; i++)
                //{
                //    NashSutUpper += Math.Pow(OutputDown.FLT_Arr_Qt_Calc[i] - InputDown.FLT_Arr_QtObsSeries[i], 2);
                //    NashSutLower += Math.Pow(InputDown.FLT_Arr_QtObsSeries[i] - MeanSimulatedFlow, 2);
                //}

                //objectiveNashSut -= (NashSutUpper / NashSutLower);

                double objectiveSquareSum = 0;
                for (int i = 0; i < OutputDown.FLT_Arr_Qt_Calc.Length; i++)
                {
                    objectiveSquareSum += Math.Pow(OutputDown.FLT_Arr_Qt_Calc[i] - InputDown.FLT_Arr_QtObsSeries[i], 2);
                }

                //double objectiveAbsSum = 0;
                //for(int i = 0; i < OutputDown.FLT_Arr_Qt_Calc.Length; i++)
                //{
                //    objectiveAbsSum +=  Math.Abs(OutputDown.FLT_Arr_Qt_Calc[i] - InputDown.FLT_Arr_QtObsSeries[i]);
                //}

                //return objectiveAbsSum * -1;

                return objectiveSquareSum * -1;

                //return objectiveNashSut;


            });

            var selection = new EliteSelection();
            var crossover = new UniformCrossover(0.3f);
            var mutation = new FlipBitMutation();
            var termination = new FitnessStagnationTermination(250);

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

            #endregion Genetic Algorithm
            var bestChrom = ga.BestChromosome as FloatingPointChromosome;
            var bestVal = bestChrom.ToFloatingPoints();


            PLASHInput InputUpstream = new PLASHInput()
            {
                DTE_Arr_TimeSeries = new DateTime[SimulationLength],
                FLT_Arr_PrecipSeries = InputPrecipUp.ToArray(),
                FLT_Arr_EPSeries = InputEvap.ToArray(),
                FLT_Arr_QtObsSeries = InputQObs.ToArray(),
                FLT_Arr_QtUpstream = new double[SimulationLength]
            };
            
            PLASHParameters ParametersUpstream = new PLASHParameters()
            {
                FLT_AD = 861.42252,    //Watershed Area (km2)
                FLT_AI = 0.02,     //Impervious Area Fraction (km2/km2)
                FLT_AP = 0.95,    //Pervious Area Fraction (km2/km2)
                FLT_TimeStep = 24,
                
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

            InputUpstream.DTE_Arr_TimeSeries[0] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            for (int i = 1; i < InputUpstream.DTE_Arr_TimeSeries.Length; i++)
            {
                InputUpstream.DTE_Arr_TimeSeries[i] = InputUpstream.DTE_Arr_TimeSeries[0].AddHours(ParametersUpstream.FLT_TimeStep * i);
            }

            PLASHReservoir ReservoirUpstream = new PLASHReservoir();

            PLASHInitialConditions InitialUpstream = new PLASHInitialConditions()
            {
                RImp0 = bestVal[14],
                RInt0 = bestVal[15],
                RSup0 = bestVal[16],
                RCan0 = bestVal[17]
            };

            PLASHOutput OutputUpstream = new PLASHOutput();

            PLASH.Run(InputUpstream, ParametersUpstream, InitialUpstream, ReservoirUpstream, OutputUpstream);


            PLASHInput InputDownstream = new PLASHInput()
            {
                DTE_Arr_TimeSeries = new DateTime[SimulationLength],
                FLT_Arr_PrecipSeries = InputPrecipDown.ToArray(),
                FLT_Arr_EPSeries = InputEvap.ToArray(),
                FLT_Arr_QtObsSeries = InputQObs.ToArray()                
            };

            PLASHParameters ParametersDownstream = new PLASHParameters()
            {
                FLT_AD = 727.8917,    //Watershed Area (km2)
                FLT_AI = 0.02,     //Impervious Area Fraction (km2/km2)
                FLT_AP = 0.95,    //Pervious Area Fraction (km2/km2)
                FLT_TimeStep = 24,

                //Parameters
                FLT_DI = bestVal[18],        //Maximum Impervious Detention (mm)                
                FLT_IP = bestVal[19],        //Maximum Interception (mm)
                FLT_DP = bestVal[20],           //Maximum Pervious Detention (mm)
                FLT_KSup = bestVal[21],         //Surface Reservoir Decay (h)
                FLT_CS = bestVal[22],         //Soil Saturation Capacity (mm)
                FLT_CC = bestVal[23],          //Field Capacity (%)
                FLT_CR = bestVal[24],          //Recharge Capacity (%)
                FLT_PP = bestVal[25],          //Deep Percolation (mm/h)
                FLT_KSub = bestVal[26],         //Aquifer Reservoir Decay (d)
                FLT_KCan = bestVal[27],        //Channel Reservoir Decay (h)
                FLT_CH = bestVal[28],        //Hydraulic Conductivity (mm/h)
                FLT_FS = bestVal[29],          //Soil Capilarity Factor (mm)
                FLT_PS = bestVal[30],          //Soil Porosity (cm3/cm3)
                FLT_UI = bestVal[31]          //Initial Moisture (cm3/cm3)

            };


            InputDownstream.DTE_Arr_TimeSeries[0] = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            for (int i = 1; i < InputDownstream.DTE_Arr_TimeSeries.Length; i++)
            {
                InputDownstream.DTE_Arr_TimeSeries[i] = InputDownstream.DTE_Arr_TimeSeries[0].AddHours(ParametersDownstream.FLT_TimeStep * i);
            }

            PLASHReservoir ReservoirDownstream = new PLASHReservoir();

            PLASHInitialConditions InitialDownstream = new PLASHInitialConditions()
            {
                RImp0 = bestVal[32],
                RInt0 = bestVal[33],
                RSup0 = bestVal[34],
                RCan0 = bestVal[35]
            };

            PLASHOutput OutputDownstream = new PLASHOutput();

            Muskingum DampenedUpstream = new Muskingum()
            {
                FLT_K = bestVal[36],
                FLT_X = bestVal[37],
                FLT_Timestep = 24,
                FLT_Arr_InputFlow = OutputUpstream.FLT_Arr_Qt_Calc
            };

            DampenedUpstream.FLT_Arr_OutputFlow = Muskingum.ProcessDamping(DampenedUpstream);

            InputDownstream.FLT_Arr_QtUpstream = DampenedUpstream.FLT_Arr_OutputFlow;

            PLASH.Run(InputDownstream, ParametersDownstream, InitialDownstream, ReservoirDownstream, OutputDownstream);
            

            Console.ReadKey();



            //Console.WriteLine("");
            //Console.ReadKey();

            #endregion PLASH Simulation

            #region Buwo Simulation

            List<Buildup_Washoff> UsesUpstream = Buildup_Washoff.BuwoUpstreamList(Timestep, ReservoirUpstream.FLT_Arr_ESSup);
            List<Buildup_Washoff> UsesDownstream = Buildup_Washoff.BuwoDownstreamList(Timestep, ReservoirDownstream.FLT_Arr_ESSup);
            
            foreach(Buildup_Washoff Use in UsesUpstream)
            {
                Buildup_Washoff.fncBuildupWashoffProcess(Use);
            }

            Buildup_Washoff BuwoUpstream = Buildup_Washoff.AggregateUses(UsesUpstream, 863.178D);
            
            foreach(Buildup_Washoff Use in UsesDownstream)
            {
                Buildup_Washoff.fncBuildupWashoffProcess(Use);
            }
            
            Buildup_Washoff BuwoDownstream = Buildup_Washoff.AggregateUses(UsesDownstream, 729.018D);

            

            Buildup_Washoff Aggregate = Buildup_Washoff.Transport(BuwoUpstream, BuwoDownstream);
            
            
            #endregion Buwo Simulation



            //#region Excel Output
            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Param_PLASHUp");
                excel.Workbook.Worksheets.Add("Param_PLASHDown");
                excel.Workbook.Worksheets.Add("PLASHReservoirUp");
                excel.Workbook.Worksheets.Add("PLASHReservoirDown");
                excel.Workbook.Worksheets.Add("PLASHInitialUp");
                excel.Workbook.Worksheets.Add("PLASHInitialDown");
                excel.Workbook.Worksheets.Add("Results_PLASHUp");
                excel.Workbook.Worksheets.Add("Results_PLASHDown");
                excel.Workbook.Worksheets.Add("Param_Muskingum");
                excel.Workbook.Worksheets.Add("Results_Muskingum");
                excel.Workbook.Worksheets.Add("Results_BuWo");

                #region Parameters

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
                    ParametersUpstream.FLT_DI, ParametersUpstream.FLT_IP, ParametersUpstream.FLT_DP, ParametersUpstream.FLT_KSup, ParametersUpstream.FLT_CS,
                    ParametersUpstream.FLT_CC, ParametersUpstream.FLT_CR, ParametersUpstream.FLT_PP, ParametersUpstream.FLT_KSub, ParametersUpstream.FLT_KCan,
                    ParametersUpstream.FLT_CH, ParametersUpstream.FLT_FS, ParametersUpstream.FLT_PS, ParametersUpstream.FLT_UI
                });

                worksheet.Cells[2, 1].LoadFromArrays(cellDataPLASHParamUP);

                worksheet = excel.Workbook.Worksheets["Param_PLASHDown"];

                worksheet.Cells[headerRangePLASHParam].LoadFromArrays(HeaderRowPLASHParam);

                List<object[]> cellDataPLASHParamDown = new List<object[]>();

                cellDataPLASHParamDown.Add(new object[]
                {
                    ParametersDownstream.FLT_DI, ParametersDownstream.FLT_IP, ParametersDownstream.FLT_DP, ParametersDownstream.FLT_KSup, ParametersDownstream.FLT_CS,
                    ParametersDownstream.FLT_CC, ParametersDownstream.FLT_CR, ParametersDownstream.FLT_PP, ParametersDownstream.FLT_KSub, ParametersDownstream.FLT_KCan,
                    ParametersDownstream.FLT_CH, ParametersDownstream.FLT_FS, ParametersDownstream.FLT_PS, ParametersDownstream.FLT_UI
                });

                worksheet.Cells[2, 1].LoadFromArrays(cellDataPLASHParamDown);

                #endregion Parameters

                #region Reservoir
                var HeaderRowPLASHreservoir = new List<string[]>()
                {
                    new string[]
                    {
                        "ImpRes", "ImpEvap", "ImpFlow",
                        "IntRes", "IntEvap", "IntFlow",
                        "SurfRes", "SurfEvap", "SurfFlow",
                        "Inf", "InfCum", "IAE", "TP", "IAEAdim", "TPAdim",
                        "SoilRes", "SoilEvap", "SoilUpFlow", "SoilDownFlow",
                        "AquiRes", "AquiPerc", "AquiFlow",
                        "ChanRes", "ChanEvap", "ChanUpFlow", "ChanDownFlow"
                    }
                };

                string HeaderRangePLASHReservoir = "A1:" + Char.ConvertFromUtf32(HeaderRowPLASHreservoir[0].Length + 64) + 1;

                worksheet = excel.Workbook.Worksheets["PLASHReservoirUp"];

                worksheet.Cells[HeaderRangePLASHReservoir].LoadFromArrays(HeaderRowPLASHreservoir);

                List<object[]> cellDataPLASHResUp = new List<object[]>();

                for(int i = 0; i < SimulationLength; i++)
                {
                    cellDataPLASHResUp.Add(new object[]
                    {
                        ReservoirUpstream.FLT_Arr_RImp[i], ReservoirUpstream.FLT_Arr_ERImp[i], ReservoirUpstream.FLT_Arr_ESImp[i],
                        ReservoirUpstream.FLT_Arr_RInt[i], ReservoirUpstream.FLT_Arr_ERInt[i], ReservoirUpstream.FLT_Arr_ESInt[i],
                        ReservoirUpstream.FLT_Arr_RSup[i], ReservoirUpstream.FLT_Arr_ERSup[i], ReservoirUpstream.FLT_Arr_ESSup[i],
                        ReservoirUpstream.FLT_Arr_Infiltration[i], ReservoirUpstream.FLT_Arr_Infiltration_Cumulative[i], ReservoirUpstream.FLT_Arr_IAE[i], ReservoirUpstream.FLT_Arr_TP[i], ReservoirUpstream.FLT_Arr_IAEAdim[i], ReservoirUpstream.FLT_Arr_TPAdim[i],
                        ReservoirUpstream.FLT_Arr_RSol[i], ReservoirUpstream.FLT_Arr_ERSol[i], ReservoirUpstream.FLT_Arr_EESol[i], ReservoirUpstream.FLT_Arr_ESSol[i],
                        ReservoirUpstream.FLT_Arr_RSub[i], ReservoirUpstream.FLT_Arr_PPSub[i], ReservoirUpstream.FLT_Arr_EESub[i],
                        ReservoirUpstream.FLT_Arr_RCan[i], ReservoirUpstream.FLT_Arr_ERCan[i], ReservoirUpstream.FLT_Arr_EECan[i], ReservoirUpstream.FLT_ARR_ESCan[i]
                    });
                }

                worksheet.Cells[2, 1].LoadFromArrays(cellDataPLASHResUp);


                worksheet = excel.Workbook.Worksheets["PLASHReservoirDown"];

                worksheet.Cells[HeaderRangePLASHReservoir].LoadFromArrays(HeaderRowPLASHreservoir);

                List<object[]> cellDataPLASHResDown = new List<object[]>();

                for (int i = 0; i < SimulationLength; i++)
                {
                    cellDataPLASHResDown.Add(new object[]
                    {
                        ReservoirDownstream.FLT_Arr_RImp[i], ReservoirDownstream.FLT_Arr_ERImp[i], ReservoirDownstream.FLT_Arr_ESImp[i],
                        ReservoirDownstream.FLT_Arr_RInt[i], ReservoirDownstream.FLT_Arr_ERInt[i], ReservoirDownstream.FLT_Arr_ESInt[i],
                        ReservoirDownstream.FLT_Arr_RSup[i], ReservoirDownstream.FLT_Arr_ERSup[i], ReservoirDownstream.FLT_Arr_ESSup[i],
                        ReservoirDownstream.FLT_Arr_Infiltration[i], ReservoirDownstream.FLT_Arr_Infiltration_Cumulative[i], ReservoirDownstream.FLT_Arr_IAE[i], ReservoirDownstream.FLT_Arr_TP[i], ReservoirDownstream.FLT_Arr_IAEAdim[i], ReservoirDownstream.FLT_Arr_TPAdim[i],
                        ReservoirDownstream.FLT_Arr_RSol[i], ReservoirDownstream.FLT_Arr_ERSol[i], ReservoirDownstream.FLT_Arr_EESol[i], ReservoirDownstream.FLT_Arr_ESSol[i],
                        ReservoirDownstream.FLT_Arr_RSub[i], ReservoirDownstream.FLT_Arr_PPSub[i], ReservoirDownstream.FLT_Arr_EESub[i],
                        ReservoirDownstream.FLT_Arr_RCan[i], ReservoirDownstream.FLT_Arr_ERCan[i], ReservoirDownstream.FLT_Arr_EECan[i], ReservoirDownstream.FLT_ARR_ESCan[i]
                    });
                }

                worksheet.Cells[2, 1].LoadFromArrays(cellDataPLASHResDown);

                #endregion Reservoir

                #region Initial Conditions
                var HeaderRowPLASHInitial = new List<string[]>()
                {
                    new string[] {"RImp0", "RInt0", "RSup0", "RCan0" }
                };

                string headerRangePLASHInitial = "A1:" + Char.ConvertFromUtf32(HeaderRowPLASHInitial[0].Length + 64) + 1;

                worksheet = excel.Workbook.Worksheets["PLASHInitialUp"];

                worksheet.Cells[headerRangePLASHInitial].LoadFromArrays(HeaderRowPLASHInitial);

                List<object[]> cellDataPLASHInitialUp = new List<object[]>();

                cellDataPLASHInitialUp.Add(new object[] { InitialUpstream.RImp0, InitialUpstream.RInt0, InitialUpstream.RSup0, InitialUpstream.RCan0 });

                worksheet.Cells[2, 1].LoadFromArrays(cellDataPLASHInitialUp);


                worksheet = excel.Workbook.Worksheets["PLASHInitialDown"];

                worksheet.Cells[headerRangePLASHInitial].LoadFromArrays(HeaderRowPLASHInitial);

                List<object[]> cellDataPLASHInitialDown = new List<object[]>();

                cellDataPLASHInitialDown.Add(new object[] { InitialDownstream.RImp0, InitialDownstream.RInt0, InitialDownstream.RSup0, InitialDownstream.RCan0 });

                worksheet.Cells[2, 1].LoadFromArrays(cellDataPLASHInitialDown);

                #endregion Initial Conditions

                #region Results

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
                    cellDataPLASHUp.Add(new object[] { InputUpstream.FLT_Arr_PrecipSeries[i], InputUpstream.FLT_Arr_EPSeries[i], InputUpstream.FLT_Arr_QtObsSeries[i],
                    ReservoirUpstream.FLT_Arr_RImp[i], ReservoirUpstream.FLT_Arr_RInt[i], ReservoirUpstream.FLT_Arr_RSup[i], ReservoirUpstream.FLT_Arr_RSol[i], ReservoirUpstream.FLT_Arr_RSub[i], ReservoirUpstream.FLT_Arr_RCan[i],
                    OutputUpstream.FLT_Arr_QBas_Calc[i], OutputUpstream.FLT_Arr_QSup_Calc[i], OutputUpstream.FLT_Arr_Qt_Calc[i]});
                }

                worksheet.Cells[2, 1].LoadFromArrays(cellDataPLASHUp);

                //PLASH Downstream

                worksheet = excel.Workbook.Worksheets["Results_PLASHDown"];

                worksheet.Cells[headerRangePLASH].LoadFromArrays(HeaderRowPLASH);

                List<object[]> cellDataPLASHDown = new List<object[]>();


                for (int i = 0; i < SimulationLength; i++)
                {
                    cellDataPLASHDown.Add(new object[] { InputDownstream.FLT_Arr_PrecipSeries[i], InputDownstream.FLT_Arr_EPSeries[i], InputDownstream.FLT_Arr_QtObsSeries[i],
                    ReservoirDownstream.FLT_Arr_RImp[i], ReservoirDownstream.FLT_Arr_RInt[i], ReservoirDownstream.FLT_Arr_RSup[i], ReservoirDownstream.FLT_Arr_RSol[i], ReservoirDownstream.FLT_Arr_RSub[i], ReservoirDownstream.FLT_Arr_RCan[i],
                    OutputDownstream.FLT_Arr_QBas_Calc[i], OutputDownstream.FLT_Arr_QSup_Calc[i], OutputDownstream.FLT_Arr_Qt_Calc[i]});
                }

                worksheet.Cells[2, 1].LoadFromArrays(cellDataPLASHDown);

                #endregion Results


                #region Muskingum Parameters
                //Muskingum

                var HeaderRowMuskingumParam = new List<string[]>()
                {
                    new string[] { "K", "X"},
                };

                string headerRangeMuskingumParam = "A1:" + Char.ConvertFromUtf32(HeaderRowMuskingumParam[0].Length + 64) + 1;


                worksheet = excel.Workbook.Worksheets["Param_Muskingum"];

                worksheet.Cells[headerRangeMuskingumParam].LoadFromArrays(HeaderRowMuskingumParam);

                List<object[]> cellDataMuskingumParam = new List<object[]>();

                cellDataMuskingumParam.Add(new object[] { DampenedUpstream.FLT_K, DampenedUpstream.FLT_X });
                

                worksheet.Cells[2, 1].LoadFromArrays(cellDataMuskingumParam);

                #endregion Muskingum Parameters

                #region Muskingum
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

                #endregion Muskingum

                #region BuWo
                //Buwo

                var HeaderRow2 = new List<string[]>()
                {
                    new string[] {"Precipitation", "Surface Flow", "Buildup", "Effective Washoff"},
                };

                string headerRange2 = "A1:" + Char.ConvertFromUtf32(HeaderRow2[0].Length + 64) + 1;

                worksheet = excel.Workbook.Worksheets["Results_BuWo"];

                worksheet.Cells[headerRange2].LoadFromArrays(HeaderRow2);

                List<object[]> cellDataBuwo = new List<object[]>();

                for (int i = 0; i < SimulationLength; i++)
                {
                    cellDataBuwo.Add(new object[] { InputUpstream.FLT_Arr_PrecipSeries[i], ReservoirDownstream.FLT_Arr_ESSup[i], Aggregate.FLT_Arr_Buildup[i], Aggregate.FLT_Arr_EffectiveWashoff[i] });
                }

                worksheet.Cells[2, 1].LoadFromArrays(cellDataBuwo);

                #endregion BuWo

                FileInfo excelFile = new FileInfo(@"D:\dataGA.xlsx");
                excel.SaveAs(excelFile);
            }

            ////Console.WriteLine("Excel processed");

            //#endregion Excel Output

            //Console.ReadKey();



        }
    }
}
