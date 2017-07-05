using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace PhilosophersPuzzle.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var mesa = new Mesa();
            mesa.Liga();
        }
    }
}