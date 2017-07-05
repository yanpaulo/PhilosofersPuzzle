using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PhilosofersPuzzle.Server
{
    class Garfo
    {
        public Semaphore Semaforo { get; private set; } = new Semaphore(1, 1);
    }
}
