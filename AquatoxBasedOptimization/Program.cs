using AquatoxBasedOptimization.AquatoxBasedModel.Implementation;
using AquatoxBasedOptimization.AquatoxBasedProblem.Implementation;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Input;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Output;
using AquatoxBasedOptimization.Data;
using AquatoxBasedOptimization.Data.OutputObservations;
using AquatoxBasedOptimization.Data.OutputVariables;
using AquatoxBasedOptimization.ExternalProgramOperating;
using AquatoxBasedOptimization.Metrics.PredefinedComparing;
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
            //SimpleSingleLauncher simpleSingleLauncher = new SimpleSingleLauncher();
            //simpleSingleLauncher.File = new FileInfo(@"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/PROGRAM/aquatox.exe");
            //simpleSingleLauncher.SetParameters("EPSAVE \"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/STUDIES/Lake Pyhajarvi Finland.txt\" \"test.txt\"");

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
            List<string> parameters = new List<string> { "_param1_", "_param2_" };
            AquatoxInputFileProcessor inputFileProcessor = new AquatoxInputFileProcessor(inputFileTemp, parameters);

            //
            AquatoxOutputFileProcessor outputFileProcessor = new AquatoxOutputFileProcessor(variablesAndIndices); 

            ConcurrentBag<Dictionary<string, ITimeSeries>> bag = new ConcurrentBag<Dictionary<string, ITimeSeries>>();
            ConcurrentBag<double> bagOfDistances = new ConcurrentBag<double>();

            PredefinedDistanceCalculator distanceCalculator = new PredefinedDistanceCalculator();

            SimpleSingleLauncher simpleSingleLauncher = new SimpleSingleLauncher();
            simpleSingleLauncher.File = new FileInfo(@"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/PROGRAM/aquatox.exe");


            //Parallel.For(1, 10, (i) =>
            //{
            //    string inputFilePath = @"C:/Users/ivanry/Documents/Repositories/AquatoxBasedOptimization/AquatoxBasedOptimization/AquatoxBasedOptimization/bin/Debug/test_out_" + i + ".txt";
            //    var parametersValuesPairs = parameters.ToDictionary(item => item, item => i.ToString("0.00000000000000E+0000"));
            //    inputFileProcessor.SetParametersBySubstitution(inputFilePath, parametersValuesPairs);

            //    string resultiveFileName = "test" + i + ".txt";


            //    simpleSingleLauncher.SetParameters("EPSAVE " + inputFilePath + " \"" + resultiveFileName + "\"");
            //    simpleSingleLauncher.Run();

            //    var outputTest = outputFileProcessor.ReadOutputs(resultiveFileName);

            //    var dist = distanceCalculator.CalculateDistance(outputTest["Oxygen"], observations["Oxygen"].DepthRelatedObservations["1,0"]);

            //    bagOfDistances.Add(dist);
            //    bag.Add(outputTest);
            //});

            AquatoxModelParameters modelParameters = new AquatoxModelParameters();
            modelParameters.InputParameters = new Dictionary<string, string>() { { "par1", "_param1_" }, { "par2", "_param2_" } };
            AquatoxModel model = new AquatoxModel(outputFileProcessor);
            model.SetParameters(modelParameters);
            //AquatoxModelInput someModelInput = new AquatoxModelInput(new Dictionary<string, string> { { "_param1_", "_param1_" } });

            ConcurrentBag<AquatoxModelOutput> outputs = new ConcurrentBag<AquatoxModelOutput>();

            //Parallel.For(1, 2, (i) =>
            //{
            //    AquatoxModelInput someModelInput = new AquatoxModelInput(new Dictionary<string, string> { { "_param1_", i.ToString("0.00000000000000E+0000") } });
            //    model.SetInput(someModelInput, i);
            //    var result = model.Evaluate(i);

            //    outputs.Add(result);
            //});

            //var outputTest = outputFileProcessor.ReadOutputs("test1.txt");

            //
            //distanceCalculator.CalculateDistance(outputTest["Oxygen"], observations["Oxygen"].DepthRelatedObservations["1,0"]);

            //string[] strings = File.ReadAllLines("output.txt");
            //string[] stringsInput = File.ReadAllLines(@"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/STUDIES/Lake Pyhajarvi Finland.txt");

            int dimension = modelParameters.InputParameters.Count;

            AquatoxParametersTuningProblem tuningProblem = new AquatoxParametersTuningProblem(dimension);
            tuningProblem.SetDistanceCalculator(distanceCalculator);
            tuningProblem.SetObservations(observations);
            tuningProblem.SetModel(model);

            DifferentialEvolutionParameters differentialEvolutionParameters = new DifferentialEvolutionParameters();
            differentialEvolutionParameters.CrossoverProbability = 0.5;
            differentialEvolutionParameters.DifferentialWeight = 1;
            differentialEvolutionParameters.GenerationFrom = Enumerable.Repeat(0.0, dimension).ToArray();
            differentialEvolutionParameters.GenerationTo = Enumerable.Repeat(10.0, dimension).ToArray();
            differentialEvolutionParameters.GenerationType = Optimization.EvolutionaryAlgorithms.PopulationGenerationType.Uniform;
            differentialEvolutionParameters.Iterations = 10;
            differentialEvolutionParameters.Size = 10;

            ParallelDifferentialEvolution differentialEvolutionParallel = new ParallelDifferentialEvolution();
            differentialEvolutionParallel.SetParameters(differentialEvolutionParameters);
            differentialEvolutionParallel.SetProblem(tuningProblem);
            differentialEvolutionParallel.Evaluate();

            Console.WriteLine("End!");
            Console.Read();
        }
    }
}