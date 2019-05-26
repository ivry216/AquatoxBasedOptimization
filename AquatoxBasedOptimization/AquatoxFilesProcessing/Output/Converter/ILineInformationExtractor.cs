using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Output.Converter
{
    public interface ILineInformationExtractor
    {
        Dictionary<string, int> VariablesAndIndices { get; set; }
        (DateTime Date, Dictionary<string, double>) ExtractData(string line);
    }
}
