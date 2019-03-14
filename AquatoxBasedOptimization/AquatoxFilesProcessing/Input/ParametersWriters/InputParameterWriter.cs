using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Input.ParametersWriters
{
    public class InputParameterWriter : IInputParameterWriter
    {
        private string _inputString;
        private string _leftImmutablePart;
        private string _rightImmutablePart;

        public string ParameterName { get; }
        public ParameterLocationType ParameterLocationType { get; private set; }

        public InputParameterWriter(string inputString, string parameter)
        {
            ParameterName = parameter;

            IdentifyImmutableParts(inputString);
        }

        public string Write(string parameterValue)
        {
            // Set the initial value for the string builder
            StringBuilder stringBuilder = new StringBuilder(_leftImmutablePart);
            stringBuilder.Append(parameterValue);
            stringBuilder.Append(_rightImmutablePart);

            return stringBuilder.ToString();
        }

        private void IdentifyImmutableParts(string inputString)
        {
            // Initialize input string
            _inputString = inputString;
            // Split it with the parameter name
            var splitted = inputString.Split(new string[] { ParameterName }, StringSplitOptions.None);
            // Get its left and right parts
            _leftImmutablePart = splitted[0];
            _rightImmutablePart = splitted[1];
            // Identify the type
            if (_leftImmutablePart == "")
            {
                ParameterLocationType = ParameterLocationType.Left;
            }
            else if (_rightImmutablePart == "")
            {
                ParameterLocationType = ParameterLocationType.Right;
            }
            else
            {
                if (_leftImmutablePart == "" && _rightImmutablePart == "")
                    throw new Exception("Wrong parameter location in parameter string");

                ParameterLocationType = ParameterLocationType.Center;
            }
        }
    }
}
