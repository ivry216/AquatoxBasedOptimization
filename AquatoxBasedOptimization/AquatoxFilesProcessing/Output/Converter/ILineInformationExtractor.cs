using System;
using System.Collections.Generic;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Output.Converter
{
    public interface ILineInformationExtractor
    {
        Dictionary<string, int> VariablesAndIndices { get; }
        (DateTime Date, Dictionary<string, double> Variables) ExtractData(string line);
    }
}
