using AquatoxBasedOptimization.ExternalProgramOperating;
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
            SimpleSingleLauncher simpleSingleLauncher = new SimpleSingleLauncher();
            simpleSingleLauncher.File = new System.IO.FileInfo(@"C:/Users/ivanry/AQUATOX R3.2/PROGRAM/aquatox.exe");
            simpleSingleLauncher.SetParameters("ECSAVE \"C:/Users/ivanry/AQUATOX R3.2/STUDIES/Lake Pyhajarvi Finland.txt\" \"test1.txt\"");

            Console.WriteLine("Starting...");

            simpleSingleLauncher.Run();

            Console.WriteLine("End!");
            Console.Read();
        }
    }
}
