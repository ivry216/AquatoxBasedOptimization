using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.ExternalProgramOperating.OperatingStrategies
{
    public interface IOperatingStrategy
    {
        void Execute(FileInfo fileInfo);
    }
}
