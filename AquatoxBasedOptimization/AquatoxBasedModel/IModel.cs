namespace AquatoxBasedOptimization.AquatoxBasedModel
{
    public interface IModel<TInput, TParameters, TOutput>
        where TInput : IModelInput
        where TParameters : IModelParameters
        where TOutput : IModelOutput
    {
        void SetParameters(TParameters modelParameters);
        void SetInput(TInput modelInput);
        TOutput Evaluate();
    }
}
