using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Output.Converter;
using AquatoxBasedOptimization.Data;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Output
{
    public class AquatoxOutputFileProcessor : IAquatoxOutputFileProcessor
    {
        #region Fields

        private IDayObservationsGetter _dayObservationGetter;
        private ILineInformationExtractor _lineInformationExtractor;
        private ITimeSeriesBuilder _timeSeriesBuilder;

        #endregion

        #region Properties

        public Dictionary<string, int> OutputVariables { get; private set; }

        #endregion

        #region Constructor

        public AquatoxOutputFileProcessor(Dictionary<string, int> outputVariables)
        {
            OutputVariables = outputVariables;

            _dayObservationGetter = new DayObservationsGetter();
            _lineInformationExtractor = new LineInformationExtractor(outputVariables);
            _timeSeriesBuilder = new TimeSeriesBuilder(outputVariables);
        }

        #endregion

        #region Main Methods

        public Dictionary<string, ITimeSeries> ReadOutputs(string pathToRead)
        {
            var fileStream = File.OpenRead(pathToRead);
            TextReader textReader = new StreamReader(fileStream);

            // TODO: possible we could stop reading lines, when found some symbol or line?
            string[] lines = textReader.ReadToEnd().Split('\n');

            // Getting the data for each day observations
            List<string> linesForDays = _dayObservationGetter.GetLinesOfData(lines);

            // TODO: Getting only the surface
            List<string> surfaceLinesForDays = linesForDays
                .Select((line, index) => (line, index))
                .Where(pair => pair.index < linesForDays.Count)
                .Select(pair => pair.line)
                .ToList();

            // Get the data for each day
            List<(DateTime Date, Dictionary<string, double> Variables)> dataForEachDay = new List<(DateTime Date, Dictionary<string, double> Variables)>();
            foreach (var lineForDay in linesForDays)
            {
                // Extract data for the particular day
                var dayData = _lineInformationExtractor.ExtractData(lineForDay);
                // Add the results to the collection
                dataForEachDay.Add((dayData.Date, dayData.Variables));
            }

            // Perform time series
            var timeSeries = _timeSeriesBuilder.Build(dataForEachDay);

            // Close the stream
            textReader.Dispose();
            fileStream.Dispose();

            return timeSeries;
        }

        #endregion
    }
}
