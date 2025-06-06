CREATE DATABASE CEDigital
USE CEDigital

----------------- Creaci�n de las Tablas y sus FKs ----------------------
CREATE TABLE Student(
    student_id INT NOT NULL PRIMARY KEY
);

CREATE TABLE Professor(
    id_number INT NOT NULL PRIMARY KEY
);

CREATE TABLE Admin(
    admin_id INT PRIMARY KEY IDENTITY(1,1),
    username VARCHAR(50) NOT NULL,
    password VARCHAR(50) NOT NULL
);

CREATE TABLE Semester(
    semester_id INT PRIMARY KEY IDENTITY(1,1),
    year INT NOT NULL,
    period INT NOT NULL
);

CREATE TABLE Course(
    name VARCHAR(60) NOT NULL,
    course_code VARCHAR(20) NOT NULL PRIMARY KEY,
    credits INT NOT NULL,
    career VARCHAR(50) NOT NULL DEFAULT 'Ingenieria en Computadores'
);

CREATE TABLE Groups(
    group_id INT PRIMARY KEY IDENTITY(1,1),
    course_code VARCHAR(20) NOT NULL,
    group_number INT NOT NULL,
    student_id INT,
    FOREIGN KEY (course_code) REFERENCES Course(course_code),
    FOREIGN KEY (student_id) REFERENCES Student(student_id)
);

CREATE TABLE News(
    news_id INT PRIMARY KEY IDENTITY(1,1),
    message VARCHAR(100),
    title VARCHAR(50),
    course_code VARCHAR(20),
    publication_date DATETIME,
    author VARCHAR(100),
    FOREIGN KEY (course_code) REFERENCES Course(course_code)
);

CREATE TABLE Folder(
    folder_id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(50) NOT NULL,
    group_id INT,
    FOREIGN KEY (group_id) REFERENCES Groups(group_id)
);

CREATE TABLE Document(
    document_id INT PRIMARY KEY IDENTITY(1,1),
    filename VARCHAR(60) NOT NULL,
    path VARCHAR(200) NOT NULL,
    upload_date DATE NOT NULL,
    uploaded_by_professor BIT,
    folder_id INT,
    FOREIGN KEY (folder_id) REFERENCES Folder(folder_id)
);

CREATE TABLE Grading_item(
    grading_item_id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(60) NOT NULL,
    percentage INT NOT NULL,
    group_id INT,
    FOREIGN KEY (group_id) REFERENCES Groups(group_id)
);

CREATE TABLE Evaluation (
    evaluation_id INT PRIMARY KEY IDENTITY(1,1),
    evaluation_title VARCHAR(100),
    professor_filename VARCHAR(50),
    data_base_path_professor VARCHAR(200),
    evaluation_date DATETIME,
    is_group BIT,
    grading_item_id INT NOT NULL,
    FOREIGN KEY (grading_item_id) REFERENCES Grading_item(grading_item_id)
);

CREATE TABLE Evaluation_Student (
    evaluation_id INT NOT NULL,
    student_id INT NOT NULL,
    evaluation_filename VARCHAR(50),
    data_base_path_evalution VARCHAR(200),
    grade FLOAT,
    feedback VARCHAR(MAX),
    is_public BIT,
    PRIMARY KEY (evaluation_id, student_id),
    FOREIGN KEY (evaluation_id) REFERENCES Evaluation(evaluation_id),
    FOREIGN KEY (student_id) REFERENCES Student(student_id)
);


-- Tablas N:M --

CREATE TABLE Course_Semester(
    course_code VARCHAR(20),
    semester_id INT,
    PRIMARY KEY (course_code, semester_id),
    FOREIGN KEY (course_code) REFERENCES Course(course_code),
    FOREIGN KEY (semester_id) REFERENCES Semester(semester_id)
);

CREATE TABLE Professor_Group(
    id_number INT,
    group_id INT,
    PRIMARY KEY (id_number, group_id),
    FOREIGN KEY (id_number) REFERENCES Professor(id_number),
    FOREIGN KEY (group_id) REFERENCES Groups(group_id)
);


CREATE TABLE Student_Group(
    student_id INT,
    group_id INT,
    course_code VARCHAR(20),
    PRIMARY KEY (student_id, group_id),
    FOREIGN KEY (student_id) REFERENCES Student(student_id),
    FOREIGN KEY (group_id) REFERENCES Groups(group_id),
    FOREIGN KEY (course_code) REFERENCES Course(course_code)
);


CREATE TABLE Group_Section_Percentage (
    id INT IDENTITY(1,1) PRIMARY KEY,
    group_id INT NOT NULL,
    section_name VARCHAR(100) NOT NULL,
    percentage FLOAT NOT NULL,
    FOREIGN KEY (group_id) REFERENCES Groups(group_id)
);
---------------------------- Datos a insertar ---------------------------

