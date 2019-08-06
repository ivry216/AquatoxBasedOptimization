using System.Diagnostics;
using System.IO;

namespace AquatoxBasedOptimization.ExternalProgramOperating.OperatingStrategies
{
    public class ExecutiveLauncher : IOperatingStrategyParametrized
    {
        #region Fields

        private string _executionParameters;
        private FileInfo _executionFile;
        private ProcessStartInfo _processInfo;

        #endregion Fields

        #region Constructor

        public ExecutiveLauncher()
        {}

        #endregion Constructor

        #region Main Methods

        public void Execute()
        {
            _processInfo = new ProcessStartInfo();
            _processInfo.FileName = _executionFile.FullName;
            _processInfo.Arguments = _executionParameters;

            using (Process process = Process.Start(_processInfo))
            {
                process.WaitForExit();
            }
        }

        public void SetExecutionParameters(string parameters)
        {
            _executionParameters = parameters;
        }

        public void SetExecutiveFile(FileInfo fileInfo)
        {
            _executionFile = fileInfo;
        }

        #endregion Main Methods
    }
}
