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

            // Date
            DateTime[] startingEarlier;
            DateTime[] startingLater;
            // Values
            double[] startingEarlierValues;
            double[] startingLaterValues;

            // Get the time series that is starting earlier
            if (outputDates.Min() <= observationDates.Min())
            {
                startingEarlier = outputDates;
                startingLater = observationDates;
                startingEarlierValues = output.Values;
                startingLaterValues = observations.Values;
            }
            else
            {
                startingLater = outputDates;
                startingEarlier = observationDates;
                startingEarlierValues = observations.Values;
                startingLaterValues = output.Values;
            }


            int secondDateArrayIndex = 0;
            for (int i = 0; i < startingLater.Length; i++)
            {
                while(!startingEarlier[secondDateArrayIndex].Equals(startingLater[i]))
                {
                    secondDateArrayIndex++;

                    if (secondDateArrayIndex >= startingEarlier.Length)
                    {
                        return distance;
                    }
                }

                distance += Math.Abs(startingLaterValues[i] - startingEarlierValues[secondDateArrayIndex]);
            }

            return distance;
        }
    }
}
