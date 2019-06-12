using AquatoxBasedOptimization.Data;
using OptimizationProblems.Models;
using System.Collections.Generic;

namespace AquatoxBasedOptimization.AquatoxBasedModel.Implementation
{
    public class AquatoxModelOutput : IModelOutput
    {
        public Dictionary<string, ITimeSeries> Outputs { get; }

        public AquatoxModelOutput(Dictionary<string, ITimeSeries> outputValues)
        {
            Outputs = new Dictionary<string, ITimeSeries>(outputValues);
        }
    }
}
