using AquatoxBasedOptimization.AquatoxBasedModel.Implementation;

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
