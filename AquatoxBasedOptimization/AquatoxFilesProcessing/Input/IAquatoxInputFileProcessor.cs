using System.Collections.Generic;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Input
{
    public interface IAquatoxFileProcessor
    {
        void SetParametersBySubstitution(string pathToSave, Dictionary<string, string> parametersToSubstitute);
    }
}
