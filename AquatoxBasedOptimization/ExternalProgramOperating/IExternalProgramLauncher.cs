using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.ExternalProgramOperating
{
    public interface IExternalProgramLauncher
    {
        void Run();
        void Run(string parameters);
    }
}
