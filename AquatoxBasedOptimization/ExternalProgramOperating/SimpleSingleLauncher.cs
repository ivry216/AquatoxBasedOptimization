using AquatoxBasedOptimization.ExternalProgramOperating.OperatingStrategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.ExternalProgramOperating
{
    public class SimpleSingleLauncher : ExternalProgramLauncher<ExecutiveLauncher>
    {
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

        public override void Run()
        {
            operatingStrategy.SetExecutiveFile(fileInfo);
            operatingStrategy.SetExecutionParameters(parameters);
            operatingStrategy.Execute();
        }
    }
}
