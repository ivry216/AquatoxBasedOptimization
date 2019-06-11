using Optimization.OptimizationAlgorithms.DifferentialEvolutionAlgorithm;

namespace Optimization.OptimizationAlgorithms.AlgorithmsGeneralParameters
{
    public interface IGenerationParameters
    {
        DeGeneratingType GeneratingType { get; set; }
        double[] GenerateFrom { get; set; }
        double[] GenerateTo { get; set; }
        double[] GenerateMean { get; set; }
        double[] GenerateSd { get; set; }
    }
}
