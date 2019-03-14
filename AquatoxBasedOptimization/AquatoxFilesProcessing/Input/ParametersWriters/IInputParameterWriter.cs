using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Input.ParametersWriters
{
    interface IInputParameterWriter
    {
        string ParameterName { get; }
        ParameterLocationType ParameterLocationType { get; }
        string Write(string parameterValue);
    }
}
