using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.Data
{
    public interface IOutputObservation
    {
        string Name { get; }
        Dictionary<double, ITimeSeries> DepthRelatedObservations { get; }
    }
}
