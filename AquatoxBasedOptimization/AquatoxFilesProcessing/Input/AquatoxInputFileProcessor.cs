﻿using AquatoxBasedOptimization.AquatoxFilesProcessing.Input.ParametersWriters;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Input
{
    class AquatoxInputFileProcessor : IAquatoxInputFileProcessor
    {
        private FileInfo _inputFileTemplatePath;
        private string[] _inputFileLines;

        private Dictionary<string, IInputParameterWriter> _parameterWriterPairs;
        private Dictionary<int, string> _parameterIndexPair;

        public AquatoxInputFileProcessor(string inputFileTemplatePath, List<string> parameters)
        {
            _inputFileTemplatePath = new FileInfo(inputFileTemplatePath);
            _inputFileLines = File.ReadAllLines(_inputFileTemplatePath.FullName);

            InitializeDictionaries(parameters);
        }

        public void SetParametersBySubstitution(string pathToSave, Dictionary<string, string> parametersToSubstitute)
        {
            var fileStream = File.Create(pathToSave);
            TextWriter textWriter = new StreamWriter(fileStream);

            var indices = _parameterIndexPair.Keys.ToList();

            for (int i = 0; i < _inputFileLines.Length; i++)
            {
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

            textWriter.Flush();
            textWriter.Dispose();
            fileStream.Dispose();
        }

        private void InitializeDictionaries(List<string> parameters)
        {
            _parameterWriterPairs = new Dictionary<string, IInputParameterWriter>();
            _parameterIndexPair = new Dictionary<int, string>();

            var trialParameters = new List<string>(parameters);

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
                    trialParameters.Remove(parameterFound);

                    _parameterWriterPairs.Add(parameterFound, new InputParameterWriter(lineContainingParameter, parameterFound));
                    _parameterIndexPair.Add(i, parameterFound);
                }
            }
        }
    }
}
