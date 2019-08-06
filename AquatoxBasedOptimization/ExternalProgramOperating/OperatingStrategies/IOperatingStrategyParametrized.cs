namespace AquatoxBasedOptimization.ExternalProgramOperating.OperatingStrategies
{
    public interface IOperatingStrategyParametrized : IOperatingStrategy
    {
        void SetExecutionParameters(string parameters);
    }
}
