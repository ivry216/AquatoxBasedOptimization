using AquatoxBasedOptimization.AquatoxBasedModel;

namespace AquatoxBasedOptimization.AquatoxBasedProblem
{
    public interface IParallelProblem<TModel, TModelInput, TModelOutput, TValues, TAlternatives>
        where TModel : IModel<TModelInput, TModelOutput>
        where TModelInput : IModelInput
        where TModelOutput : IModelOutput
        where TValues : IParallelProblemValues
        where TAlternatives : IParallelProblemAlternative
    {
        void SetModel(TModel model);
        TValues Evaluate(TAlternatives alternatives);
    }
}
