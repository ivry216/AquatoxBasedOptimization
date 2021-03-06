﻿using OptimizationProblems.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AquatoxBasedOptimization.AquatoxBasedModel.Implementation
{
    public class AquatoxModelParameters : IModelParameters
    {
        private Dictionary<string, string> _inputParameters;

        //string inputFileTemp = @"C:/Users/ivanry/Documents/Repositories/AquatoxBasedOptimization/AquatoxBasedOptimization/JupyterNotebooks/Lake Pyhajarvi Finland.txt";
        //public string InputFilePath { get; } = @"C:/Users/Ivan/Repositiries/AquatoxBasedOptimization/JupyterNotebooks/Lake Pyhajarvi Finland.txt";
        public string InputFilePath { get; } = @"C:/Users/ivanry/Documents/Reps/aquaox_modeling/JupyterNotebooks/Lake Pyhajarvi Finland.txt";
        //public string AquatoxExecutablePath { get; } = @"C:/Users/Ivan/aquatox/AQUATOX R3.2/PROGRAM/aquatox.exe";
        public string AquatoxExecutablePath { get; } = @"D:/AQUATOX R3.2 fixed/PROGRAM/aquatox.exe";
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
