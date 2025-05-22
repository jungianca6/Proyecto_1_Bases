CREATE DATABASE CEDigital;

----------------- Creaci�n de las Tablas y sus FKs ----------------------

CREATE TABLE Groups(
    group_id int PRIMARY KEY IDENTITY(1,1),
	course_code varchar(20) NOT NULL,
    group_number int NOT NULL,
);

CREATE TABLE Course(
	name varchar(60) NOT NULL,
	course_code varchar(20) NOT NULL PRIMARY KEY,
	credits int NOT NULL,
	career varchar(50) NOT NULL DEFAULT 'Ingenieria en Computadores',
);

CREATE TABLE Semester(
    semester_id int PRIMARY KEY IDENTITY(1,1),
    year int NOT NULL,
    period int NOT NULL
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
    course_code varchar(20),
    publication_date datetime,
    author varchar(100)
);

CREATE TABLE Folder(
	folder_id int PRIMARY KEY IDENTITY(1,1),
	name varchar(50) NOT NULL,
	group_id int
);

CREATE TABLE Document(
	document_id int PRIMARY KEY IDENTITY(1,1),
	filename varchar(60) NOT NULL,
	path varchar(20) NOT NULL,
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
	semester_id int,
	PRIMARY KEY (course_id, semester_id)
);

CREATE TABLE Professor_Group(
	id_number int,
	group_id int,
	PRIMARY KEY (id_number, group_id)

);

CREATE TABLE Student_Group(
	student_id int,
	group_id int,
	PRIMARY KEY (student_id, group_id)
);

-- FKs de Las tablas --
-- Para las 1:1 y 1:N --

ALTER TABLE Groups
ADD CONSTRAINT FK_Group_student
FOREIGN KEY (student_id)
REFERENCES Student(student_id);

ALTER TABLE Groups
ADD CONSTRAINT FK_Grupo_Curso
FOREIGN KEY (course_code)
REFERENCES Course(course_code);

ALTER TABLE Folder
ADD CONSTRAINT FK_Carpeta_Grupo
FOREIGN KEY (group_id)
REFERENCES Groups(group_id);

ALTER TABLE Document
ADD CONSTRAINT FK_Documento_Carpeta
FOREIGN KEY (folder_id)
REFERENCES Folder(folder_id);

ALTER TABLE Grading_item
ADD CONSTRAINT FK_Rubro_Grupo
FOREIGN KEY (group_id)
REFERENCES Groups(group_id);

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
FOREIGN KEY (id_number)
REFERENCES Professor(id_number);

ALTER TABLE Professor_Group
ADD CONSTRAINT FK_Profesor_Grupo_Grupo
FOREIGN KEY (group_id)
REFERENCES Groups(group_id);

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