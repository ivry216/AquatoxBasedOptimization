using System.Collections.Generic;
using System.Linq;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Output.Converter
{
    public class DayObservationsGetter : IDayObservationsGetter
    {
        #region Fields

        private const string _label = "\"ObjID\": 1010";

        #endregion Fields

        #region Getter

        public List<string> GetLinesOfData(string[] allLines)
        {
            // Look for indices of lines, which contain labels
            var linesContainingLabel = allLines
                .Select((line, index) => (line, index))
                .Where(pair => pair.line.Contains(_label))
                .Select(pair => pair.index)
                .ToList();
            // Increment indices
            linesContainingLabel = linesContainingLabel.Select(index => index + 1).ToList();

            // Select the right lines
            var linesWithObservations = linesContainingLabel
                .Select(index => allLines[index])
                .ToList();

            return linesWithObservations;
        }

        #endregion Getter
    }
}
