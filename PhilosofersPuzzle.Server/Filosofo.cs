using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;

namespace PhilosofersPuzzle.Server
{
    class Filosofo
    {
        public int Id { get; set; }

        public NamedPipeServerStream Stream { get; set; }

        public Garfo[] Garfos { get; set; }
    }
}
