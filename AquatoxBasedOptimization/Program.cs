using AquatoxBasedOptimization.AquatoxBasedModel.Implementation;
using AquatoxBasedOptimization.AquatoxBasedProblem.Implementation;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Output;
using AquatoxBasedOptimization.Data.OutputObservations;
using AquatoxBasedOptimization.Data.OutputVariables;
using AquatoxBasedOptimization.Metrics.PredefinedComparing;
using Optimization.AlgorithmsControl.AlgorithmRunStatisticsInfrastructure.IterationStatistics;
using Optimization.EvolutionaryAlgorithms.DifferentialEvolutionAlgorithm;
using Optimization.EvolutionaryAlgorithms.DifferentialEvolutionAlgorithm.Parallel;
using System;
using System.Collections.Generic;
using System.Linq;


namespace AquatoxBasedOptimization
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Model

            IOutputVariablesReader outputVariablesReader = new OutputVariablesReaderFromExcel();
            Dictionary<string, int> variablesAndIndices = outputVariablesReader.Read();
            IAquatoxOutputFileProcessor outputFileProcessor = new AquatoxOutputFileProcessor(variablesAndIndices); 
            AquatoxModelParameters modelParameters = new AquatoxModelParameters();
            modelParameters.InputParameters = new Dictionary<string, string>() { { "par1", "_param1_" }, { "par2", "_param2_" }, { "par3", "_param3_" }, { "par4", "_param4_" } };
            AquatoxModel model = new AquatoxModel(outputFileProcessor);
            model.SetParameters(modelParameters);

            #endregion Model

            int dimension = modelParameters.InputParameters.Count;

            #region Problem

            PredefinedDistanceCalculator distanceCalculator = new PredefinedDistanceCalculator();

            OutputObservationsReaderFromExcel outputObservationsReader = new OutputObservationsReaderFromExcel();
            var observations = outputObservationsReader.ReadOutputVariableObservations();
            
            AquatoxParametersTuningProblem tuningProblem = new AquatoxParametersTuningProblem(dimension);
            tuningProblem.SetDistanceCalculator(distanceCalculator);
            tuningProblem.SetObservations(observations);
            tuningProblem.SetModel(model);

            #endregion Problem

            BestAlternativeHistoryMaker bestAltHistMaker = new BestAlternativeHistoryMaker();
            IterationValuesHistoryMaker iterationHistoryMaker = new IterationValuesHistoryMaker();

            DifferentialEvolutionParameters differentialEvolutionParameters = new DifferentialEvolutionParameters();
            differentialEvolutionParameters.CrossoverProbability = 0.5;
            differentialEvolutionParameters.DifferentialWeight = 1;
            differentialEvolutionParameters.GenerationParameters.GenerationFrom = Enumerable.Repeat(0.0, dimension).ToArray();
            differentialEvolutionParameters.GenerationParameters.GenerationTo = Enumerable.Repeat(10.0, dimension).ToArray();
            differentialEvolutionParameters.GenerationParameters.GenerationType = Optimization.EvolutionaryAlgorithms.PopulationGenerationType.Uniform;
            differentialEvolutionParameters.Iterations = 10;
            differentialEvolutionParameters.Size = 20;

            ParallelDifferentialEvolution differentialEvolutionParallel = new ParallelDifferentialEvolution();
            differentialEvolutionParallel.SetParameters(differentialEvolutionParameters);
            differentialEvolutionParallel.SetProblem(tuningProblem);
            differentialEvolutionParallel.AddStatsFollowers(new List<IAlgorithmIterationFollower> { bestAltHistMaker, iterationHistoryMaker });
            differentialEvolutionParallel.Evaluate();

            bestAltHistMaker.SaveToFile();
            iterationHistoryMaker.SaveToFile();

            Console.WriteLine("End!");
            Console.Read();
        }
    }
}