
using System.Collections.Generic;

namespace ProjetoControleAcessos
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public List<Ambiente> Ambientes { get; set; }

        public Usuario()
        {
            Ambientes = new List<Ambiente>();
        }

        public bool ConcederPermissao(Ambiente ambiente)
        {
            if (Ambientes.Exists(a => a.Id == ambiente.Id))
                return false;

            Ambientes.Add(ambiente);
            return true;
        }

        public bool RevogarPermissao(Ambiente ambiente)
        {
            Ambiente encontrado = Ambientes.Find(a => a.Id == ambiente.Id);

            if (encontrado == null)
                return false;

            Ambientes.Remove(encontrado);
            return true;
        }
    }
}
