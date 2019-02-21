using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquatoxBasedOptimization
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessStartInfo info = new ProcessStartInfo();

            info.FileName = "\"C:/Users/ivanry/AQUATOX R3.2/PROGRAM/aquatox.exe\"";
            info.Arguments = "EPSAVE \"C:/Users/ivanry/AQUATOX R3.2/STUDIES/Lake Pyhajarvi Finland.aps\" \"test.txt\"";

            Console.WriteLine("Starting...");

            using (Process process = Process.Start(info))
            {
                process.WaitForExit();
            }

            Console.WriteLine("End!");
            Console.Read();
        }
    }
}
