using AquatoxBasedOptimization.AquatoxBasedModel.Implementation;
using AquatoxBasedOptimization.Data;
using AquatoxBasedOptimization.Metrics.PredefinedComparing;
using Optimization.Problem.Constrains;
using Optimization.Problem.Constrains.Parallel;
using Optimization.Problem.Parallel.Alternatives;
using Optimization.Problem.Parallel.Constrained;
using Optimization.Problem.Parallel.Values;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxBasedProblem.Implementation
{
    public class AquatoxParametersTuningProblem : ParallelOptimizationConstrainedProblem<RealObjectiveValues, RealVectorAlternatives>
    {
        private AquatoxModel _model;
        private PredefinedDistanceCalculator _distanceCalculator;
        private Dictionary<string, IOutputObservation> _observations;

        public AquatoxParametersTuningProblem(int dimension, HardAndSoftConstrainer constrainer) : base(dimension, constrainer)
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
                if (_constrainer.IsFeasibleForHard(alternatives.Alternatives[i]))
                {
                    var inputForModel = _model.ConvertValuesToInput(alternatives.Alternatives[i]);
                    _model.SetInput(new AquatoxModelInput(inputForModel), i);
                    AquatoxModelOutput output = _model.Evaluate(i);
                    var distOxygen = _distanceCalculator.CalculateDistance(output.Outputs["Oxygen"], _observations["Oxygen"].DepthRelatedObservations["1,0"]);
                    var distChlorophyll = _distanceCalculator.CalculateDistance(output.Outputs["Phyto. Chlorophyll"], _observations["Chlorophyll"].DepthRelatedObservations["1,0"]);
                    var distNitrogene = _distanceCalculator.CalculateDistance(output.Outputs["TN"], _observations["Nitrogene"].DepthRelatedObservations["1,0"]);
                    var distPhosphorus = _distanceCalculator.CalculateDistance(output.Outputs["TP"], _observations["Phosphorus"].DepthRelatedObservations["1,0"]);
                    var fitness = 1 / (1 + distOxygen + _constrainer.CalculatePenaltyForSoft(alternatives.Alternatives[i]));
                    concurrentResults.Add((i, fitness));
                }
                else
                {
                    concurrentResults.Add((i, 0));
                }
            });

            return new RealObjectiveValues(concurrentResults);
        }
    }
}
