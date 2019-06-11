using AquatoxBasedOptimization.AquatoxBasedModel;
using AquatoxBasedOptimization.AquatoxBasedProblem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.OptimizationAlgorithms.DifferentialEvolutionAlgorithm
{
    public class DifferentialEvolution<TProblem, TValues, TAlternatives> : IOptimizationAlgorithm<DifferentialEvolutionParameters, TProblem, TValues, TAlternatives>
        where TAlternatives : IParallelProblemAlternative
        where TValues : IParallelProblemValues
        where TProblem : IParallelProblem<TValues, TAlternatives>
    {
        public void Evaluate()
        {
            throw new NotImplementedException();
        }

        public void SetParameters(DifferentialEvolutionParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void SetProblem(TProblem problem)
        {
            throw new NotImplementedException();
        }
    }
}
