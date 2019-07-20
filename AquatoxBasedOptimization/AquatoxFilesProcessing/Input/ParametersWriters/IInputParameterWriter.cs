namespace AquatoxBasedOptimization.AquatoxFilesProcessing.Input.ParametersWriters
{
    interface IInputParameterWriter
    {
        string ParameterName { get; }
        ParameterLocationType ParameterLocationType { get; }
        string Write(string parameterValue);
    }
}
