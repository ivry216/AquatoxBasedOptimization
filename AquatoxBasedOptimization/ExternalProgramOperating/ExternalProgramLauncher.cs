﻿using AquatoxBasedOptimization.ExternalProgramOperating.OperatingStrategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.ExternalProgramOperating
{
    public abstract class ExternalProgramLauncher<TStrategy> : IExternalProgramLauncher
        where TStrategy : IOperatingStrategyParametrized, new()
    {
        protected FileInfo fileInfo;
        protected TStrategy operatingStrategy;

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

        public abstract void Run();
        public abstract void Run(string parameters);
    }
}
