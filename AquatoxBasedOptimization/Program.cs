using AquatoxBasedOptimization.AquatoxFilesProcessing.Input;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Input.ParametersWriters;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Output;
using AquatoxBasedOptimization.Data;
using AquatoxBasedOptimization.Data.OutputObservations;
using AquatoxBasedOptimization.Data.OutputVariables;
using AquatoxBasedOptimization.ExternalProgramOperating;
using AquatoxBasedOptimization.Metrics.PredefinedComparing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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

            string inputFileTemp = @"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/STUDIES/Lake Pyhajarvi Finland Template.txt";
            List<string> parameters = new List<string> { "_param1_" };
            AquatoxInputFileProcessor inputFileProcessor = new AquatoxInputFileProcessor(inputFileTemp, parameters);

            //
            AquatoxOutputFileProcessor outputFileProcessor = new AquatoxOutputFileProcessor(variablesAndIndices); 

            //string fileToSave = @"C:/Users/ivanry/Documents/Repositories/AquatoxBasedOptimization/AquatoxBasedOptimization/AquatoxBasedOptimization/bin/Debug/test_out.txt";
            //var dict = parameters.ToDictionary(item => item, item => "1");
            //inputFileProcessor.SetParametersBySubstitution(fileToSave, dict);

            ConcurrentBag<Dictionary<string, ITimeSeries>> bag = new ConcurrentBag<Dictionary<string, ITimeSeries>>();
            ConcurrentBag<double> bagOfDistances = new ConcurrentBag<double>();

            PredefinedDistanceCalculator distanceCalculator = new PredefinedDistanceCalculator();

            Parallel.For(1, 10, (i) =>
            {
                string fileToSave = @"C:/Users/ivanry/Documents/Repositories/AquatoxBasedOptimization/AquatoxBasedOptimization/AquatoxBasedOptimization/bin/Debug/test_out_" + i + ".txt";
                var dict = parameters.ToDictionary(item => item, item => i.ToString("0.00000000000000E+0000"));
                inputFileProcessor.SetParametersBySubstitution(fileToSave, dict);
                SimpleSingleLauncher simpleSingleLauncher = new SimpleSingleLauncher();
                simpleSingleLauncher.File = new FileInfo(@"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/PROGRAM/aquatox.exe");

                string resultiveFileName = "test" + i + ".txt";
                simpleSingleLauncher.SetParameters("EPSAVE " + fileToSave + " \"" + resultiveFileName + "\"");
                simpleSingleLauncher.Run();

                var outputTest = outputFileProcessor.ReadOutputs(resultiveFileName);

                var dist = distanceCalculator.CalculateDistance(outputTest["Oxygen"], observations["Oxygen"].DepthRelatedObservations["1,0"]);

                bagOfDistances.Add(dist);
                bag.Add(outputTest);
            });

            //var outputTest = outputFileProcessor.ReadOutputs("test1.txt");

            //
            //distanceCalculator.CalculateDistance(outputTest["Oxygen"], observations["Oxygen"].DepthRelatedObservations["1,0"]);

            string[] strings = File.ReadAllLines("output.txt");
            string[] stringsInput = File.ReadAllLines(@"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/STUDIES/Lake Pyhajarvi Finland.txt");

            Console.WriteLine("End!");
            Console.Read();
        }
    }
}