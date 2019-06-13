using OptimizationProblems.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AquatoxBasedOptimization.AquatoxBasedModel.Implementation
{
    public class AquatoxModelParameters : IModelParameters
    {
        private Dictionary<string, string> _inputParameters;

        public string InputFilePath { get; } = @"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/STUDIES/Lake Pyhajarvi Finland Template.txt";
        public string AquatoxExecutablePath { get; } = @"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/PROGRAM/aquatox.exe";
        public string CurrentDirectory { get; } = Directory.GetCurrentDirectory();

        public string NumericRepresentationFormat { get; } = "0.00000000000000E+0000";

        public Dictionary<string, string> InputParameters
        {
            get
            {
                return _inputParameters;
            }
            set
            {
                InputsInnerNames = value.Values.ToArray();
                _inputParameters = value;
            }
        }

        public string[] InputsInnerNames { get; private set; }

        public AquatoxModelParameters()
        {
            
        }

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

        public Dictionary<string, string> ConvertValuesToInput(double[] values)
        {
            Dictionary<string, string> input = new Dictionary<string, string>();
            for (int i = 0; i < InputsInnerNames.Length; i++)
            {
                input.Add(InputsInnerNames[i], values[i].ToString("0.00000000000000E+0000"));
            }

            return input;
        }
    }
}
