CREATE DATABASE CEDigital;
USE master;
ALTER DATABASE CEDigital SET SINGLE_USER WITH ROLLBACK AFTER 5 SECONDS;
DROP DATABASE CEDigital;



USE CEDigital;
GO

-- Tablas N:M primero (porque pueden tener claves foráneas a otras)
IF OBJECT_ID('Professor_Group', 'U') IS NOT NULL DROP TABLE Professor_Group;
IF OBJECT_ID('Course_Semester', 'U') IS NOT NULL DROP TABLE Course_Semester;

-- Luego tablas que dependen de otras
IF OBJECT_ID('Submission', 'U') IS NOT NULL DROP TABLE Submission;
IF OBJECT_ID('Grading_item', 'U') IS NOT NULL DROP TABLE Grading_item;
IF OBJECT_ID('Document', 'U') IS NOT NULL DROP TABLE Document;
IF OBJECT_ID('Folder', 'U') IS NOT NULL DROP TABLE Folder;
IF OBJECT_ID('News', 'U') IS NOT NULL DROP TABLE News;

-- Tablas principales
IF OBJECT_ID('Admin', 'U') IS NOT NULL DROP TABLE Admin;
IF OBJECT_ID('Professor', 'U') IS NOT NULL DROP TABLE Professor;
IF OBJECT_ID('Student', 'U') IS NOT NULL DROP TABLE Student;
IF OBJECT_ID('Semester', 'U') IS NOT NULL DROP TABLE Semester;
IF OBJECT_ID('Course', 'U') IS NOT NULL DROP TABLE Course;
IF OBJECT_ID('[Group]', 'U') IS NOT NULL DROP TABLE [Group];  -- usa corchetes por palabra reservada




-- Eliminar claves foráneas de relaciones 1:N
ALTER TABLE Course DROP CONSTRAINT FK_Curso_Grupo;
ALTER TABLE News DROP CONSTRAINT FK_Noticia_Profesor;
ALTER TABLE Folder DROP CONSTRAINT FK_Carpeta_Grupo;
ALTER TABLE Document DROP CONSTRAINT FK_Documento_Carpeta;
ALTER TABLE Grading_item DROP CONSTRAINT FK_Rubro_Grupo;
ALTER TABLE Submission DROP CONSTRAINT FK_Entrega_Rubro;

-- Eliminar claves foráneas de relaciones N:M
ALTER TABLE Course_Semester DROP CONSTRAINT FK_Curso_Semestre_Curso;
ALTER TABLE Course_Semester DROP CONSTRAINT FK_Curso_Semestre_Semestre;
ALTER TABLE Professor_Group DROP CONSTRAINT FK_Profesor_Grupo_Profesor;
ALTER TABLE Professor_Group DROP CONSTRAINT FK_Profesor_Grupo_Grupo;


DELETE FROM Course;

----------------- Creación de las Tablas y sus FKs ----------------------

CREATE TABLE [Group](
    group_id int PRIMARY KEY IDENTITY(1,1),
    group_number int NOT NULL
);

CREATE TABLE Course(
	course_id int PRIMARY KEY IDENTITY(1,1),
	name varchar(60) NOT NULL,
	course_code varchar(20) NOT NULL,
	credits int NOT NULL,
	group_id int
);

CREATE TABLE Semester(
	semester_id int PRIMARY KEY IDENTITY(1,1),
	year int NOT NULL
);

CREATE TABLE Student(
	student_id int NOT NULL PRIMARY KEY
);

CREATE TABLE Professor(
	id_number int NOT NULL PRIMARY KEY
);

CREATE TABLE News(
	news_id int PRIMARY KEY IDENTITY(1,1),
	message varchar(100),
	title varchar(50),
	professor_id int
);

CREATE TABLE Folder(
	folder_id int PRIMARY KEY IDENTITY(1,1),
	name varchar(50) NOT NULL,
	group_id int
);

CREATE TABLE Document(
	document_id int PRIMARY KEY IDENTITY(1,1),
	name varchar(60) NOT NULL,
	path varchar(20) NOT NULL,
	size varchar(50) NOT NULL,
	upload_date DATE NOT NULL,
	uploaded_by_professor BIT,
	folder_id int
);

