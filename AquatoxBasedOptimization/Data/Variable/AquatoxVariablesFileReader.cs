using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using AquatoxBasedOptimization.AquatoxBasedProblem.Implementation;
using OfficeOpenXml;

namespace AquatoxBasedOptimization.Data.Variable
{
    public class AquatoxVariablesFileReader
    {
        private static string _colName = "Name";
        private static string _colInnerName = "Inner Name";
        private static string _colInnerVarName = "Inner Variable Name";
        private static string _colInitialVal = "Initial Value";
        private static string _colHardMin = "Hard Min";
        private static string _colSoftMin = "Soft Min";
        private static string _colSoftMax = "Soft Max";
        private static string _colHardMax = "Hard Max";
        private static string _colIsToBeTuned = "Tune";

        private readonly List<string> _colnames = new List<string>() { _colName, _colInnerName, _colInnerVarName, _colInitialVal, _colHardMin, _colSoftMin, _colSoftMax, _colHardMax, _colIsToBeTuned };

        private Dictionary<string, int> GetColumnIndices(ExcelWorksheet worksheet, int startingRow, int startingCol, int nCols)
        {
            string trialString;

            var wordsIndices = _colnames.ToDictionary(name => name, name => (int?)null);
            List<string> wordsToFind = new List<string>(_colnames);

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

            return wordsIndices.ToDictionary(pair => pair.Key, pair => pair.Value.Value);
        }

        public Dictionary<string, AquatoxParameterToTune> ReadParameters(string fileName)
        {
            List<AquatoxParameterToTune> foundParameters = new List<AquatoxParameterToTune>();

            using (ExcelPackage package = new ExcelPackage(new FileInfo(fileName)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                int nRows = worksheet.Dimension.End.Row;
                int nCols = worksheet.Dimension.End.Column;

                var indices = GetColumnIndices(worksheet, startingRow: 1, startingCol: 1, nCols: nCols);
                
                for (int i = 2; i <= nRows; i++)
                {
                    try
                    {
                        if (worksheet.Cells[i, indices[_colIsToBeTuned]].Value.ToString() == "yes")
                        {
                            AquatoxParameterToTune currentParameter = new AquatoxParameterToTune(
                            name: worksheet.Cells[i, indices[_colName]].Value.ToString(),
                            aquatoxName: worksheet.Cells[i, indices[_colInnerName]].Value.ToString(),
                            aquatoxVarName: worksheet.Cells[i, indices[_colInnerVarName]].Value.ToString(),
                            initialValue: double.Parse(worksheet.Cells[i, indices[_colInitialVal]].Value.ToString()),
                            hardMax: GetNulalbleDouble(worksheet.Cells[i, indices[_colHardMax]].Value),
                            hardMin: GetNulalbleDouble(worksheet.Cells[i, indices[_colHardMin]].Value),
                            softMax: GetNulalbleDouble(worksheet.Cells[i, indices[_colSoftMax]].Value),
                            softMin: GetNulalbleDouble(worksheet.Cells[i, indices[_colSoftMin]].Value));
                            foundParameters.Add(currentParameter);
                        }
                    }
                    catch (Exception e)
                    {
                        break;
                    }
                }
            }

            return foundParameters.ToDictionary(item => item.AquatoxName + " " + item.AquatoxVariableName, item => item);
        }

        private double? GetNulalbleDouble(object cell)
        {
            return cell == null || cell.ToString() == "" ? (double?)null : double.Parse(cell.ToString());
        }
    }
}
