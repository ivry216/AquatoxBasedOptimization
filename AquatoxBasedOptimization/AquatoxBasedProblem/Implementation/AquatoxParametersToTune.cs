using AquatoxBasedOptimization.AquatoxBasedModel.Implementation;
using Optimization.Problem.Parallel;

namespace AquatoxBasedOptimization.AquatoxBasedProblem.Implementation
{
    public class AquatoxParametersToTune
    {
        public AquatoxModelInput[] Parameters { get; }

        public AquatoxParametersToTune(AquatoxModelInput[] inputs)
        {
            Parameters = inputs;
        }
    }
}