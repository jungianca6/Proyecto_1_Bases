
--CREATE DATABASE CEDigital;


CREATE TABLE Grupo(
    ID_Grupo int NOT NULL PRIMARY KEY,
    Número_grupo int NOT NULL,
	Carnet int
);

CREATE TABLE Curso(
	ID_Curso int NOT NULL PRIMARY KEY,
	Nombre varchar(60) NOT NULL,
	Codigo_Curso varchar(20) NOT NULL,
	Carrera varchar(50) NOT NULL,
	Creditos int NOT NULL,
	ID_Grupo int
);

CREATE TABLE Semestre(
	ID_Semestre int NOT NULL PRIMARY KEY,
	Año int NOT NULL
);

CREATE TABLE Estudiante(
	Carnet int NOT NULL PRIMARY KEY,
);

CREATE TABLE Profesor(
	Cedula int NOT NULL PRIMARY KEY,
);

CREATE TABLE Noticia(
	ID_Noticia int NOT NULL PRIMARY KEY,
	Mensaje varchar(100),
	Titulo varchar(50),
	Cedula int
);

CREATE TABLE Carpeta(
	ID_Carpeta int NOT NULL PRIMARY KEY,
	Nombre varchar(50) NOT NULL,
	ID_Grupo int
);

CREATE TABLE Documento(
	ID_Documento int NOT NULL PRIMARY KEY,
	Nombre varchar(60) NOT NULL,
	Ruta varchar(20) NOT NULL,
	Tamaño varchar(50) NOT NULL,
	Fecha_subida DATE NOT NULL,
	Subida_por_Profesor BIT,
	ID_Carpeta int
);

CREATE TABLE Rubro(
	ID_Rubro int NOT NULL PRIMARY KEY,
	Nombre varchar(60) NOT NULL,
	Porcentaje int NOT NULL,
	ID_Grupo int
);

CREATE TABLE Entrega(
	ID_Entrega int NOT NULL PRIMARY KEY,
	Archivo varbinary(MAX) NOT NULL,
	Peso varchar(20) NOT NULL,
	Fecha_Entrega DATE,
	Hora_Entrega TIME,
	EsGrupo BIT,
	ID_Rubro int
);

CREATE TABLE Administrador(
	ID_Admin int NOT NULL PRIMARY KEY,
	Usuario varchar(50) NOT NULL,
	Contraseña varchar(50) NOT NULL,
);


-- Tablas N:M --
CREATE TABLE Curso_Semestre(
	ID_Curso int ,
	ID_Semestre int
	PRIMARY KEY (ID_Curso, ID_Semestre)
);

CREATE TABLE Profesor_Grupo(
	Cedula int,
	ID_Grupo int,
	PRIMARY KEY (Cedula, ID_Grupo)

);


-- FKs de Las tablas --
-- Para las 1:1 y 1:N --

ALTER TABLE Curso
ADD CONSTRAINT FK_Curso_Grupo
FOREIGN KEY (ID_Grupo)
REFERENCES Grupo(ID_Grupo);

ALTER TABLE Grupo
ADD CONSTRAINT FK_Grupo_Estudiante
FOREIGN KEY (Carnet)
REFERENCES Estudiante(Carnet);

ALTER TABLE Noticia
ADD CONSTRAINT FK_Noticia_Profesor
FOREIGN KEY (Cedula)
REFERENCES Profesor(Cedula);

ALTER TABLE Carpeta
ADD CONSTRAINT FK_Carpeta_Grupo
FOREIGN KEY (ID_Grupo)
REFERENCES Grupo(ID_Grupo);

ALTER TABLE Documento
ADD CONSTRAINT FK_Documento_Carpeta
FOREIGN KEY (ID_Carpeta)
REFERENCES Carpeta(ID_Carpeta);

ALTER TABLE Rubro
ADD CONSTRAINT FK_Rubro_Grupo
FOREIGN KEY (ID_Grupo)
REFERENCES Grupo(ID_Grupo);

ALTER TABLE Entrega
ADD CONSTRAINT FK_Entrega_Rubro
FOREIGN KEY (ID_Rubro)
REFERENCES Rubro(ID_Rubro);


-- Para las N:M --
ALTER TABLE Curso_Semestre
ADD CONSTRAINT FK_Curso_Semestre_Curso
FOREIGN KEY (ID_Curso)
REFERENCES Curso(ID_Curso);

ALTER TABLE Curso_Semestre
ADD CONSTRAINT FK_Curso_Semestre_Semestre
FOREIGN KEY (ID_Semestre)
REFERENCES Semestre(ID_Semestre);


ALTER TABLE Profesor_Grupo
ADD CONSTRAINT FK_Profesor_Grupo_Profesor
FOREIGN KEY (Cedula)
REFERENCES Profesor(Cedula);

ALTER TABLE Profesor_Grupo
ADD CONSTRAINT FK_Profesor_Grupo_Grupo
FOREIGN KEY (ID_Grupo)
REFERENCES Grupo(ID_Grupo);