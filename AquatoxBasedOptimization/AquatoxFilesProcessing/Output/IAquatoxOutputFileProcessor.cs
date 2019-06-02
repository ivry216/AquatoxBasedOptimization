using AquatoxBasedOptimization.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Output
{
    public interface IAquatoxOutputFileProcessor
    {
        Dictionary<string, int> OutputVariables { get; }
        Dictionary<string, ITimeSeries> ReadOutputs(string pathToRead);
    }
}
