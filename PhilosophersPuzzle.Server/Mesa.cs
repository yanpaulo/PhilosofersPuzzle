using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Linq;
using System.Threading;

namespace PhilosophersPuzzle.Server
{
    public class Mesa
    {
        private readonly string PipeName = "YansCorp.PP";

        private List<Thread> _threads = new List<Thread>();
        private List<Garfo> _garfos = new List<Garfo>();
        private List<Filosofo> _filosofos = new List<Filosofo>();
        private Random _rng = new Random();

        public void Liga()
        {
            while (true)
            {
                RecebeFilosofo();
            }
        }

        public void RecebeFilosofo()
        {
            var stream = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 100);
            stream.WaitForConnection();

            //Somente adiciona um garfo na mesa se já houverem pelo menos três filósofos.
            if (_filosofos.Count >= 3)
            {
                _garfos.Add(new Garfo()); 
            }

            var filosofo = new Filosofo
            {
                Id = _filosofos.Count,
                Stream = stream,
                Garfos = _garfos.Any() ? 
                            //Se houverem garfos na mesa, pega o último e o primeiro.
                            new[] { _garfos.Last(), _garfos.First() } :
                            null
            };

            _filosofos.Add(filosofo);

            var thread = new Thread(() => AtendeFilosofo(filosofo));
            _threads.Add(thread);

            //Se houverem mais que três filósofos na mesa, somente inicia a thread.
            if (_filosofos.Count > 3)
            {
                thread.Start();
            }
            //Se o filósofo recém-chegado for o terceiro.
            else if (_filosofos.Count == 3)
            {
                //Coloca 3 garfos na mesa
                _garfos.AddRange(new[] { new Garfo(), new Garfo(), new Garfo() });

                //Entrega os garfos certos para cada filósofo
                for (int i = 0; i < 2; i++)
                {
                    _filosofos[i].Garfos = _garfos.Skip(i).Take(2).ToArray();
                }
                _filosofos[2].Garfos = new[] { _garfos.Last(), _garfos.First() };

                //Inicia as threads
                foreach (var t in _threads)
                {
                    t.Start();
                }
            }
        }

        private void AtendeFilosofo(Filosofo filosofo)
        {
            var stream = filosofo.Stream;

            stream.WriteByte((byte)filosofo.Id);
            using (var reader = new StreamReader(stream))
            {
                while (stream.IsConnected)
                {
                    var message = reader.ReadLine();


                    if (message == "pega")
                    {
                        //Índice de um garfo aleatório
                        var garfoIndex = _rng.Next(0, 2);
                        var garfo = filosofo.Garfos[garfoIndex];
                        Console.WriteLine($"Filosofo {filosofo.Id} pegando garfo {_garfos.IndexOf(garfo)}");
                        garfo.Semaforo.WaitOne();
                        Console.WriteLine($"Filosofo {filosofo.Id} pegou garfo {_garfos.IndexOf(garfo)}");

                        garfoIndex = 1 - garfoIndex;
                        garfo = filosofo.Garfos[garfoIndex];
                        Console.WriteLine($"Filosofo {filosofo.Id} pegando garfo {_garfos.IndexOf(garfo)}");
                        garfo.Semaforo.WaitOne();
                        Console.WriteLine($"Filosofo {filosofo.Id} pegou garfo {_garfos.IndexOf(garfo)}");

                        stream.WriteByte(1);
                    }
                    else if (message == "solta")
                    {
                        Console.WriteLine($"Filosofo {filosofo.Id} soltando garfos ({_garfos.IndexOf(filosofo.Garfos[0])}, {_garfos.IndexOf(filosofo.Garfos[1])})");
                        foreach (var g in filosofo.Garfos)
                        {
                            g.Semaforo.Release();
                        }
                    }
                    else
                    {
                        //Eu num intindi o que ele falô
                    }


                }
            }
        }
    }
}
