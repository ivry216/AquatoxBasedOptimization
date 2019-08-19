namespace AquatoxBasedOptimization.AquatoxBasedProblem.Implementation
{
    public class AquatoxParameterToTune
    {
        public string Name { get; }
        public double InitialValue { get; }
        public double? SoftMaxConstrain { get; }
        public double? SoftMinConstrain { get; }
        public double? HardMaxConstrain { get; }
        public double? HardMinConstrain { get; }

        public AquatoxParameterToTune(string name, double initialValue, double? hardMax = null, double? hardMin = null, double? softMax = null, double? softMin = null)
        {
            Name = name;
            InitialValue = initialValue;
            HardMaxConstrain = hardMax;
            HardMinConstrain = hardMin;
            SoftMinConstrain = softMax;
            SoftMaxConstrain = softMax;
        }
    }
}