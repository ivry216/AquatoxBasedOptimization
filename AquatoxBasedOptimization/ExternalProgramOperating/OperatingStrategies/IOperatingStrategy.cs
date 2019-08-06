using System.IO;

namespace AquatoxBasedOptimization.ExternalProgramOperating.OperatingStrategies
{
    public interface IOperatingStrategy
    {
        void SetExecutiveFile(FileInfo fileInfo);
        void Execute();
    }
}
