using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.Data
{
    public class OutputObservation : IOutputObservation
    {
        public string Name { get; private set; }

        public Dictionary<double, ITimeSeries> DepthRelatedObservations { get; private set; }

        public OutputObservation(string name, Dictionary<double, ITimeSeries> depthRelatedObservations)
        {
            Name = name;
            DepthRelatedObservations = depthRelatedObservations;
        }
    }
}
