using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace PhilosophersPuzzle.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            int numArgs = args.Length;
            int n = numArgs > 0 ? int.Parse(args[0]) : 3;

            var client = new Cliente(n);
            if (numArgs >= 2)
            {
                client.MinTime = int.Parse(args[1]);
            }
            if (numArgs >= 3)
            {
                client.MaxTime = int.Parse(args[2]);
            }
            client.Inicia();
            Console.ReadKey();
        }
    }
}