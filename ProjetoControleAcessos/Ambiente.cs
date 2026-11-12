
using System;
using System.Collections.Generic;

namespace ProjetoControleAcessos
{
    public class Ambiente
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public Queue<Log> Logs { get; set; }

        public Ambiente()
        {
            Logs = new Queue<Log>();
        }

        public void RegistrarLog(Log log)
        {
            if (Logs.Count >= 100)
            {
                Logs.Dequeue();
            }

            Logs.Enqueue(log);
        }
    }
}
