using Optimization.OptimizationProblems;
using System.Collections.Concurrent;

namespace AquatoxBasedOptimization.AquatoxBasedProblem.Implementation
{
    public class AquatoxTuningProblemValues : IParallelProblemValues
    {
        public double[] Values { get; }

        public AquatoxTuningProblemValues(ConcurrentBag<(int Index, double Value)> bag)
        {
            Values = new double[bag.Count];
            var array = bag.ToArray();
            for (int i = 0; i < bag.Count; i++)
            {
                Values[array[i].Index] = array[i].Value;
            }
        }
    }
}
