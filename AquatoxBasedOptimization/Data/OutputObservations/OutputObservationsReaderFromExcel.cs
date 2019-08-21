using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AquatoxBasedOptimization.Data.OutputObservations
{
    public class OutputObservationsReaderFromExcel : IOutputObservationsReader
    {
        // Excel file name
        private readonly string _fileName = "OutputObservations.xlsx";

        // Excel table colnames used for data parsing
        private readonly string _timeFileColname = "Sampling time";
        private readonly string _depthFileColname = "Sample depth";
        private readonly string _oxygenFileColname = "Dissolved oxygen mg/l";
        private readonly string _chlorophyllFileColname = "Chlorophyll a µg/l";
        private readonly string _nitrogenFileColname = "Total nitrogen, unfiltered µg/l";
        private readonly string _phosphorusFileColname = "Total phosphorous, unfiltered µg/l";

        private readonly string _timeDtColname = "Datetime";
        private readonly string _depthDtColname = "Depth";
        private readonly string _oxygenDtColname = "Oxygen";
        private readonly string _chlorophyllDtColname = "Chlorophyll";
        private readonly string _nitrogenDtColname = "Nitrogen";
        private readonly string _phosphorusDtColname = "Phosphorus";

        private Dictionary<string, string> variablesNamesPairs;


        public void SetVariableNames(Dictionary<string, string> variables)
        {
            variablesNamesPairs = variables;
        }


        private (int Time, int Depth, int Oxygen) GetColumnIndices(ExcelWorksheet worksheet, int startingRow, int startingCol, int nCols)
        {
            string trialString;

            Dictionary<string, int?> wordsIndices = new Dictionary<string, int?> { { _timeFileColname, null }, { _depthFileColname, null }, { _oxygenFileColname, null }, { _chlorophyllFileColname, null }, { _nitrogenFileColname, null }, { _phosphorusFileColname, null } };
            List<string> wordsToFind = wordsIndices.Keys.ToList();

            bool toDelete = false;
            string thisWord = null;

            for (int i = startingCol; wordsToFind.Count != 0 || i <= nCols; i++)
            {
                toDelete = false;
                trialString = worksheet.Cells[startingRow, i].Value.ToString();
                foreach (var word in wordsToFind)
                {
                    if (trialString.Contains(word))
                    {
                        wordsToFind.Remove(word);
                        wordsIndices[word] = i;
                        thisWord = word;
                        toDelete = true;
                        break;
                    }
                }

                if (toDelete)
                {
                    wordsToFind.Remove(thisWord);
                }
            }

            if (wordsIndices.Values.Any(v => v == null))
            {
                throw new Exception($"Could not find {string.Join(", ", wordsIndices.Where(pair => pair.Value == null).Select(pair => pair.Key))}");
            }

            return (wordsIndices[_timeFileColname].Value, wordsIndices[_depthFileColname].Value, wordsIndices[_oxygenFileColname].Value);
        }


        public Dictionary<string, IOutputObservation> ReadOutputVariableObservations()
        {
            // TODO: Make checking if the names dictionary is empty or null

            var observations = new Dictionary<string, IOutputObservation>();

            using (ExcelPackage package = new ExcelPackage(new FileInfo(_fileName)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                int nRows = worksheet.Dimension.End.Row;
                int nCols = worksheet.Dimension.End.Column;

                var (timeIndex, depthIndex, oxygenIndex) = GetColumnIndices(worksheet, startingRow: 1, startingCol: 1, nCols: nCols);

                // Get all depths
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add(_timeDtColname, typeof(DateTime));
                dataTable.Columns.Add(_depthDtColname, typeof(string));
                dataTable.Columns.Add(_oxygenDtColname, typeof(string));

                for (int i = 2; i < nRows; i++)
                {
                    var newRow = dataTable.NewRow();
                    newRow[_timeDtColname] = DateTime.FromOADate(double.Parse(worksheet.Cells[i, timeIndex].Value.ToString()));
                    newRow[_depthDtColname] = worksheet.Cells[i, depthIndex].Value.ToString();
                    newRow[_oxygenDtColname] = worksheet.Cells[i, oxygenIndex].Value == null ? 
                        "" : worksheet.Cells[i, oxygenIndex].Value.ToString();
                    dataTable.Rows.Add(newRow);
                }

                // Get distinct depths
                List<string> distinctDepths = dataTable
                    .AsEnumerable()
                    .Select(row => row.Field<string>(_depthDtColname))
                    .Distinct()
                    .ToList();

                // Dictionary with observations for each particular depth
                var depthRelatedTimeseries = new Dictionary<string, ITimeSeries>();
                // Make an output for each depth
                foreach (var depth in distinctDepths)
                {
                    // Get the data for ts
                    List<(DateTime Timestamp, double Value)> timeseriesData = dataTable
                        .AsEnumerable()
                        .Select(row => (Depth: row.Field<string>(_depthDtColname), Timestamp: row.Field<DateTime>(_timeDtColname), Value: row.Field<string>(_oxygenDtColname)))
                        .Where(triple => triple.Depth.Equals(depth))
                        .Where(triple => !triple.Value.Equals(""))
                        .Select(triple => (triple.Timestamp, double.Parse(triple.Value)))
                        .ToList();

                    // Perform ts
                    TimeSeries timeseries = new TimeSeries(name: $"Oxygen, depth: {depth}", values: timeseriesData.Select(pair => pair.Value).ToArray(), times: timeseriesData.Select(pair => pair.Timestamp).ToArray());
                    // Add the ts to dictionary for this depth
                    depthRelatedTimeseries.Add(depth, timeseries);
                }

                var observation = new OutputObservation("Oxygen", depthRelatedTimeseries);
                observations.Add("Oxygen", observation);
            }

            return observations;
        }
    }
}
