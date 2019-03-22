using AquatoxBasedOptimization.AquatoxFilesProcessing.Input;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Input.ParametersWriters;
using AquatoxBasedOptimization.ExternalProgramOperating;
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

            Console.WriteLine("Starting...");

            //simpleSingleLauncher.Run();

            string inputFileTemp = @"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/STUDIES/Lake Pyhajarvi Finland Template.txt";
            List<string> parameters = new List<string> { "_param1_" };
            AquatoxInputFileProcessor inputFileProcessor = new AquatoxInputFileProcessor(inputFileTemp, parameters);

            //string fileToSave = @"C:/Users/ivanry/Documents/Repositories/AquatoxBasedOptimization/AquatoxBasedOptimization/AquatoxBasedOptimization/bin/Debug/test_out.txt";
            //var dict = parameters.ToDictionary(item => item, item => "1");
            //inputFileProcessor.SetParametersBySubstitution(fileToSave, dict);

            ConcurrentBag<string[]> bag = new ConcurrentBag<string[]>();

            Parallel.For(1, 10, (i) =>
            {
                string fileToSave = @"C:/Users/ivanry/Documents/Repositories/AquatoxBasedOptimization/AquatoxBasedOptimization/AquatoxBasedOptimization/bin/Debug/test_out_" + i + ".txt";
                var dict = parameters.ToDictionary(item => item, item => 0.ToString());
                inputFileProcessor.SetParametersBySubstitution(fileToSave, dict);
                SimpleSingleLauncher simpleSingleLauncher = new SimpleSingleLauncher();
                simpleSingleLauncher.File = new FileInfo(@"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/PROGRAM/aquatox.exe");

                string resultiveFileName = "test" + i + ".txt";
                simpleSingleLauncher.SetParameters("EPSAVE " + fileToSave + " \"" + resultiveFileName + "\"");
                simpleSingleLauncher.Run();

                bag.Add(File.ReadAllLines(resultiveFileName));
            });

            string[] strings = File.ReadAllLines("output.txt");
            string[] stringsInput = File.ReadAllLines(@"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/STUDIES/Lake Pyhajarvi Finland.txt");
            int counterStart = strings.Skip(1).Where(str => str.Contains("{")).Count();
            int counterEnd = strings.Skip(1).Where(str => str.Contains("}")).Count();

            int counterStartInput = stringsInput.Where(str => str.Contains("{")).Count();
            int counterEndInput = stringsInput.Where(str => str.Contains("}")).Count();


            Console.WriteLine("End!");
            Console.Read();
        }
    }
}