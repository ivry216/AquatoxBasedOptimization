using AquatoxBasedOptimization.AquatoxFilesProcessing.Input;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Output;
using AquatoxBasedOptimization.ExternalProgramOperating;
using OptimizationProblems.Models;
using System.Linq;

namespace AquatoxBasedOptimization.AquatoxBasedModel.Implementation
{
    public class AquatoxModel : IModel<AquatoxModelInput, AquatoxModelParameters, AquatoxModelOutput>
    {
        private AquatoxOutputFileProcessor _outputFileProcessor;
        private AquatoxInputFileProcessor _inputFileProcessor;

        public AquatoxModelParameters Parameters { get; private set; }

        public AquatoxModel(AquatoxOutputFileProcessor outputFileProcessor)
        {
            _outputFileProcessor = outputFileProcessor;
        }

        public void SetInput(AquatoxModelInput modelInput, int id)
        {
            _inputFileProcessor.SetParametersBySubstitution(Parameters.BuildInputFileName(id), modelInput.ModelVariables);
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
            _simpleSingleLauncher.SetParameters(Parameters.BuildAquatoxRunningCommand(id));
            _simpleSingleLauncher.Run();

            return new AquatoxModelOutput(_outputFileProcessor.ReadOutputs(Parameters.BuildOutputFileName(id)));
        }
    }
}
