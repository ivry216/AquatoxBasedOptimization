using AquatoxBasedOptimization.AquatoxBasedModel.Implementation;
using AquatoxBasedOptimization.AquatoxBasedProblem.Implementation;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Output;
using AquatoxBasedOptimization.Data.OutputObservations;
using AquatoxBasedOptimization.Data.OutputVariables;
using AquatoxBasedOptimization.Data.Variable;
using AquatoxBasedOptimization.Metrics.PredefinedComparing;
using Optimization.AlgorithmsControl.AlgorithmRunStatisticsInfrastructure.IterationStatistics;
using Optimization.EvolutionaryAlgorithms.DifferentialEvolutionAlgorithm;
using Optimization.EvolutionaryAlgorithms.DifferentialEvolutionAlgorithm.Parallel;
using Optimization.Problem.Constrains;
using Optimization.Problem.Parallel.Alternatives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace AquatoxBasedOptimization
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Variables

            string variablesFileName = @"C:/Users/Ivan/Repositiries/AquatoxBasedOptimization/JupyterNotebooks/variables.xlsx"; ;
            AquatoxVariablesFileReader variablesReader = new AquatoxVariablesFileReader();
            Dictionary<string, AquatoxParameterToTune> modelVariables = variablesReader.ReadParameters(variablesFileName);

            #endregion Variables

            #region Generating

            double defaultMin = -10;
            double defaultMax = 10;

            List<(double From, double To)> generatingBoundaries = modelVariables.Select(pair => pair.Value.MakeGenerationBoundaries(defaultMin, defaultMax)).ToList();

            #endregion Generating

            #region Constrains

            double softConstrainWight = 1000;

            HardAndSoftConstrain[] constrains = modelVariables.Select(pair => pair.Value.MakeConstrain(softConstrainWight)).ToArray();

            HardAndSoftConstrainerParameters hardAndSoftConstrainerParameters = new HardAndSoftConstrainerParameters(constrains);
            HardAndSoftConstrainer constrainer = new HardAndSoftConstrainer(hardAndSoftConstrainerParameters);

            #endregion Constrains

            #region Model

            IOutputVariablesReader outputVariablesReader = new OutputVariablesReaderFromExcel();
            Dictionary<string, int> variablesAndIndices = outputVariablesReader.Read();
            IAquatoxOutputFileProcessor outputFileProcessor = new AquatoxOutputFileProcessor(variablesAndIndices); 
            AquatoxModelParameters modelParameters = new AquatoxModelParameters();
            modelParameters.InputParameters = new Dictionary<string, string>() { { "par1", "_param1_" }, { "par2", "_param2_" }, { "par3", "_param3_" }, { "par4", "_param4_" }, { "par5", "_param5_" } };
            AquatoxModel model = new AquatoxModel(outputFileProcessor);
            model.SetParameters(modelParameters);

            #endregion Model

            int dimension = modelParameters.InputParameters.Count;

            #region Problem

            PredefinedDistanceCalculator distanceCalculator = new PredefinedDistanceCalculator();

            OutputObservationsReaderFromExcel outputObservationsReader = new OutputObservationsReaderFromExcel();
            var observations = outputObservationsReader.ReadOutputVariableObservations();
            
            AquatoxParametersTuningProblem tuningProblem = new AquatoxParametersTuningProblem(dimension, constrainer);
            tuningProblem.SetDistanceCalculator(distanceCalculator);
            tuningProblem.SetObservations(observations);
            tuningProblem.SetModel(model);

            #endregion Problem

            #region Algorithm

            BestAlternativeHistoryMaker bestAltHistMaker = new BestAlternativeHistoryMaker();
            IterationValuesHistoryMaker iterationHistoryMaker = new IterationValuesHistoryMaker();

            DifferentialEvolutionParameters differentialEvolutionParameters = new DifferentialEvolutionParameters();
            differentialEvolutionParameters.CrossoverProbability = 0.5;
            differentialEvolutionParameters.DifferentialWeight = 1;
            //differentialEvolutionParameters.GenerationParameters.GenerationFrom = Enumerable.Repeat(0.0, dimension).ToArray();
            //differentialEvolutionParameters.GenerationParameters.GenerationTo = Enumerable.Repeat(10.0, dimension).ToArray();
            differentialEvolutionParameters.GenerationParameters.GenerationFrom = generatingBoundaries.Select(pair => pair.From).ToArray();
            differentialEvolutionParameters.GenerationParameters.GenerationTo = generatingBoundaries.Select(pair => pair.To).ToArray();

            differentialEvolutionParameters.GenerationParameters.GenerationType = Optimization.EvolutionaryAlgorithms.PopulationGenerationType.Uniform;
            differentialEvolutionParameters.Iterations = 20;
            differentialEvolutionParameters.Size = 20;

            ParallelDifferentialEvolution differentialEvolutionParallel = new ParallelDifferentialEvolution();
            differentialEvolutionParallel.SetParameters(differentialEvolutionParameters);
            differentialEvolutionParallel.SetProblem(tuningProblem);
            differentialEvolutionParallel.AddStatsFollowers(new List<IAlgorithmIterationFollower> { bestAltHistMaker, iterationHistoryMaker });
            differentialEvolutionParallel.Evaluate();

            bestAltHistMaker.SaveToFile();
            iterationHistoryMaker.SaveToFile();

            #endregion Algorithm

            var initialValueBag = tuningProblem.CalculateCriterion(new RealVectorAlternatives(new double[][] { modelVariables.Select(pair => pair.Value.InitialValue).ToArray() }));
            var initialValueFitness = initialValueBag.Values.ToArray()[0].Value;
            File.WriteAllText("initial.txt", initialValueFitness.ToString());

            Console.WriteLine("End!");
            Console.Read();
        }
    }
}