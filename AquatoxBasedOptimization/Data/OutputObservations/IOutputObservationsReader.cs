using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization.Data.OutputObservations
{
    public interface IOutputObservationsReader
    {
        //
        void SetVariableNames(Dictionary<string, string> variables);
        //
        Dictionary<string, IOutputObservation> ReadOutputVariableObservations();
    }
}