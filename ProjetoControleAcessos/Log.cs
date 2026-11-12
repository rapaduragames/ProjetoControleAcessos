
using System;

namespace ProjetoControleAcessos
{
    public class Log
    {
        public DateTime DtAcesso { get; set; }

        public Usuario Usuario { get; set; }

        public Ambiente Ambiente { get; set; }

        public bool TipoAcesso { get; set; }

        public Log()
        {
            DtAcesso = DateTime.Now;
        }
    }
}
