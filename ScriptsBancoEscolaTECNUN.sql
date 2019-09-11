CREATE DATABASE EscolaTECNUN;

USE EscolaTECNUN;

CREATE TABLE Aluno (
    id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    nome varchar(255) NOT NULL,
    datanasc datetime NOT NULL,
    telefone varchar(255) NOT NULL,
    cpf varchar(255) NOT NULL,
	email varchar(255),
	infoadic varchar(255)
);

CREATE TABLE Professor (
    id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    nome varchar(255) NOT NULL,
    datanasc datetime NOT NULL,
    telefone varchar(255),
    cpf varchar(255) NOT NULL
);

CREATE TABLE Turma (
    id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    numturma varchar(255) NOT NULL,
    dataturma datetime NOT NULL,
    periodo varchar(255),
    horario varchar(255),
	professorid int NOT NULL,
	FOREIGN KEY(professorid) REFERENCES professor(id)
);


CREATE TABLE Matricula (
    id int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    turmaid int NOT NULL,
	alunoid int NOT NULL,
	FOREIGN KEY(alunoid) REFERENCES aluno(id),
	FOREIGN KEY(turmaid) REFERENCES turma(id)
);