using System.IO;

namespace AquatoxBasedOptimization.ExternalProgramOperating
{
    public interface IExternalProgramLauncher
    {
        FileInfo File { get; set; }
        void SetParameters(string parameters);
        void Run();
    }
}