CREATE TABLE Grading_item(
	grading_item_id int PRIMARY KEY IDENTITY(1,1),
	name varchar(60) NOT NULL,
	percentage int NOT NULL,
	group_id int
);

CREATE TABLE Submission(
	submission_id int PRIMARY KEY IDENTITY(1,1),
	[file] varbinary(MAX) NOT NULL,
	weight varchar(20) NOT NULL,
	delivery_date DATE,
	delivery_time TIME,
	is_group BIT,
	grading_item_id int
);

CREATE TABLE Admin(
	admin_id int PRIMARY KEY IDENTITY(1,1),
	username varchar(50) NOT NULL,
	password varchar(50) NOT NULL,
);


-- Tablas N:M --
CREATE TABLE Course_Semester(
	course_id int ,
	semester_id int
	PRIMARY KEY (course_id, semester_id)
);

CREATE TABLE Professor_Group(
	id_number int,
	group_id int,
	PRIMARY KEY (id_number, group_id)

);


-- FKs de Las tablas --
-- Para las 1:1 y 1:N --

ALTER TABLE Course
ADD CONSTRAINT FK_Curso_Grupo
FOREIGN KEY (group_id)
REFERENCES [Group](group_id);

ALTER TABLE News
ADD CONSTRAINT FK_Noticia_Profesor
FOREIGN KEY (id_number)
REFERENCES Professor(id_number);

ALTER TABLE Folder
ADD CONSTRAINT FK_Carpeta_Grupo
FOREIGN KEY (group_id)
REFERENCES [Group](group_id);

ALTER TABLE Document
ADD CONSTRAINT FK_Documento_Carpeta
FOREIGN KEY (folder_id)
REFERENCES Folder(folder_id);

ALTER TABLE Grading_item
ADD CONSTRAINT FK_Rubro_Grupo
FOREIGN KEY (group_id)
REFERENCES [Group](group_id);

ALTER TABLE Submission
ADD CONSTRAINT FK_Entrega_Rubro
FOREIGN KEY (grading_item_id)
REFERENCES Grading_item(grading_item_id);


-- Para las N:M --
ALTER TABLE Course_Semester
ADD CONSTRAINT FK_Curso_Semestre_Curso
FOREIGN KEY (course_id)
REFERENCES Course(course_id);

ALTER TABLE Course_Semester
ADD CONSTRAINT FK_Curso_Semestre_Semestre
FOREIGN KEY (semester_id)
REFERENCES Semester(semester_id);


ALTER TABLE Professor_Group
ADD CONSTRAINT FK_Profesor_Grupo_Profesor
FOREIGN KEY (Cedula)
REFERENCES Profesor(Cedula);

ALTER TABLE Professor_Group
ADD CONSTRAINT FK_Profesor_Grupo_Grupo
FOREIGN KEY (group_id)
REFERENCES [Group](group_id);

---------------------------- Datos a insertar ---------------------------

