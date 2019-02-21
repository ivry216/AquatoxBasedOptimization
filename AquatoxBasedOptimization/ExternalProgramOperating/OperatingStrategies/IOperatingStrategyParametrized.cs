using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.ExternalProgramOperating.OperatingStrategies
{
    public interface IOperatingStrategyParametrized : IOperatingStrategyParametrized
    {
        void SetExecutionParameters(string parameters);
    }
}
