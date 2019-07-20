using System.Collections.Generic;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Output.Converter
{
    public interface IDayObservationsGetter
    {
        List<string> GetLinesOfData(string[] allLines);
    }
}
