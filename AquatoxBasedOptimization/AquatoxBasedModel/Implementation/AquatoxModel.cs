using AquatoxBasedOptimization.AquatoxFilesProcessing.Input;
using AquatoxBasedOptimization.AquatoxFilesProcessing.Output;
using AquatoxBasedOptimization.ExternalProgramOperating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxBasedModel.Implementation
{
    public class AquatoxModel : IModel<AquatoxModelInput, AquatoxModelParameters, AquatoxModelOutput>
    {
        private AquatoxOutputFileProcessor _outputFileProcessor;
        private AquatoxInputFileProcessor _inputFileProcessor;
        private SimpleSingleLauncher _simpleSingleLauncher;

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
            // TODO: same?
            _simpleSingleLauncher = new SimpleSingleLauncher();
            _simpleSingleLauncher.File = new FileInfo(Parameters.AquatoxExecutablePath);
        }

        public AquatoxModelOutput Evaluate(int id)
        {
            _simpleSingleLauncher.SetParameters(Parameters.BuildAquatoxRunningCommand(id));
            _simpleSingleLauncher.Run();

            return new AquatoxModelOutput(_outputFileProcessor.ReadOutputs(Parameters.PerformOutputFileName(id)));
        }
    }
}
