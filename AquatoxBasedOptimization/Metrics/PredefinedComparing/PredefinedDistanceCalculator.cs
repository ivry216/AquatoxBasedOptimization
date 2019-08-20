using AquatoxBasedOptimization.Data;
using System;
using System.Linq;


namespace AquatoxBasedOptimization.Metrics.PredefinedComparing
{
    // TODO: Make it predefined
    public class PredefinedDistanceCalculator
    {
        private (DateTime[] earlierDates, DateTime[] laterDates, double[] earlierValue, double[] laterValues) GetEarlieAndLaterData(ITimeSeries output, ITimeSeries observations)
        {
            var outputDates = output.Times.Select(date => date.Date).ToArray();
            var observationDates = observations.Times.Select(date => date.Date).ToArray();

            DateTime[] startingEarlieDates;
            DateTime[] startingLaterDates;
            double[] startingEarlierValues;
            double[] startingLaterValues;

            // Get the time series that is starting earlier
            if (outputDates.Min() <= observationDates.Min())
            {
                startingEarlieDates = outputDates;
                startingLaterDates = observationDates;
                startingEarlierValues = output.Values;
                startingLaterValues = observations.Values;
            }
            else
            {
                startingLaterDates = outputDates;
                startingEarlieDates = observationDates;
                startingEarlierValues = observations.Values;
                startingLaterValues = output.Values;
            }

            return (startingEarlieDates, startingLaterDates, startingEarlierValues, startingLaterValues);
        }

        public double CalculateDistance(ITimeSeries output, ITimeSeries observations)
        {
            var (startingEarlieDates, startingLaterDates, startingEarlierValues, startingLaterValues) = GetEarlieAndLaterData(output, observations);

            double distance = 0;

            // TODO: make a precalculation of indices to calculate the distances 
            int secondDateArrayIndex = 0;
            for (int i = 0; i < startingLaterDates.Length; i++)
            {
                while(!startingEarlieDates[secondDateArrayIndex].Equals(startingLaterDates[i]))
                {
                    secondDateArrayIndex++;

                    if (secondDateArrayIndex >= startingEarlieDates.Length)
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
