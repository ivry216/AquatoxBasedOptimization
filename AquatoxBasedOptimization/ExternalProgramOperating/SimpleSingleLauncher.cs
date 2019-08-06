using AquatoxBasedOptimization.ExternalProgramOperating.OperatingStrategies;
using System.IO;

namespace AquatoxBasedOptimization.ExternalProgramOperating
{
    public class SimpleSingleLauncher : ExternalProgramLauncher<ExecutiveLauncher>
    {
        #region Constructor

        public SimpleSingleLauncher() : base()
        {
        }

        public SimpleSingleLauncher(string executiveFilePath) : base(executiveFilePath)
        {
            fileInfo = new FileInfo(executiveFilePath);
        }

        public SimpleSingleLauncher(FileInfo fileInfo) : base(fileInfo)
        {
            this.fileInfo = new FileInfo(fileInfo.FullName);
        }

        #endregion Constructor

        #region Main Methods

        public override void Run()
        {
            operatingStrategy.SetExecutiveFile(fileInfo);
            operatingStrategy.SetExecutionParameters(parameters);
            operatingStrategy.Execute();
        }

        #endregion Main Methods
    }
}
