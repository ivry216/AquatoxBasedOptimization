using System.IO;

namespace AquatoxBasedOptimization.AquatoxBasedModel.Implementation
{
    public class AquatoxModelParameters : IModelParameters
    {
        public string InputFilePath { get; } = @"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/STUDIES/Lake Pyhajarvi Finland Template.txt";
        public string AquatoxExecutablePath { get; } = @"C:/Users/ivanry/fixed_aquatox/AQUATOX R3.2/PROGRAM/aquatox.exe";
        public string CurrentDirectory { get; } = Directory.GetCurrentDirectory();

        public string NumericRepresentationFormat { get; } = "0.00000000000000E+0000";

        public string PerformOutputFileName(int filenameId)
        {
            return "ModelOutput_" + filenameId + ".txt";
        }

        public string BuildAquatoxRunningCommand(string inputFileName, string outputFileName)
        {
            return "EPSAVE " + inputFileName + " \"" + outputFileName + "\"";
        }
    }
}
