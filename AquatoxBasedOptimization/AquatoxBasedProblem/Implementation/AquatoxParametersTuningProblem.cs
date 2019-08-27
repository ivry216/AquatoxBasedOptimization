using AquatoxBasedOptimization.AquatoxBasedModel.Implementation;
using AquatoxBasedOptimization.Data;
using AquatoxBasedOptimization.Metrics.PredefinedComparing;
using Optimization.Problem.Constrains;
using Optimization.Problem.Constrains.Parallel;
using Optimization.Problem.Parallel.Alternatives;
using Optimization.Problem.Parallel.Constrained;
using Optimization.Problem.Parallel.Values;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxBasedProblem.Implementation
{
    public class AquatoxParametersTuningProblem : ParallelOptimizationConstrainedProblem<RealObjectiveValues, RealVectorAlternatives>
    {
        private static int iteration = 1;

        private AquatoxModel _model;
        private PredefinedDistanceCalculator _distanceCalculator;
        private Dictionary<string, IOutputObservation> _observations;
        private Dictionary<string, double> _maxObservationValues;
        private Dictionary<string, string> _observationsDepth;

        public AquatoxParametersTuningProblem(int dimension, HardAndSoftConstrainer constrainer) : base(dimension, constrainer)
        {
            _observationsDepth = new Dictionary<string, string>
            {
                { "Oxygen", "1,0" },
                { "Chlorophyll", "0,0-5,0" },
                { "Nitrogene", "1,0" },
                { "Phosphorus", "1,0" }
            };
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
            _maxObservationValues = observations
                .ToDictionary(
                pair => pair.Key, 
                pair => pair.Value.DepthRelatedObservations[_observationsDepth[pair.Key]].Values.Max()
                );
        }

        public override RealObjectiveValues CalculateCriterion(RealVectorAlternatives alternatives)
        {
            ConcurrentBag<(int Index, double Value)> concurrentResults = new ConcurrentBag<(int Index, double Value)>();

            Console.Write($"Another iteration! {iteration}");
            iteration++;

            Parallel.For(0, alternatives.Alternatives.Length, (i) =>
            {
                if (_constrainer.IsFeasibleForHard(alternatives.Alternatives[i]))
                {
                    var inputForModel = _model.ConvertValuesToInput(alternatives.Alternatives[i]);
                    _model.SetInput(new AquatoxModelInput(inputForModel), i);
                    AquatoxModelOutput output = _model.Evaluate(i);
                    var distOxygen = _distanceCalculator.CalculateDistance(output.Outputs["Oxygen"], _observations["Oxygen"].DepthRelatedObservations["1,0"]) / _maxObservationValues["Oxygen"];
                    var distChlorophyll = _distanceCalculator.CalculateDistance(output.Outputs["Phyto. Chlorophyll"], _observations["Chlorophyll"].DepthRelatedObservations["0,0-5,0"]) / _maxObservationValues["Chlorophyll"];
                    var distNitrogene = _distanceCalculator.CalculateDistance(output.Outputs["TN"], _observations["Nitrogene"].DepthRelatedObservations["1,0"]) / _maxObservationValues["Nitrogene"];
                    var distPhosphorus = _distanceCalculator.CalculateDistance(output.Outputs["TP"], _observations["Phosphorus"].DepthRelatedObservations["1,0"]) / _maxObservationValues["Phosphorus"];
                    var fitness = 1 / (1 + distOxygen + distChlorophyll + distNitrogene + distPhosphorus  + _constrainer.CalculatePenaltyForSoft(alternatives.Alternatives[i]));
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
