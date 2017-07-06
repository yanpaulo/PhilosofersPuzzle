using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace PhilosophersPuzzle.Client
{
    class Cliente
    {
        private readonly string PipeName = "YansCorp.PP";
        private readonly Random rng = new Random();

        public int MinTime { get; set; } = 10;
        public int MaxTime { get; set; } = 15;
        public int NumFilosofos { get; private set; }

        public Cliente(int numFilosofos)
        {
            NumFilosofos = numFilosofos;
        }

        public void Inicia()
        {
            for (int i = 0; i < NumFilosofos; i++)
            {
                var t = new Thread(MetodoFilosofo);
                t.Start();
            }
        }

        private void MetodoFilosofo()
        {
            var stream = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut);
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
