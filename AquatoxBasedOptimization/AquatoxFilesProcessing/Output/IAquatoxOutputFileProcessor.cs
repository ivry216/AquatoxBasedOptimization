using AquatoxBasedOptimization.Data;
using System.Collections.Generic;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Output
{
    public interface IAquatoxOutputFileProcessor
    {
        Dictionary<string, int> OutputVariables { get; }
        Dictionary<string, ITimeSeries> ReadOutputs(string pathToRead);
    }
}
