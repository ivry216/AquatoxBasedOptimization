namespace AquatoxBasedOptimization.AquatoxBasedModel
{
    public interface IModel<TInput, TOutput>
        where TInput : IModelInput
        where TOutput : IModelOutput
    {
        void SetInput(TInput modelInput, int id);
        TOutput Evaluate(int id);
    }

    public interface IModel<TInput, TParameters, TOutput> : IModel<TInput, TOutput>
        where TInput : IModelInput
        where TParameters : IModelParameters
        where TOutput : IModelOutput
    {
        void SetParameters(TParameters modelParameters);
    }
}
