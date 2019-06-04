﻿using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.Data.OutputObservations
{
    public class OutputObservationsReaderFromExcel : IOutputObservationsReader
    {
        // Excel file name
        private const string _fileName = "OutputObservations.xlsx";

        // Excel table colnames used for data parsing
        private const string _timeFileColname = "Sampling time";
        private const string _depthFileColname = "Sample depth";
        private const string _oxygenFileColname = "Dissolved oxygen mg/l";

        private const string _timeDtColname = "Datetime";
        private const string _depthDtColname = "Depth";
        private const string _oxygenDtColname = "Oxygen";

        // 
        private Dictionary<string, string> variablesNamesPairs;

        //
        public void SetVariableNames(Dictionary<string, string> variables)
        {
            variablesNamesPairs = variables;
        }

        public Dictionary<string, IOutputObservation> ReadOutputVariableObservations()
        {
            // TODO: Make checking if the names dictionary is empty or null

            // Initialize variables-indices pairs
            Dictionary<string, IOutputObservation> observations = new Dictionary<string, IOutputObservation>();

            FileInfo file = new FileInfo(_fileName);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                //get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                //
                int nRows = worksheet.Dimension.End.Row;
                int nCols = worksheet.Dimension.End.Column;
                //
                // Get time, depth and oxygen columns indices
                int timeIndex = 0, depthIndex = 0, oxygenIndex = 0;
                string trialString;
                for (int i = 1, j = 0; i <= nCols; i++, j++)
                {
                    trialString = worksheet.Cells[1, i].Value.ToString();
                    if (trialString.Contains(_timeFileColname))
                    {
                        timeIndex = i;
                    }
                    if (trialString.Contains(_depthFileColname))
                    {
                        depthIndex = i;
                    }
                    if (trialString.Contains(_oxygenFileColname))
                    {
                        oxygenIndex = i;
                    }
                }

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
                Dictionary<string, ITimeSeries> depthRelatedTimeseries = new Dictionary<string, ITimeSeries>();
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

                OutputObservation observation = new OutputObservation("Oxygen", depthRelatedTimeseries);
                observations.Add("Oxygen", observation);
            }

            return observations;
        }
    }
}