-- Courses
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
('Trabajo Final De Graduacion', 'CE5601', 12),
('Cálculo Diferencial', 'MA1101', 4),
('Física General I', 'FI1402', 4),
('Arquitectura de Computadores', 'CE3102', 3);

-- Semesters
INSERT INTO Semester (year, period) VALUES (2024, 1), (2024, 2), (2025, 1);

-- Insertar estudiantes
INSERT INTO Student (student_id) VALUES (2023065165);
INSERT INTO Student (student_id) VALUES (2023493175);
INSERT INTO Student (student_id) VALUES (2024983216);
INSERT INTO Student (student_id) VALUES (2020697822);
INSERT INTO Student (student_id) VALUES (2016698746);

-- Insertar profesores
INSERT INTO Professor (id_number) VALUES (301849792);
INSERT INTO Professor (id_number) VALUES (194875095);
INSERT INTO Professor (id_number) VALUES (406284097);
INSERT INTO Professor (id_number) VALUES (256894765);
INSERT INTO Professor (id_number) VALUES (100000001);

-- Groups
INSERT INTO Groups (course_code, group_number) VALUES 
('CE2201', 1),
('CE2201', 2),
('CE3102', 1);



INSERT INTO Groups (course_code, group_number) VALUES 
('MA1101', 1),
('FI1402', 2),
('CE3102', 3);

-- Student_Group
INSERT INTO Student_Group (student_id, group_id, course_code) VALUES 
(2023065165, 1, 'MA1101'),
(2023493175, 1, 'MA1101'),
(2024983216, 2, 'FI1402'),
(2016698746, 3, 'CE3102'),
(2020697822, 3, 'CE3102')
(2020697822, 1, 'CE2201');

-- Professor_Group
INSERT INTO Professor_Group (id_number, group_id) VALUES 
(301849792, 1),
(406284097, 2),
(256894765, 3);

-- Course_Semester
INSERT INTO Course_Semester (course_code, semester_id) VALUES 
('MA1101', 1),
('FI1402', 2),
('CE3102', 3);

-- Folders
INSERT INTO Folder (name, group_id) VALUES 
('Material Semana 1', 1),
('Tareas', 2),
('Proyectos', 3);

-- Documents
INSERT INTO Document (filename, path, upload_date, uploaded_by_professor, folder_id) VALUES 
('Guía_Integrales.pdf', '/docs/mat1', '2024-03-01', 1, 1),
('Tarea1_Fisica.docx', '/docs/fi2', '2024-04-10', 0, 2),
('ProyectoFinal.zip', '/docs/ce3', '2024-05-15', 1, 3);

-- Grading Items
INSERT INTO Grading_item (name, percentage, group_id) VALUES 
('Examen 1', 30, 1),
('Tarea 1', 10, 2),
('Proyecto Final', 40, 3);

-- Insert evaluations


INSERT INTO Evaluation (
    evaluation_title,
    professor_filename,
    data_base_path_professor,
    evaluation_date,
    is_group,
    grading_item_id
) VALUES
('Examen Parcial 1', 'prof_ma1101.pdf', '/professors/ma1101', '2024-04-15 10:00:00', 1, 1),
('Tarea 1 Física', 'prof_fi1402.pdf', '/professors/fi1402', '2024-04-20 23:59:59', 0, 2),
('Proyecto Final Ingeniería', 'prof_ce3102.pdf', '/professors/ce3102', '2024-05-30 17:00:00', 1, 3),
('Laboratorio 2', 'prof_ma1101_lab.pdf', '/professors/ma1101', '2024-04-25 15:00:00', 0, 1),
('Examen 1 - Primera Evaluación Individual', 'string', 'string', '2024-04-18 12:00:00.000', 0, 1),
('Quiz 1', 'prof_fi1402.pdf', '/professors/fi1402', '2024-04-18 12:00:00', 0, 2);

INSERT INTO Evaluation_Student (
    evaluation_id,
    student_id,
    evaluation_filename,
    data_base_path_evalution,
    grade,
    feedback,
    is_public
) VALUES
(1, 2020697822, 'examen1_ma1101.pdf', '/evaluations/ma1101/examen1', 85.0, 'Buen desempeño general, algunas preguntas con dificultad.', 1),
(2, 2023493175, 'tarea1_fi1402.docx', '/evaluations/fi1402/tarea1', 92.5, 'Muy buenos resultados en general.', 1),
(2, 2020697822, 'tarea1_fi1402.docx', '/evaluations/fi1402/tarea1', 92.5, 'Muy buenos resultados en general.', 1),
(3, 2023065165, 'proyecto_final_ce3102.zip', '/evaluations/ce3102/proyecto_final', 88.0, 'Proyecto bien realizado, aunque con detalles a mejorar.', 1),
(4, 2020697822, 'lab2_ma1101.pdf', '/evaluations/ma1101/lab2', 90.0, 'Muy buen manejo de los conceptos prácticos.', 1),
(5, 2023493175, 'quiz1_fi1402.pdf', '/evaluations/fi1402/quiz1', 88.5, 'Buen desempeño en preguntas rápidas.', 1);


