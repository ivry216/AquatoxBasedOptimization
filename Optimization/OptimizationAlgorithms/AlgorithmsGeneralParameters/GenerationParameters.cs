using Optimization.OptimizationAlgorithms.DifferentialEvolutionAlgorithm;

namespace Optimization.OptimizationAlgorithms.AlgorithmsGeneralParameters
{
    public class GenerationParameters : IGenerationParameters
    {
        private int _dimension;

        public DeGeneratingType GeneratingType { get; set; }
        public double[] GenerateFrom { get; set; }
        public double[] GenerateTo { get; set; }
        public double[] GenerateMean { get; set; }
        public double[] GenerateSd { get; set; }

        GenerationParameters(int dimension)
        {
            // Set dimension
            _dimension = dimension;
            // Initialize arrays
            GenerateFrom = new double[_dimension];
            GenerateTo = new double[_dimension];
            GenerateMean = new double[_dimension];
            GenerateSd = new double[_dimension];
        }

        GenerationParameters(int dimension, double[] generateFrom, double[] generateTo, double[] generateMean, double[] generateSd)
        {
            // Set dimension
            _dimension = dimension;
            // Initialize arrays
            GenerateFrom = generateFrom;
            GenerateTo = generateTo;
            GenerateMean = generateMean;
            GenerateSd = generateSd;
        }
    }
}
