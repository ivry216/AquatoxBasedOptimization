using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AquatoxBasedOptimization.AquatoxBasedProblem.Implementation;
using OfficeOpenXml;

namespace AquatoxBasedOptimization.Data.Variable
{
    public class AquatoxVariablesFileReader
    {
        public Dictionary<string, AquatoxParameterToTune> ReadParameters(string fileName)
        {
            var parameters = new Dictionary<string, AquatoxParameterToTune>();

            using (ExcelPackage package = new ExcelPackage(new FileInfo(fileName)))
            {

            }

            throw new NotImplementedException();
        }
    }
}
