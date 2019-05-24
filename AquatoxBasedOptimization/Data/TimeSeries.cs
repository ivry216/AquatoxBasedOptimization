using AquatoxBasedOptimization.Extensions.Array;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.Data
{
    public class TimeSeries : ITimeSeries
    {
        #region Properties

        public string Name { get; set; }

        public int Size { get; set; }

        public double[] Values { get; set; }

        public DateTime[] Times { get; set; }

        #endregion Properties

        #region Constructor

        public TimeSeries(string name, double[] values, DateTime[] times)
        {
            // Set name
            Name = name;

            // Initialize size
            Size = values.Length;

            // Check the size
            if (Size != times.Length)
                throw new Exception("The size of timestamps and values are different");

            // Initialize arrays
            Values = new double[Size];
            Times = new DateTime[Size];

            // Assign values
            Values.CopyFrom(values);
            Times.CopyFrom(times);
        }

        #endregion Constructor
    }
}