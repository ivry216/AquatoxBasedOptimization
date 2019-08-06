using OptimizationProblems.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AquatoxBasedOptimization.AquatoxBasedModel.Implementation
{
    public class AquatoxModelParameters : IModelParameters
    {
        private Dictionary<string, string> _inputParameters;

        public string InputFilePath { get; } = @"C:/Users/ivanry/Documents/Repositories/AquatoxBasedOptimization/AquatoxBasedOptimization/JupyterNotebooks/Lake Pyhajarvi Finland.txt";
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
    }
}
