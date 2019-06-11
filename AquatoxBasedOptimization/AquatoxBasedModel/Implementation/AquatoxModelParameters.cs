using Optimization.OptimizationProblems.Models;
using System.Collections.Generic;
using System.IO;

namespace AquatoxBasedOptimization.AquatoxBasedModel.Implementation
{
    public class AquatoxModelParameters : IModelParameters
    {
        public string InputFilePath { get; } = @"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/STUDIES/Lake Pyhajarvi Finland Template.txt";
        public string AquatoxExecutablePath { get; } = @"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/PROGRAM/aquatox.exe";
        public string CurrentDirectory { get; } = Directory.GetCurrentDirectory();

        public string NumericRepresentationFormat { get; } = "0.00000000000000E+0000";

        public Dictionary<string, string> InputParameters { get; set; }

        public string BuildInputFileName(int id)
        {
            return CurrentDirectory + "\\Input" + id + ".txt";
        }

        public string BuildOutputFileName(int id)
        {
            return "Output_" + id + ".txt";
        }

        public string BuildAquatoxRunningCommand(int id)
        {
            return "EPSAVE " + BuildInputFileName(id) + " \"" + BuildOutputFileName(id) + "\"";
        }
    }
}
