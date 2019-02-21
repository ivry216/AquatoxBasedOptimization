using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.ExternalProgramOperating.OperatingStrategies
{
    public class ExecutiveLauncher : IOperatingStrategyParametrized
    {
        private string _executionParameters;
        private FileInfo _executionFile;
        private ProcessStartInfo _processInfo;

        public ExecutiveLauncher()
        {
            _processInfo = new ProcessStartInfo();
        }

        public void Execute()
        {
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
    }
}
