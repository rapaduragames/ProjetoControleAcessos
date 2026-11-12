# Projeto Controle de Acessos

## Sobre o projeto

O Projeto Controle de Acessos é uma aplicação desenvolvida em C# utilizando o modelo Console Application e banco de dados SQL Server.

O sistema tem como objetivo gerenciar usuários, ambientes e permissões de acesso, permitindo registrar e consultar tentativas de acesso, tanto autorizadas quanto negadas.

## Tecnologias utilizadas

- C#
- .NET
- SQL Server
- Microsoft SQL Server Management Studio (SSMS)
- Visual Studio 2022

## Funcionalidades

- Cadastro de usuários.
- Consulta e exclusão de usuários.
- Cadastro de ambientes.
- Consulta e exclusão de ambientes.
- Concessão de permissões de acesso.
- Revogação de permissões.
- Registro de tentativas de acesso.
- Identificação de acessos autorizados e negados.
- Consulta de logs por tipo de acesso.
- Armazenamento dos registros no banco de dados SQL Server.
- Limite de 100 registros de log por ambiente, mantendo os mais recentes.

## Estrutura do projeto

O projeto é organizado nas seguintes classes:

- **Usuario:** representa os usuários cadastrados e suas permissões.
- **Ambiente:** representa os ambientes e mantém seus registros de acesso.
- **Log:** armazena as informações das tentativas de acesso.
- **Cadastro:** gerencia usuários, ambientes, permissões e persistência dos dados.
- **Conexao:** realiza a conexão com o banco de dados SQL Server.
- **Program:** contém o menu principal e a interação com o usuário.

## Banco de dados

O sistema utiliza o SQL Server com um banco de dados chamado `ControleAcessos`.

Para executar o projeto, é necessário criar o banco de dados e suas tabelas no SQL Server, além de configurar a string de conexão na classe `Conexao.cs`.

## Como executar

1. Ter o Visual Studio 2022 instalado.
2. Ter o SQL Server instalado e funcionando.
3. Criar o banco de dados `ControleAcessos` e suas tabelas.
4. Conferir a string de conexão na classe `Conexao.cs`.
5. Abrir o projeto no Visual Studio.
6. Executar a aplicação.

## Objetivo acadêmico

Projeto desenvolvido com o objetivo de aplicar conceitos de programação orientada a objetos, estruturas de dados, integração com banco de dados e gerenciamento de permissões de acesso.
