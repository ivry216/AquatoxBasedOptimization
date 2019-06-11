using AquatoxBasedOptimization.AquatoxBasedModel.Implementation;
using AquatoxBasedOptimization.Data;
using AquatoxBasedOptimization.Metrics.PredefinedComparing;
using Optimization.OptimizationProblems;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxBasedProblem.Implementation
{
    public class AquatoxParametersTuningProblem : IParallelProblem<AquatoxModel, AquatoxModelInput, AquatoxModelOutput, AquatoxTuningProblemValues, AquatoxParametersToTune>
    {
        private AquatoxModel _model;
        private PredefinedDistanceCalculator _distanceCalculator;
        private Dictionary<string, IOutputObservation> _observations;

        // 
        public AquatoxTuningProblemValues Evaluate(AquatoxParametersToTune alternatives)
        {
            ConcurrentBag<(int Index, double Value)> concurrentResults = new ConcurrentBag<(int Index, double Value)>();

            Parallel.For(0, alternatives.Parameters.Length, (i) =>
            {
                _model.SetInput(alternatives.Parameters[i], i);
                var output = _model.Evaluate(i);

                var dist = _distanceCalculator.CalculateDistance(output.Outputs["Oxygen"], _observations["Oxygen"].DepthRelatedObservations["1,0"]);

                concurrentResults.Add((i, dist));
            });

            return new AquatoxTuningProblemValues(concurrentResults);
        }

        // TODO: move to abstract class
        public void SetModel(AquatoxModel model)
        {
            _model = model;
        }

        public void SetDistanceCalculator(PredefinedDistanceCalculator distanceCalculator)
        {
            _distanceCalculator = distanceCalculator;
        }

        public void SetObservations(Dictionary<string, IOutputObservation> observations)
        {
            _observations = observations;
        }
    }
}
