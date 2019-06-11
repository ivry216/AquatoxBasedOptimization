using AquatoxBasedOptimization.AquatoxBasedProblem;

namespace AquatoxBasedOptimization.OptimizationAlgorithms
{
    public interface IOptimizationAlgorithm<TParameters, TProblem, TValues, TAlternatives>
        where TParameters : IOptimizationAlgorithmParameters
        where TValues : IParallelProblemValues
        where TAlternatives : IParallelProblemAlternative
        where TProblem : IParallelProblem<TValues, TAlternatives>
    {
        void SetParameters(TParameters parameters);
        void SetProblem(TProblem problem);
        void Evaluate();
    }
}