-- Eliminar todos los registros de Evaluation_Student (tabla dependiente)
DELETE FROM Evaluation_Student;

-- Eliminar todos los registros de Evaluation (tabla principal)
DELETE FROM Evaluation;


-- Admins
INSERT INTO Admin (username, password) VALUES 
('admin01', 'admin123'),
('admin02', 'admin456');

-- News
INSERT INTO News (message, title, course_code, publication_date, author) VALUES 
('Se publicaron los resultados del Examen 1.', 'Resultados Examen', 'MA1101', '2024-04-01', 'Prof. Pérez'),
('Recuerden entregar la tarea antes del viernes.', 'Recordatorio', 'FI1402', '2024-04-05', 'Prof. Gómez'),
('Subido el documento del proyecto final.', 'Proyecto Final', 'CE3102', '2024-05-20', 'Prof. Ramírez');



SELECT c.course_code, c.name AS course_name, g.group_number
FROM Student_Group sg
JOIN Groups g ON sg.group_id = g.group_id
JOIN Course c ON g.course_code = c.course_code
WHERE sg.student_id = 20231001

SELECT s.*
FROM Student s
JOIN Student_Group sg ON s.student_id = sg.student_id
WHERE sg.student_id = 20231001 AND sg.group_id = 1;


SELECT 
    gi.name AS rubric_name,
    e.evaluation_title,
    es.grade,
    es.feedback,
    es.is_public,
    e.evaluation_date,
    es.data_base_path_evalution,
    e.data_base_path_professor,
    es.evaluation_filename,
    e.professor_filename
FROM Evaluation_Student es
JOIN Evaluation e ON es.evaluation_id = e.evaluation_id
JOIN Grading_item gi ON e.grading_item_id = gi.grading_item_id
JOIN Groups g ON gi.group_id = g.group_id
JOIN Student s ON es.student_id = s.student_id
WHERE s.student_id = 2020697822
  AND g.course_code = 'CE3102'
  AND g.group_number = 3;


-- 1. Visualizar todos los ítems de calificación para un curso y grupo específico
SELECT gi.grading_item_id, gi.name, gi.percentage, gi.group_id, g.course_code, g.group_number
FROM Grading_item gi
INNER JOIN Groups g ON gi.group_id = g.group_id
WHERE g.course_code = 'MA1101' AND g.group_number = 1;

-- 2. Visualizar un ítem específico de calificación por su ID para un curso y grupo
SELECT gi.grading_item_id, gi.name, gi.percentage, gi.group_id, g.course_code, g.group_number
FROM Grading_item gi
INNER JOIN Groups g ON gi.group_id = g.group_id
WHERE gi.grading_item_id = 5 AND g.course_code = 'CS101' AND g.group_number = 1;

-- 3. Visualizar ítems por nombre para confirmar antes de eliminar
SELECT gi.grading_item_id, gi.name, gi.percentage, gi.group_id
FROM Grading_item gi
INNER JOIN Groups g ON gi.group_id = g.group_id
WHERE g.course_code = 'MA1101' AND g.group_number = 1
  AND gi.name IN ('Quiz 1 - Updated', 'Midterm Exam');



  -- Estudiante
INSERT INTO Student (student_id) VALUES (2020697822);

-- Grupo asociado al curso 'CE3102' con número de grupo 3
INSERT INTO Groups (course_code, group_number) VALUES ( 'CE3102', 3);

-- Sin especificar el grading_item_id
INSERT INTO Grading_item (name, percentage, group_id) 
VALUES ('Proyecto Final', 40, 7);

-- Evaluación asociada a ese ítem
INSERT INTO Evaluation (
    evaluation_title,
    professor_filename,
    data_base_path_professor,
    evaluation_date,
    is_group,
    grading_item_id
) VALUES (
    'Proyecto Final Ingeniería',
    'prof_ce3102.pdf',
    '/professors/ce3102',
    '2024-05-30 17:00:00',
    1,
    5
);

-- Evaluación realizada por el estudiante
INSERT INTO Evaluation_Student (
    evaluation_id,
    student_id,
    evaluation_filename,
    data_base_path_evalution,
    grade,
    feedback,
    is_public
) VALUES (
    7,
    2020697822,
    'proyecto_final_ce3102.zip',
    '/evaluations/ce3102/proyecto_final',
    88.0,
    'Proyecto bien realizado, aunque con detalles a mejorar.',
    1
);