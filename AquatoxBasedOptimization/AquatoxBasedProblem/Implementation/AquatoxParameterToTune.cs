using Optimization.Problem.Constrains;

namespace AquatoxBasedOptimization.AquatoxBasedProblem.Implementation
{
    public class AquatoxParameterToTune
    {
        public string Name { get; }
        public string AquatoxName { get; }
        public string AquatoxVariableName { get; }
        public double InitialValue { get; }
        public double? SoftMaxConstrain { get; }
        public double? SoftMinConstrain { get; }
        public double? HardMaxConstrain { get; }
        public double? HardMinConstrain { get; }

        public AquatoxParameterToTune(string name, string aquatoxName, string aquatoxVarName, double initialValue, double? hardMax = null, double? hardMin = null, double? softMax = null, double? softMin = null)
        {
            Name = name;
            AquatoxName = aquatoxName;
            AquatoxVariableName = aquatoxVarName;
            InitialValue = initialValue;
            HardMaxConstrain = hardMax;
            HardMinConstrain = hardMin;
            SoftMinConstrain = softMin;
            SoftMaxConstrain = softMax;
        }

        public HardAndSoftConstrain MakeConstrain(double softConstrainWeight = 0)
        {
            var constrain = new HardAndSoftConstrain(HardMinConstrain, HardMaxConstrain, SoftMinConstrain, SoftMaxConstrain, softConstrainWeight);

            return constrain;
        }

        public (double From, double To) MakeGenerationBoundaries(double defaultMinDelta, double defaultMaxDelta)
        {
            double min, max;

            if (SoftMaxConstrain.HasValue)
                max = SoftMaxConstrain.Value;
            else if (HardMaxConstrain.HasValue)
                max = HardMaxConstrain.Value;
            else
                max = InitialValue + defaultMaxDelta;

            if (SoftMinConstrain.HasValue)
                min = SoftMinConstrain.Value;
            else if (HardMinConstrain.HasValue)
                min = HardMinConstrain.Value;
            else
                min = InitialValue - defaultMinDelta;

            return (min, max);
        }
    }
}