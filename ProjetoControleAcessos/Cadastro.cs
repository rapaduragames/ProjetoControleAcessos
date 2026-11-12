
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ProjetoControleAcessos
{
    public class Cadastro
    {
        private List<Usuario> usuarios;
        private List<Ambiente> ambientes;

        public Cadastro()
        {
            usuarios = new List<Usuario>();
            ambientes = new List<Ambiente>();
        }

        // CADASTRO DE USUARIOS

        public void AdicionarUsuario(Usuario usuario)
        {
            if (usuario.Id == 0)
                usuario.Id = ProximoIdUsuario();

            if (PesquisarUsuario(usuario) == null)
                usuarios.Add(usuario);
        }

        public bool RemoverUsuario(Usuario usuario)
        {
            Usuario encontrado = PesquisarUsuario(usuario);

            if (encontrado == null)
                return false;

            if (encontrado.Ambientes.Count > 0)
                return false;

            // Remove os logs relacionados ao usuario excluido
            foreach (Ambiente ambiente in ambientes)
            {
                Queue<Log> novosLogs = new Queue<Log>();

                foreach (Log log in ambiente.Logs)
                {
                    if (log.Usuario.Id != encontrado.Id)
                        novosLogs.Enqueue(log);
                }

                ambiente.Logs = novosLogs;
            }

            usuarios.Remove(encontrado);
            return true;
        }

        public Usuario PesquisarUsuario(Usuario usuario)
        {
            return usuarios.Find(u => u.Id == usuario.Id);
        }

        // CADASTRO DE AMBIENTES

        public void AdicionarAmbiente(Ambiente ambiente)
        {
            if (ambiente.Id == 0)
                ambiente.Id = ProximoIdAmbiente();

            if (PesquisarAmbiente(ambiente) == null)
                ambientes.Add(ambiente);
        }

        public bool RemoverAmbiente(Ambiente ambiente)
        {
            Ambiente encontrado = PesquisarAmbiente(ambiente);

            if (encontrado == null)
                return false;

            foreach (Usuario usuario in usuarios)
            {
                if (usuario.Ambientes.Exists(
                    a => a.Id == encontrado.Id))
                    return false;
            }

            ambientes.Remove(encontrado);
            return true;
        }

        public Ambiente PesquisarAmbiente(Ambiente ambiente)
        {
            return ambientes.Find(a => a.Id == ambiente.Id);
        }

        // GERACAO DE IDS

        private int ProximoIdUsuario()
        {
            int maiorId = 0;

            foreach (Usuario usuario in usuarios)
            {
                if (usuario.Id > maiorId)
                    maiorId = usuario.Id;
            }

            return maiorId + 1;
        }

        private int ProximoIdAmbiente()
        {
            int maiorId = 0;

            foreach (Ambiente ambiente in ambientes)
            {
                if (ambiente.Id > maiorId)
                    maiorId = ambiente.Id;
            }

            return maiorId + 1;
        }

        // DOWNLOAD: CARREGAR DADOS DO BANCO

        public void Download()
        {
            usuarios.Clear();
            ambientes.Clear();

            using (SqlConnection conexao = new Conexao().Conectar())
            {
                conexao.Open();

                // Carregar usuarios

                string sqlUsuarios =
                    "SELECT Id, Nome FROM Usuario";

                using (SqlCommand comando =
                    new SqlCommand(sqlUsuarios, conexao))
                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        Usuario usuario = new Usuario();

                        usuario.Id = leitor.GetInt32(0);
                        usuario.Nome = leitor.GetString(1);

                        usuarios.Add(usuario);
                    }
                }

                // Carregar ambientes

                string sqlAmbientes =
                    "SELECT Id, Nome FROM Ambiente";

                using (SqlCommand comando =
                    new SqlCommand(sqlAmbientes, conexao))
                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        Ambiente ambiente = new Ambiente();

                        ambiente.Id = leitor.GetInt32(0);
                        ambiente.Nome = leitor.GetString(1);

                        ambientes.Add(ambiente);
                    }
                }

                // Carregar permissoes

                string sqlPermissoes =
                    "SELECT IdUsuario, IdAmbiente FROM Permissao";

                using (SqlCommand comando =
                    new SqlCommand(sqlPermissoes, conexao))
                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        int idUsuario = leitor.GetInt32(0);
                        int idAmbiente = leitor.GetInt32(1);

                        Usuario usuario =
                            usuarios.Find(u => u.Id == idUsuario);

                        Ambiente ambiente =
                            ambientes.Find(a => a.Id == idAmbiente);

                        if (usuario != null && ambiente != null)
                            usuario.ConcederPermissao(ambiente);
                    }
                }

                // Carregar logs

                string sqlLogs =
                    @"SELECT DtAcesso, IdUsuario, IdAmbiente, TipoAcesso
                      FROM [Log]
                      ORDER BY DtAcesso ASC, Id ASC";

                using (SqlCommand comando =
                    new SqlCommand(sqlLogs, conexao))
                using (SqlDataReader leitor = comando.ExecuteReader())
                {
                    while (leitor.Read())
                    {
                        DateTime data = leitor.GetDateTime(0);
                        int idUsuario = leitor.GetInt32(1);
                        int idAmbiente = leitor.GetInt32(2);
                        bool tipoAcesso = leitor.GetBoolean(3);

                        Usuario usuario =
                            usuarios.Find(u => u.Id == idUsuario);

                        Ambiente ambiente =
                            ambientes.Find(a => a.Id == idAmbiente);

                        if (usuario != null && ambiente != null)
                        {
                            Log log = new Log();

                            log.DtAcesso = data;
                            log.Usuario = usuario;
                            log.Ambiente = ambiente;
                            log.TipoAcesso = tipoAcesso;

                            ambiente.RegistrarLog(log);
                        }
                    }
                }
            }
        }

        // UPLOAD: SALVAR DADOS NO BANCO

        public void Upload()
        {
            using (SqlConnection conexao = new Conexao().Conectar())
            {
                conexao.Open();

                using (SqlTransaction transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        // Limpar os dados anteriores

                        ExecutarComando(conexao, transacao,
                            "DELETE FROM [Log]");

                        ExecutarComando(conexao, transacao,
                            "DELETE FROM Permissao");

                        ExecutarComando(conexao, transacao,
                            "DELETE FROM Usuario");

                        ExecutarComando(conexao, transacao,
                            "DELETE FROM Ambiente");

                        // Salvar usuarios

                        ExecutarComando(conexao, transacao,
                            "SET IDENTITY_INSERT Usuario ON");

                        foreach (Usuario usuario in usuarios)
                        {
                            string sql =
                                @"INSERT INTO Usuario (Id, Nome)
                                  VALUES (@Id, @Nome)";

                            using (SqlCommand comando =
                                new SqlCommand(sql, conexao, transacao))
                            {
                                comando.Parameters.Add("@Id",
                                    SqlDbType.Int).Value = usuario.Id;

                                comando.Parameters.Add("@Nome",
                                    SqlDbType.VarChar, 100).Value =
                                    usuario.Nome;

                                comando.ExecuteNonQuery();
                            }
                        }

                        ExecutarComando(conexao, transacao,
                            "SET IDENTITY_INSERT Usuario OFF");

                        // Salvar ambientes

                        ExecutarComando(conexao, transacao,
                            "SET IDENTITY_INSERT Ambiente ON");

                        foreach (Ambiente ambiente in ambientes)
                        {
                            string sql =
                                @"INSERT INTO Ambiente (Id, Nome)
                                  VALUES (@Id, @Nome)";

                            using (SqlCommand comando =
                                new SqlCommand(sql, conexao, transacao))
                            {
                                comando.Parameters.Add("@Id",
                                    SqlDbType.Int).Value = ambiente.Id;

                                comando.Parameters.Add("@Nome",
                                    SqlDbType.VarChar, 100).Value =
                                    ambiente.Nome;

                                comando.ExecuteNonQuery();
                            }
                        }

                        ExecutarComando(conexao, transacao,
                            "SET IDENTITY_INSERT Ambiente OFF");

                        // Salvar permissoes

                        foreach (Usuario usuario in usuarios)
                        {
                            foreach (Ambiente ambiente in usuario.Ambientes)
                            {
                                string sql =
                                    @"INSERT INTO Permissao
                                      (IdUsuario, IdAmbiente)
                                      VALUES (@IdUsuario, @IdAmbiente)";

                                using (SqlCommand comando =
                                    new SqlCommand(sql, conexao, transacao))
                                {
                                    comando.Parameters.Add("@IdUsuario",
                                        SqlDbType.Int).Value = usuario.Id;

                                    comando.Parameters.Add("@IdAmbiente",
                                        SqlDbType.Int).Value = ambiente.Id;

                                    comando.ExecuteNonQuery();
                                }
                            }
                        }

                        // Salvar logs

                        foreach (Ambiente ambiente in ambientes)
                        {
                            foreach (Log log in ambiente.Logs)
                            {
                                string sql =
                                    @"INSERT INTO [Log]
                                      (DtAcesso, IdUsuario, IdAmbiente, TipoAcesso)
                                      VALUES
                                      (@Data, @IdUsuario, @IdAmbiente, @Tipo)";

                                using (SqlCommand comando =
                                    new SqlCommand(sql, conexao, transacao))
                                {
                                    comando.Parameters.Add("@Data",
                                        SqlDbType.DateTime).Value =
                                        log.DtAcesso;

                                    comando.Parameters.Add("@IdUsuario",
                                        SqlDbType.Int).Value =
                                        log.Usuario.Id;

                                    comando.Parameters.Add("@IdAmbiente",
                                        SqlDbType.Int).Value =
                                        ambiente.Id;

                                    comando.Parameters.Add("@Tipo",
                                        SqlDbType.Bit).Value =
                                        log.TipoAcesso;

                                    comando.ExecuteNonQuery();
                                }
                            }
                        }

                        transacao.Commit();
                    }
                    catch
                    {
                        transacao.Rollback();
                        throw;
                    }
                }
            }
        }

        // EXECUTAR COMANDOS SQL

        private void ExecutarComando(
            SqlConnection conexao,
            SqlTransaction transacao,
            string sql)
        {
            using (SqlCommand comando =
                new SqlCommand(sql, conexao, transacao))
            {
                comando.ExecuteNonQuery();
            }
        }
    }
}
