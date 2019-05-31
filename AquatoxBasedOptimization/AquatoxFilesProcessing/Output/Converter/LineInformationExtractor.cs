using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Output.Converter
{
    public class LineInformationExtractor : ILineInformationExtractor
    {
        // 
        private const int _numberOfDescriptions = 2;

        public Dictionary<string, int> VariablesAndIndices { get; private set; }

        public LineInformationExtractor(Dictionary<string, int> variablesAndIndices)
        {
            VariablesAndIndices = variablesAndIndices;
        }

        // TODO: get rid of dictionaries?
        public (DateTime Date, Dictionary<string, double> Variables) ExtractData(string line)
        {
            // Preprocess the sting to get only the values
            var processedLine = line
                .Replace("}", "")
                .Replace("}", "")
                .Replace("Results at ", " ")
                .Replace("n=", "")
                .Replace(" ", "")
                .Split(';');

            // Parse date
            var date = DateTime.Parse(processedLine[0]);

            // Check if there are variables and indices
            if (VariablesAndIndices == null)
                throw new Exception("Variables and indices are undefined");
            if (VariablesAndIndices.Count == 0)
                throw new Exception("There are no variables given for parcing");

            // Values
            Dictionary<string, double> values = new Dictionary<string, double>();
            foreach (var nameIndexPair in VariablesAndIndices)
            {
                values.Add(nameIndexPair.Key, double.Parse(processedLine[nameIndexPair.Value + _numberOfDescriptions]));
            }

            return (date, values);
        }
    }
}
