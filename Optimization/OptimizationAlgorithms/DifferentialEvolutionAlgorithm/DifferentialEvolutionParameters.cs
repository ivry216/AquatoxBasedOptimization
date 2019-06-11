using Optimization.OptimizationAlgorithms.AlgorithmsGeneralParameters;

namespace Optimization.OptimizationAlgorithms.DifferentialEvolutionAlgorithm
{
    public class DifferentialEvolutionParameters : IOptimizationAlgorithmParameters
    {
        public int Size { get; set; }
        public int Generations { get; set; }
        public double CrossoverRate { get; set; }
        public double DifferentialWeight { get; set; }

        public IGenerationParameters GenerationParameters { get; set; }
    }
}