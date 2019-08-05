using AquatoxBasedOptimization.AquatoxBasedModel.Implementation;
using AquatoxBasedOptimization.AquatoxBasedProblem.Implementation;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Input;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Output;
using AquatoxBasedOptimization.Data;
using AquatoxBasedOptimization.Data.OutputObservations;
using AquatoxBasedOptimization.Data.OutputVariables;
using AquatoxBasedOptimization.ExternalProgramOperating;
using AquatoxBasedOptimization.Metrics.PredefinedComparing;
using Optimization.AlgorithmsControl.AlgorithmRunStatisticsInfrastructure.IterationStatistics;
using Optimization.EvolutionaryAlgorithms.DifferentialEvolutionAlgorithm;
using Optimization.EvolutionaryAlgorithms.DifferentialEvolutionAlgorithm.Parallel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading output variables.");

            // Read variable names and indices in output file
            IOutputVariablesReader outputVariablesReader = new OutputVariablesReaderFromExcel();
            Dictionary<string, int> variablesAndIndices = outputVariablesReader.Read();

            // Read the observations file
            OutputObservationsReaderFromExcel outputObservationsReader = new OutputObservationsReaderFromExcel();
            var observations = outputObservationsReader.ReadOutputVariableObservations();

            Console.WriteLine("Starting...");

            //simpleSingleLauncher.Run();

            string inputFileTemp = @"C:/Users/ivanry/Documents/Repositories/AquatoxBasedOptimization/AquatoxBasedOptimization/JupyterNotebooks/Lake Pyhajarvi Finland.txt";
            List<string> parameters = new List<string> { "_param1_", "_param2_", "_param3_", "_param4_" };
            AquatoxInputFileProcessor inputFileProcessor = new AquatoxInputFileProcessor(inputFileTemp, parameters);

            //
            AquatoxOutputFileProcessor outputFileProcessor = new AquatoxOutputFileProcessor(variablesAndIndices); 

            ConcurrentBag<Dictionary<string, ITimeSeries>> bag = new ConcurrentBag<Dictionary<string, ITimeSeries>>();
            ConcurrentBag<double> bagOfDistances = new ConcurrentBag<double>();

            PredefinedDistanceCalculator distanceCalculator = new PredefinedDistanceCalculator();

            SimpleSingleLauncher simpleSingleLauncher = new SimpleSingleLauncher();
            simpleSingleLauncher.File = new FileInfo(@"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/PROGRAM/aquatox.exe");


            AquatoxModelParameters modelParameters = new AquatoxModelParameters();
            modelParameters.InputParameters = new Dictionary<string, string>() { { "par1", "_param1_" }, { "par2", "_param2_" }, { "par3", "_param3_" }, { "par4", "_param4_" } };
            AquatoxModel model = new AquatoxModel(outputFileProcessor);
            model.SetParameters(modelParameters);

            ConcurrentBag<AquatoxModelOutput> outputs = new ConcurrentBag<AquatoxModelOutput>();

            int dimension = modelParameters.InputParameters.Count;

            AquatoxParametersTuningProblem tuningProblem = new AquatoxParametersTuningProblem(dimension);
            tuningProblem.SetDistanceCalculator(distanceCalculator);
            tuningProblem.SetObservations(observations);
            tuningProblem.SetModel(model);

            BestAlternativeHistoryMaker bestAltHistMaker = new BestAlternativeHistoryMaker();
            IterationValuesHistoryMaker iterationHistoryMaker = new IterationValuesHistoryMaker();

            DifferentialEvolutionParameters differentialEvolutionParameters = new DifferentialEvolutionParameters();
            differentialEvolutionParameters.CrossoverProbability = 0.5;
            differentialEvolutionParameters.DifferentialWeight = 1;
            differentialEvolutionParameters.GenerationParameters.GenerationFrom = Enumerable.Repeat(0.0, dimension).ToArray();
            differentialEvolutionParameters.GenerationParameters.GenerationTo = Enumerable.Repeat(10.0, dimension).ToArray();
            differentialEvolutionParameters.GenerationParameters.GenerationType = Optimization.EvolutionaryAlgorithms.PopulationGenerationType.Uniform;
            differentialEvolutionParameters.Iterations = 1;
            differentialEvolutionParameters.Size = 20;

            ParallelDifferentialEvolution differentialEvolutionParallel = new ParallelDifferentialEvolution();
            differentialEvolutionParallel.SetParameters(differentialEvolutionParameters);
            differentialEvolutionParallel.SetProblem(tuningProblem);
            differentialEvolutionParallel.AddStatsFollowers(new List<IAlgorithmIterationFollower> { bestAltHistMaker, iterationHistoryMaker });
            differentialEvolutionParallel.Evaluate();

            bestAltHistMaker.SaveToFile();
            iterationHistoryMaker.SaveToFile();

            Console.WriteLine("End!");
            Console.Read();
        }
    }
}