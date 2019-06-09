using AquatoxBasedOptimization.AquatoxBasedModel;

namespace AquatoxBasedOptimization.AquatoxBasedProblem
{
    public interface IParallelProblem<TValues, TAlternatives>
        where TValues : IParallelProblemValues
        where TAlternatives : IParallelProblemAlternative
    {
        TValues Evaluate(TAlternatives alternatives);
    }

    public interface IParallelProblem<TModel, TModelInput, TModelOutput, TValues, TAlternatives> : IParallelProblem<TValues, TAlternatives>
        where TModel : IModel<TModelInput, TModelOutput>
        where TModelInput : IModelInput
        where TModelOutput : IModelOutput
        where TValues : IParallelProblemValues
        where TAlternatives : IParallelProblemAlternative
    {
        void SetModel(TModel model);
    }
}
