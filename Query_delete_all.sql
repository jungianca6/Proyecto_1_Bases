-- Primero eliminamos todas las constraints FOREIGN KEY (en orden inverso a la creación)

ALTER TABLE Profesor_Grupo DROP CONSTRAINT FK_Profesor_Grupo_Grupo;
ALTER TABLE Profesor_Grupo DROP CONSTRAINT FK_Profesor_Grupo_Profesor;

ALTER TABLE Curso_Semestre DROP CONSTRAINT FK_Curso_Semestre_Semestre;
ALTER TABLE Curso_Semestre DROP CONSTRAINT FK_Curso_Semestre_Curso;

ALTER TABLE Entrega DROP CONSTRAINT FK_Entrega_Rubro;
ALTER TABLE Rubro DROP CONSTRAINT FK_Rubro_Grupo;
ALTER TABLE Documento DROP CONSTRAINT FK_Documento_Carpeta;
ALTER TABLE Carpeta DROP CONSTRAINT FK_Carpeta_Grupo;
ALTER TABLE Noticia DROP CONSTRAINT FK_Noticia_Profesor;
ALTER TABLE Grupo DROP CONSTRAINT FK_Grupo_Estudiante;
ALTER TABLE Curso DROP CONSTRAINT FK_Curso_Grupo;

-- Luego eliminamos todas las tablas (en orden que respete dependencias)

DROP TABLE Profesor_Grupo;
DROP TABLE Curso_Semestre;
DROP TABLE Entrega;
DROP TABLE Rubro;
DROP TABLE Documento;
DROP TABLE Carpeta;
DROP TABLE Noticia;
DROP TABLE Curso;
DROP TABLE Grupo;
DROP TABLE Estudiante;
DROP TABLE Profesor;
DROP TABLE Semestre;
DROP TABLE Administrador;