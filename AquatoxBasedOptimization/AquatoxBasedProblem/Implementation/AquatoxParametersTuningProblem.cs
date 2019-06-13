using AquatoxBasedOptimization.AquatoxBasedModel.Implementation;
using AquatoxBasedOptimization.Data;
using AquatoxBasedOptimization.Metrics.PredefinedComparing;
using Optimization.Problem.Parallel;
using Optimization.Problem.Parallel.Alternatives;
using Optimization.Problem.Parallel.Values;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxBasedProblem.Implementation
{
    public class AquatoxParametersTuningProblem : ParallelOptimizationProblem<RealObjectiveValues, RealVectorAlternatives>
    {
        private AquatoxModel _model;
        private PredefinedDistanceCalculator _distanceCalculator;
        private Dictionary<string, IOutputObservation> _observations;

        public AquatoxParametersTuningProblem(int dimension) : base(dimension)
        {

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

        public override RealObjectiveValues CalculateCriterion(RealVectorAlternatives alternatives)
        {
            ConcurrentBag<(int Index, double Value)> concurrentResults = new ConcurrentBag<(int Index, double Value)>();

            Parallel.For(0, alternatives.Alternatives.Length, (i) =>
            {
                var inputForModel = _model.Parameters.ConvertValuesToInput(alternatives.Alternatives[i]);
                _model.SetInput(new AquatoxModelInput(inputForModel), i);
                var output = _model.Evaluate(i);

                var dist = _distanceCalculator.CalculateDistance(output.Outputs["Oxygen"], _observations["Oxygen"].DepthRelatedObservations["1,0"]);

                concurrentResults.Add((i, dist));
            });

            return new RealObjectiveValues(concurrentResults);
        }
    }
}
