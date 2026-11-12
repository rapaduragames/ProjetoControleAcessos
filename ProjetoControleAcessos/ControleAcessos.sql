
CREATE DATABASE ControleAcessos;
GO

USE ControleAcessos;
GO

CREATE TABLE Usuario (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome VARCHAR(100) NOT NULL
);

CREATE TABLE Ambiente (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nome VARCHAR(100) NOT NULL
);

CREATE TABLE Permissao (
    IdUsuario INT NOT NULL,
    IdAmbiente INT NOT NULL,
    PRIMARY KEY (IdUsuario, IdAmbiente),
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id),
    FOREIGN KEY (IdAmbiente) REFERENCES Ambiente(Id)
);

CREATE TABLE Log (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DtAcesso DATETIME NOT NULL,
    IdUsuario INT NOT NULL,
    IdAmbiente INT NOT NULL,
    TipoAcesso BIT NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id),
    FOREIGN KEY (IdAmbiente) REFERENCES Ambiente(Id)
);
