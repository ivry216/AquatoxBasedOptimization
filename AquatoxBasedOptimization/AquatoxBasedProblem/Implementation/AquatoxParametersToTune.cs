using AquatoxBasedOptimization.AquatoxBasedModel.Implementation;
using Optimization.OptimizationProblems;

namespace AquatoxBasedOptimization.AquatoxBasedProblem.Implementation
{
    public class AquatoxParametersToTune : IParallelProblemAlternative
    {
        public AquatoxModelInput[] Parameters { get; }

        public AquatoxParametersToTune(AquatoxModelInput[] inputs)
        {
            Parameters = inputs;
        }
    }
}