using AquatoxBasedOptimization.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Output.Converter
{
    public interface ITimeSeriesBuilder
    {
        ITimeSeries Build(List<(DateTime Date, Dictionary<string, double> Variables)> parsedData);
    }
}
