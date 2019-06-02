using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.Data.OutputVariables
{
    public interface IOutputVariablesReader
    {
        Dictionary<string, int> Read();
    }
}
