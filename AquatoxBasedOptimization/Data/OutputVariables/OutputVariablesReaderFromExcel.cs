using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.Data.OutputVariables
{
    public class OutputVariablesReaderFromExcel : IOutputVariablesReader
    {
        private const string _fileName = "OutputVariables.xlsx";

        public Dictionary<string, int> Read()
        {
            // Initialize variables-indices pairs
            Dictionary<string, int> variablesIndices = new Dictionary<string, int>();

            FileInfo file = new FileInfo(_fileName);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                //get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                //
                int nRows = worksheet.Dimension.End.Row;
                //
                for (int row = 2; row <= nRows; row++)
                {
                    variablesIndices.Add(worksheet.Cells[row, 1].Value.ToString(), int.Parse(worksheet.Cells[row, 2].Value.ToString()));
                }
            }

            return variablesIndices;
        }
    }
}
