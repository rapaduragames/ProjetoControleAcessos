
using Microsoft.Data.SqlClient;

namespace ProjetoControleAcessos
{
    public class Conexao
    {
        private string conexaoString =
            @"Server=.\SQLEXPRESS;Database=ControleAcessos;Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection Conectar()
        {
            return new SqlConnection(conexaoString);
        }
    }
}
