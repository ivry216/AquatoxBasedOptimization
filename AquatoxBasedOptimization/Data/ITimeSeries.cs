using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.Data
{
    public interface ITimeSeries
    {
        int Size { get; }
        double[] Values { get; }
        DateTime[] Times { get; }
        string Name { get; }
    }
}
