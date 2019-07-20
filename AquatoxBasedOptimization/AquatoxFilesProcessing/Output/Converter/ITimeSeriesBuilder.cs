using AquatoxBasedOptimization.Data;
using System;
using System.Collections.Generic;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Output.Converter
{
    public interface ITimeSeriesBuilder
    {
        Dictionary<string, int> VariableIndexPair { get; }

        // TODO: use collection instead?
        Dictionary<string, ITimeSeries> Build(List<(DateTime Date, Dictionary<string, double> Variables)> parsedData);
    }
}