using System.Collections.Generic;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Input
{
    public interface IAquatoxInputFileProcessor
    {
        void SetParametersBySubstitution(string pathToSave, Dictionary<string, string> parametersToSubstitute);
    }
}
