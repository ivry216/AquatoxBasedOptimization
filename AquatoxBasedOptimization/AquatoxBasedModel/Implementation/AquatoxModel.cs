using AquatoxBasedOptimization.AquatoxFilesProcessing.Input;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Output;
using AquatoxBasedOptimization.ExternalProgramOperating;
using OptimizationProblems.Models;
using System.Collections.Generic;
using System.Linq;

namespace AquatoxBasedOptimization.AquatoxBasedModel.Implementation
{
    public class AquatoxModel : IModel<AquatoxModelInput, AquatoxModelParameters, AquatoxModelOutput>
    {
        #region Fields

        private IAquatoxOutputFileProcessor _outputFileProcessor;
        private IAquatoxInputFileProcessor _inputFileProcessor;

        #endregion Fields

        #region Properties

        public AquatoxModelParameters Parameters { get; private set; }

        #endregion Properties

        #region Constructor

        public AquatoxModel(IAquatoxOutputFileProcessor outputFileProcessor)
        {
            _outputFileProcessor = outputFileProcessor;
        }

        #endregion Constructor

        #region Main Methods

        public void SetInput(AquatoxModelInput modelInput, int id)
        {
            _inputFileProcessor.SetParametersBySubstitution(BuildInputFileName(id), modelInput.ModelVariables);
        }

        public void SetParameters(AquatoxModelParameters modelParameters)
        {
            Parameters = modelParameters;
            // TODO: Move initialization somewhere else?
            _inputFileProcessor = new AquatoxInputFileProcessor(Parameters.InputFilePath, Parameters.InputParameters.Values.ToList());
        }

        public AquatoxModelOutput Evaluate(int id)
        {
            // TODO: that was added to make the evaluation being correctly working in parallel
            SimpleSingleLauncher _simpleSingleLauncher = new SimpleSingleLauncher(Parameters.AquatoxExecutablePath);
            _simpleSingleLauncher.SetParameters(BuildAquatoxRunningCommand(id));
            _simpleSingleLauncher.Run();

            return new AquatoxModelOutput(_outputFileProcessor.ReadOutputs(BuildOutputFileName(id)));
        }

        #endregion Main Methods

        #region Converting To Input

        public Dictionary<string, string> ConvertValuesToInput(double[] values)
        {
            Dictionary<string, string> input = new Dictionary<string, string>();
            for (int i = 0; i < Parameters.InputsInnerNames.Length; i++)
            {
                input.Add(Parameters.InputsInnerNames[i], values[i].ToString(Parameters.NumericRepresentationFormat));
            }

            return input;
        }

        #endregion Converting To Input

        #region Supporting Methods

        private string BuildInputFileName(int id)
        {
            return Parameters.CurrentDirectory + "\\Input" + id + ".txt";
        }

        private string BuildOutputFileName(int id)
        {
            return "Output_" + id + ".txt";
        }

        private string BuildAquatoxRunningCommand(int id)
        {
            return "EPSAVE " + BuildInputFileName(id) + " \"" + BuildOutputFileName(id) + "\"";
        }

        #endregion Supporting Methods
    }
}
