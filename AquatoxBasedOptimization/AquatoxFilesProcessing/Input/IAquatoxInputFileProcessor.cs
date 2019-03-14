using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Input
{
    public interface IAquatoxFileProcessor
    {
        void SetParametersBySubstitution(string pathToSave, Dictionary<string, string> parametersToSubstitute);
    }
}
