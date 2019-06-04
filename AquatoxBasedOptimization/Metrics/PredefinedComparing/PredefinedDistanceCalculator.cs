using AquatoxBasedOptimization.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.Metrics.PredefinedComparing
{
    // TODO: Make it predefined
    public class PredefinedDistanceCalculator
    {
        public double CalculateDistance(ITimeSeries output, ITimeSeries observations)
        {
            // Get dates
            var outputDates = output.Times.Select(date => date.Date).ToArray();
            var observationDates = observations.Times.Select(date => date.Date).ToArray();

            // Run distance calculating
            double distance = 0;
            int observationIndex = 0;
            for (int i = 0; i < outputDates.Length; i++)
            {
                while(!observationDates[observationIndex].Equals(outputDates[i]))
                {
                    observationIndex++;

                    if (observationIndex >= observationDates.Length)
                    {
                        return distance;
                    }
                }

                distance += Math.Abs(output.Values[i] - observations.Values[observationIndex]);
            }

            return distance;
        }
    }
}
