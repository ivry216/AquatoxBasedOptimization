using AquatoxBasedOptimization.AquatoxBasedProblem;

namespace AquatoxBasedOptimization.OptimizationAlgorithms
{
    public interface IOptimizationAlgorithm<TParameters, TValues, TAlternatives>
        where TParameters : IOptimizationAlgorithmParameters
        where TValues : IParallelProblemValues
        where TAlternatives : IParallelProblemAlternative
    {
        void SetParameters(TParameters parameters);
        void SetProblem(IParallelProblem<TValues, TAlternatives> problem);
    }
}
