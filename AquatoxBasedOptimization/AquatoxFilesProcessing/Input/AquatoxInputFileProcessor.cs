using AquatoxBasedOptimization.AquatoxFilesProcessing.Input.ParametersWriters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Input
{
    class AquatoxInputFileProcessor : IAquatoxFileProcessor
    {
        private FileInfo _inputFileTemplatePath;
        private string[] _inputFileLines;

        private Dictionary<string, IInputParameterWriter> _parameterWriterPairs;
        private Dictionary<int, string> _parameterIndexPair;

        public AquatoxInputFileProcessor(string inputFileTemplatePath, List<string> parameters)
        {
            // Initialize template input path
            _inputFileTemplatePath = new FileInfo(inputFileTemplatePath);
            // Read all its lines
            _inputFileLines = File.ReadAllLines(_inputFileTemplatePath.FullName);

            // 
            InitializeDictionaries(parameters);
        }

        public void SetParametersBySubstitution(string pathToSave, Dictionary<string, string> parametersToSubstitute)
        {
            var fileStream = File.Create(pathToSave);
            TextWriter textWriter = new StreamWriter(fileStream);

            var indices = _parameterIndexPair.Keys.ToList();

            for (int i = 0; i < _inputFileLines.Length; i++)
            {
                // If that line is parameter
                if (indices.Contains(i))
                {
                    var parameter = _parameterIndexPair[i];
                    textWriter.WriteLine(_parameterWriterPairs[parameter].Write(parametersToSubstitute[parameter]));
                }
                else
                {
                    textWriter.WriteLine(_inputFileLines[i]);
                }
            }
        }

        private void InitializeDictionaries(List<string> parameters)
        {
            // Initialize the wirters
            _parameterWriterPairs = new Dictionary<string, IInputParameterWriter>();
            // Initialize indices
            _parameterIndexPair = new Dictionary<int, string>();

            // Make a copy of parameters list
            List<string> trialParameters = new List<string>(parameters);

            // Trial variables
            string parameterFound = null;
            string lineContainingParameter = null;
            bool isParameterFound;

            for (int i = 0; i < _inputFileLines.Length; i++)
            {
                isParameterFound = false;

                foreach (string parameter in trialParameters)
                {
                    if (_inputFileLines[i].Contains(parameter))
                    {
                        isParameterFound = true;
                        parameterFound = parameter;
                        lineContainingParameter = _inputFileLines[i];
                        break;
                    }
                }

                if (isParameterFound)
                {
                    // Remove the parameter that was found
                    trialParameters.Remove(parameterFound);
                    // Add parameter and its properties to the result
                    _parameterWriterPairs.Add(parameterFound, new InputParameterWriter(lineContainingParameter, parameterFound));
                    _parameterIndexPair.Add(i, parameterFound);
                }
            }
        }
    }
}
