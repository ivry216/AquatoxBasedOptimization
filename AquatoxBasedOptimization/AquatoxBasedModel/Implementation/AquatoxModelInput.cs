using Optimization.OptimizationProblems.Models;
using System.Collections.Generic;

namespace AquatoxBasedOptimization.AquatoxBasedModel.Implementation
{
    public class AquatoxModelInput : IModelInput
    {
        public Dictionary<string, string> ModelVariables { get; }

        public AquatoxModelInput(Dictionary<string, string> variables)
        {
            ModelVariables = new Dictionary<string, string>(variables);
        }
    }
}
