
using System;

namespace ProjetoControleAcessos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Cadastro cadastro = new Cadastro();

            // Carregar dados do banco ao iniciar
            try
            {
                cadastro.Download();
                Console.WriteLine("Dados carregados com sucesso!");
            }
            catch (Exception erro)
            {
                Console.WriteLine("Erro ao carregar os dados: " + erro.Message);
                Console.WriteLine("Verifique o banco antes de continuar.");
                Console.ReadKey();
                return;
            }

            bool sair = false;

            while (!sair)
            {
                MostrarMenu();

                int opcao = LerInteiro("Escolha uma opcao: ");

                switch (opcao)
                {
                    case 0:
                        try
                        {
                            cadastro.Upload();
                            Console.WriteLine("Dados salvos com sucesso!");
                            sair = true;
                        }
                        catch (Exception erro)
                        {
                            Console.WriteLine("Erro ao salvar: " + erro.Message);
                            Console.WriteLine("Tente encerrar novamente.");
                            Pausar();
                        }
                        break;

                    case 1:
                        CadastrarAmbiente(cadastro);
                        Pausar();
                        break;

                    case 2:
                        ConsultarAmbiente(cadastro);
                        Pausar();
                        break;

                    case 3:
                        ExcluirAmbiente(cadastro);
                        Pausar();
                        break;

                    case 4:
                        CadastrarUsuario(cadastro);
                        Pausar();
                        break;

                    case 5:
                        ConsultarUsuario(cadastro);
                        Pausar();
                        break;

                    case 6:
                        ExcluirUsuario(cadastro);
                        Pausar();
                        break;

                    case 7:
                        ConcederPermissao(cadastro);
                        Pausar();
                        break;

                    case 8:
                        RevogarPermissao(cadastro);
                        Pausar();
                        break;

                    case 9:
                        RegistrarAcesso(cadastro);
                        Pausar();
                        break;

                    case 10:
                        ConsultarLogs(cadastro);
                        Pausar();
                        break;

                    default:
                        Console.WriteLine("Opcao invalida!");
                        Pausar();
                        break;
                }
            }

            Console.WriteLine("Programa encerrado!");
        }

        // MENU

        static void MostrarMenu()
        {
            Console.WriteLine("\n===== CONTROLE DE ACESSOS =====");
            Console.WriteLine("0 - Sair");
            Console.WriteLine("1 - Cadastrar ambiente");
            Console.WriteLine("2 - Consultar ambiente");
            Console.WriteLine("3 - Excluir ambiente");
            Console.WriteLine("4 - Cadastrar usuario");
            Console.WriteLine("5 - Consultar usuario");
            Console.WriteLine("6 - Excluir usuario");
            Console.WriteLine("7 - Conceder permissao de acesso");
            Console.WriteLine("8 - Revogar permissao de acesso");
            Console.WriteLine("9 - Registrar acesso");
            Console.WriteLine("10 - Consultar logs de acesso");
            Console.WriteLine("===============================");
        }

        // CADASTRAR AMBIENTE

        static void CadastrarAmbiente(Cadastro cadastro)
        {
            string nome = LerTexto("Nome do ambiente: ");

            Ambiente ambiente = new Ambiente();
            ambiente.Nome = nome;

            cadastro.AdicionarAmbiente(ambiente);

            Console.WriteLine("Ambiente cadastrado com sucesso!");
            Console.WriteLine("ID do ambiente: " + ambiente.Id);
        }

        // CONSULTAR AMBIENTE

        static void ConsultarAmbiente(Cadastro cadastro)
        {
            int id = LerInteiro("Informe o ID do ambiente: ");

            Ambiente ambiente = cadastro.PesquisarAmbiente(
                new Ambiente { Id = id });

            if (ambiente == null)
            {
                Console.WriteLine("Ambiente nao encontrado!");
                return;
            }

            Console.WriteLine("\nID: " + ambiente.Id);
            Console.WriteLine("Nome: " + ambiente.Nome);
            Console.WriteLine("Quantidade de logs: " + ambiente.Logs.Count);
        }

        // EXCLUIR AMBIENTE

        static void ExcluirAmbiente(Cadastro cadastro)
        {
            int id = LerInteiro("Informe o ID do ambiente: ");

            Ambiente ambiente = cadastro.PesquisarAmbiente(
                new Ambiente { Id = id });

            if (ambiente == null)
            {
                Console.WriteLine("Ambiente nao encontrado!");
                return;
            }

            if (cadastro.RemoverAmbiente(ambiente))
                Console.WriteLine("Ambiente removido com sucesso!");
            else
                Console.WriteLine("Nao foi possivel remover. Existem usuarios com permissao nesse ambiente.");
        }

        // CADASTRAR USUARIO

        static void CadastrarUsuario(Cadastro cadastro)
        {
            string nome = LerTexto("Nome do usuario: ");

            Usuario usuario = new Usuario();
            usuario.Nome = nome;

            cadastro.AdicionarUsuario(usuario);

            Console.WriteLine("Usuario cadastrado com sucesso!");
            Console.WriteLine("ID do usuario: " + usuario.Id);
        }

        // CONSULTAR USUARIO

        static void ConsultarUsuario(Cadastro cadastro)
        {
            int id = LerInteiro("Informe o ID do usuario: ");

            Usuario usuario = cadastro.PesquisarUsuario(
                new Usuario { Id = id });

            if (usuario == null)
            {
                Console.WriteLine("Usuario nao encontrado!");
                return;
            }

            Console.WriteLine("\nID: " + usuario.Id);
            Console.WriteLine("Nome: " + usuario.Nome);

            Console.WriteLine("Ambientes autorizados:");

            if (usuario.Ambientes.Count == 0)
            {
                Console.WriteLine("Nenhum ambiente autorizado.");
            }
            else
            {
                foreach (Ambiente ambiente in usuario.Ambientes)
                {
                    Console.WriteLine(
                        ambiente.Id + " - " + ambiente.Nome);
                }
            }
        }

        // EXCLUIR USUARIO

        static void ExcluirUsuario(Cadastro cadastro)
        {
            int id = LerInteiro("Informe o ID do usuario: ");

            Usuario usuario = cadastro.PesquisarUsuario(
                new Usuario { Id = id });

            if (usuario == null)
            {
                Console.WriteLine("Usuario nao encontrado!");
                return;
            }

            if (cadastro.RemoverUsuario(usuario))
            {
                Console.WriteLine("Usuario removido com sucesso!");
            }
            else
            {
                Console.WriteLine(
                    "Nao foi possivel remover. Revogue todas as permissoes primeiro.");
            }
        }

        // CONCEDER PERMISSAO

        static void ConcederPermissao(Cadastro cadastro)
        {
            int idAmbiente = LerInteiro("ID do ambiente: ");
            int idUsuario = LerInteiro("ID do usuario: ");

            Ambiente ambiente = cadastro.PesquisarAmbiente(
                new Ambiente { Id = idAmbiente });

            Usuario usuario = cadastro.PesquisarUsuario(
                new Usuario { Id = idUsuario });

            if (ambiente == null || usuario == null)
            {
                Console.WriteLine("Usuario ou ambiente nao encontrado!");
                return;
            }

            if (usuario.ConcederPermissao(ambiente))
                Console.WriteLine("Permissao concedida com sucesso!");
            else
                Console.WriteLine("Este usuario ja possui permissao nesse ambiente!");
        }

        // REVOGAR PERMISSAO

        static void RevogarPermissao(Cadastro cadastro)
        {
            int idAmbiente = LerInteiro("ID do ambiente: ");
            int idUsuario = LerInteiro("ID do usuario: ");

            Ambiente ambiente = cadastro.PesquisarAmbiente(
                new Ambiente { Id = idAmbiente });

            Usuario usuario = cadastro.PesquisarUsuario(
                new Usuario { Id = idUsuario });

            if (ambiente == null || usuario == null)
            {
                Console.WriteLine("Usuario ou ambiente nao encontrado!");
                return;
            }

            if (usuario.RevogarPermissao(ambiente))
                Console.WriteLine("Permissao revogada com sucesso!");
            else
                Console.WriteLine("Este usuario nao possui permissao nesse ambiente!");
        }

        // REGISTRAR ACESSO

        static void RegistrarAcesso(Cadastro cadastro)
        {
            int idAmbiente = LerInteiro("ID do ambiente: ");
            int idUsuario = LerInteiro("ID do usuario: ");

            Ambiente ambiente = cadastro.PesquisarAmbiente(
                new Ambiente { Id = idAmbiente });

            Usuario usuario = cadastro.PesquisarUsuario(
                new Usuario { Id = idUsuario });

            if (ambiente == null || usuario == null)
            {
                Console.WriteLine("Usuario ou ambiente nao encontrado!");
                return;
            }

            bool autorizado = usuario.Ambientes.Exists(
                a => a.Id == ambiente.Id);

            Log log = new Log();

            log.DtAcesso = DateTime.Now;
            log.Usuario = usuario;
            log.Ambiente = ambiente;
            log.TipoAcesso = autorizado;

            ambiente.RegistrarLog(log);

            if (autorizado)
                Console.WriteLine("ACESSO AUTORIZADO!");
            else
                Console.WriteLine("ACESSO NEGADO!");

            Console.WriteLine("Tentativa registrada no log.");
        }

        // CONSULTAR LOGS

        static void ConsultarLogs(Cadastro cadastro)
        {
            int id = LerInteiro("ID do ambiente: ");

            Ambiente ambiente = cadastro.PesquisarAmbiente(
                new Ambiente { Id = id });

            if (ambiente == null)
            {
                Console.WriteLine("Ambiente nao encontrado!");
                return;
            }

            Console.WriteLine("\n1 - Somente autorizados");
            Console.WriteLine("2 - Somente negados");
            Console.WriteLine("3 - Todos");

            int filtro = LerInteiro("Escolha o filtro: ");

            if (filtro < 1 || filtro > 3)
            {
                Console.WriteLine("Filtro invalido!");
                return;
            }

            Console.WriteLine("\n===== LOGS DE ACESSO =====");

            int quantidade = 0;

            foreach (Log log in ambiente.Logs)
            {
                bool mostrar =
                    filtro == 3 ||
                    (filtro == 1 && log.TipoAcesso) ||
                    (filtro == 2 && !log.TipoAcesso);

                if (mostrar)
                {
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("Data: " + log.DtAcesso);
                    Console.WriteLine("Usuario: " + log.Usuario.Nome);

                    Console.WriteLine(
                        "Resultado: " +
                        (log.TipoAcesso ? "AUTORIZADO" : "NEGADO"));

                    quantidade++;
                }
            }

            if (quantidade == 0)
                Console.WriteLine("Nenhum log encontrado.");

            Console.WriteLine("==========================");
        }

        // METODOS AUXILIARES

        static int LerInteiro(string mensagem)
        {
            int valor;

            while (true)
            {
                Console.Write(mensagem);

                if (int.TryParse(Console.ReadLine(), out valor))
                    return valor;

                Console.WriteLine("Digite um numero valido!");
            }
        }

        static string LerTexto(string mensagem)
        {
            while (true)
            {
                Console.Write(mensagem);

                string texto = Console.ReadLine() ?? "";

                if (!string.IsNullOrWhiteSpace(texto))
                    return texto.Trim();

                Console.WriteLine("O texto nao pode ficar vazio!");
            }
        }

        static void Pausar()
        {
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}
