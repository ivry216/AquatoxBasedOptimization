using System;
using System.Collections.Generic;
using System.Linq;
using AquatoxBasedOptimization.Data;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Output.Converter
{
    public class TimeSeriesBuilder : ITimeSeriesBuilder
    {
        public Dictionary<string, int> VariableIndexPair { get; private set; }

        private string[] Names => VariableIndexPair.Keys.ToArray();
        private int[] Indices => VariableIndexPair.Values.ToArray();

        public TimeSeriesBuilder(Dictionary<string, int> variableIndexPair)
        {
            VariableIndexPair = variableIndexPair;
        }

        public Dictionary<string, ITimeSeries> Build(List<(DateTime Date, Dictionary<string, double> Variables)> parsedData)
        {
            // Size of time serieses
            int size = parsedData.Count;

            // Make a dict
            Dictionary<string, ITimeSeries> result = new Dictionary<string, ITimeSeries>();

            // Values
            DateTime[] dateTimes = new DateTime[size];
            double[][] values = new double[VariableIndexPair.Count][];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = new double[size];
            }

            // Run through all measurements
            for (int i = 0; i < parsedData.Count; i++)
            {
                dateTimes[i] = parsedData[i].Date;
                for (int j = 0; j < Indices.Length; j++)
                {
                    values[j][i] = parsedData[i].Variables[Names[j]];
                }
            }

            // Make time series
            for (int i = 0; i < Names.Length; i++)
            {
                result.Add(Names[i], new TimeSeries(Names[i], values[i], dateTimes));
            }

            return result;
        }
    }
}
