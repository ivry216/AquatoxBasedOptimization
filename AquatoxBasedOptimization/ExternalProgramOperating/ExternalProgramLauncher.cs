using AquatoxBasedOptimization.ExternalProgramOperating.OperatingStrategies;
using System.IO;

namespace AquatoxBasedOptimization.ExternalProgramOperating
{
    public abstract class ExternalProgramLauncher<TStrategy> : IExternalProgramLauncher
        where TStrategy : IOperatingStrategyParametrized, new()
    {
        #region Fields

        protected FileInfo fileInfo;
        protected TStrategy operatingStrategy;
        protected string parameters;

        #endregion Fields

        #region Properties

        public FileInfo File
        {
            get => fileInfo;
            set => fileInfo = new FileInfo(value.FullName);
        }

        #endregion Properties

        #region Constructor

        public ExternalProgramLauncher()
        {
            operatingStrategy = new TStrategy();
        }

        public ExternalProgramLauncher(string executiveFilePath) : this()
        {
            fileInfo = new FileInfo(executiveFilePath);
        }

        public ExternalProgramLauncher(FileInfo fileInfo) : this()
        {
            this.fileInfo = new FileInfo(fileInfo.FullName);
        }

        #endregion Constructor

        #region Main Methods

        public void SetParameters(string parameters)
        {
            this.parameters = parameters;
        }

        public abstract void Run();

        #endregion Main Methods
    }
}