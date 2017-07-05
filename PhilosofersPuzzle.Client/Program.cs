using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace PhilosofersPuzzle.Client
{
    class Program
    {
        private static readonly string WaitPipeName = "YansCorp.PP";
        private static readonly Random rng = new Random();

        static void Main(string[] args)
        {
            int n = args.Length > 0 ? int.Parse(args[0]) : 2;
            for (int i = 0; i < n; i++)
            {
                var t = new Thread(MetodoFilosofo);
                t.Start();
                Thread.Sleep(500);
            }
        }

        private static void MetodoFilosofo()
        {
            var stream = new NamedPipeClientStream(".", WaitPipeName, PipeDirection.InOut);
            stream.Connect();

            int id = stream.ReadByte();
            using (var writer = new StreamWriter(stream) { AutoFlush = true })
            {
                while (stream.IsConnected)
                {
                    Console.WriteLine($"Filosofo {id} esta pegando garfos.");
                    writer.WriteLine("pega");
                    stream.ReadByte();

                    Console.WriteLine($"Filosofo {id} esta comendo.");
                    Thread.Sleep(rng.Next(10, 15) * 1000);

                    writer.WriteLine("solta");
                    Console.WriteLine($"Filosofo {id} soltou os garfos e esta filosofando.");
                    Thread.Sleep(rng.Next(10, 15) * 1000);
                }
            }
        }
    }
}