INSERT INTO Course (name, course_code, credits) VALUES
('Prueba Avanzada Ingles', 'CI0205', 0),
('Matematica General', 'MA0101', 2),
('Introduccion A La Programacion', 'CE1101', 3),
('Fundamentos De Sistemas Computacionales', 'CE1104', 3),
('Calculo Diferencial E Integral', 'MA1102', 4),
('Matematica Discreta', 'MA1403', 4),
('Laboratorio De Quimica Basica I', 'QU1102', 1),
('Quimica Basica I', 'QU1106', 3),
('Actividad Cultural I', 'SE1100', 0),
('Actividad Deportiva I', 'SE1200', 0),
('Algoritmos Y Estructuras De Datos I', 'CE1103', 4),
('Principios De Modelado En Ingenieria', 'CE1105', 3),
('Comunicacion Tecnica', 'CI1403', 2),
('Introduccion A La Tecnica Cientifica', 'CS1502', 1),
('Fisica General I', 'FI1101', 3),
('Laboratorio Fisica General I', 'FI1201', 1),
('Calculo Y Algebra Lineal', 'MA1103', 4),
('Actividad Cultural-Deportiva', 'SE1400', 0),
('Algoritmos Y Estructuras De Datos II', 'CE2103', 4),
('Ambiente Humano', 'CS2101', 2),
('Circuitos Electricos En Corriente Continua', 'EL2113', 4),
('Centros De Formacion Humanistica', 'FH1000', 0),
('Fisica General II', 'FI1102', 3),
('Laboratorio Fisica General II', 'FI1202', 1),
('Calculo Superior', 'MA2104', 4),
('Paradigmas De Programacion', 'CE1106', 3),
('Laboratorio De Circuitos Electricos', 'CE2201', 1),
('Circuitos Electricos En Corriente Alterna', 'EL2114', 4),
('Elementos Activos', 'EL2207', 4),
('Probabilidad Y Estadistica', 'PI2609', 2),
('Seguridad Y Salud Ocupacional', 'SO4604', 3),
('Fundamentos De Arquitectura De Computadores', 'CE1107', 4),
('Bases De Datos', 'CE3101', 4),
('Taller De Diseno Digital', 'CE3201', 2),
('Ecuaciones Diferenciales', 'MA2105', 4),
('Ingenieria Economica', 'PI5516', 3),
('Compiladores E Interpretes', 'CE1108', 4),
('Circuitos Analogicos', 'CE1109', 4),
('Analisis De Senales Mixtas', 'CE1110', 4),
('Arquitectura De Computadores I', 'CE4301', 4),
('Ingles Especializado Para Ingenieria', 'CI3203', 2),
('Analisis Numerico Para La Ingenieria', 'CE1111', 3),
('Taller De Senales Mixtas', 'CE1112', 3),
('Diseno Y Calidad En Productos Tecnologicos', 'CE1116', 4),
('Arquitectura De Computadores II', 'CE4302', 4),
('Principios De Sistemas Operativos', 'CE4303', 4),
('Desarrollo De Emprendedores', 'AE4208', 4),
('Sistemas Empotrados', 'CE1113', 3),
('Redes De Computadores', 'CE5301', 4),
('Electiva I', 'CE5701', 3),
('Seminario De Etica Para La Ingenieria', 'CS3404', 2),
('Proyecto De Aplicacion De La Ingenieria', 'CE1114', 4),
('Seguridad De La Informacion', 'CE1115', 3),
('Formulacion Y Gestion De Proyectos', 'CE1117', 3),
('Electiva II', 'CE5801', 3),
('Electiva III', 'CE5901', 3),
('Seminario De Estudios Costarricenses', 'CS4402', 2),
('Trabajo Final De Graduacion', 'CE5601', 12);



SELECT 
    f.name AS foreign_key_name,
    OBJECT_NAME(f.parent_object_id) AS table_name
FROM sys.foreign_keys AS f
ORDER BY table_name;



SELECT 
    name AS table_name
FROM 
    sys.tables
ORDER BY 
    name;



SELECT 
    fk.name AS foreign_key_name,
    OBJECT_NAME(fk.parent_object_id) AS referencing_table,
    pc.name AS referencing_column,
    OBJECT_NAME(fk.referenced_object_id) AS referenced_table,
    rc.name AS referenced_column
FROM 
    sys.foreign_keys AS fk
JOIN 
    sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
JOIN 
    sys.columns AS pc ON fkc.parent_object_id = pc.object_id AND fkc.parent_column_id = pc.column_id
JOIN 
    sys.columns AS rc ON fkc.referenced_object_id = rc.object_id AND fkc.referenced_column_id = rc.column_id
ORDER BY 
    referencing_table, foreign_key_name;


SELECT * FROM Course;



SELECT * 
FROM Course
WHERE name LIKE '%Algoritmos%';



SELECT * 
FROM Course
ORDER BY credits DESC;



SELECT * 
FROM Course
WHERE credits = 0;



SELECT credits, COUNT(*) AS cantidad_de_cursos
FROM Course
GROUP BY credits
ORDER BY credits;



SELECT * 
FROM Course
WHERE course_code = 'CE1101';



SELECT name, course_code 
FROM Course;
