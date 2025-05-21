import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import axios from "axios";
import styles from "./AdminPg.module.css"; 
import { Button, Card, Form } from 'react-bootstrap';
import * as XLSX from "xlsx";

function AdminPg() {
  //Estados para cursos
  const [codigoCurso, setCodigoCurso] = useState("");
  const [nombreCurso, setNombreCurso] = useState("");
  const [creditosCurso, setCreditosCurso] = useState("");
  const [carreraCurso, setCarreraCurso] = useState("");
  const [grupoCurso, setGrupoCurso] = useState("");
  const [cedulasProfesores, setCedulasProfesores] = useState("");
  const [cursoVisualizado, setCursoVisualizado] = useState(null);

  //Agregar curso

  const [codigoAgregarGrupo, setCodigoAgregarGrupo] = useState("");

    // Estados para semestre
  const [aSemestre, setASemestre] = useState("");
  const [periodoSemestre, setPeriodoSemestre] = useState("");

  // Estados para agregar curso a semestre
  const [codigoCursoSemestre, setCodigoCursoSemestre] = useState("");
  const [aSemestreCurso, setASemestreCurso] = useState("");
  const [periodoSemestreCurso, setPeriodoSemestreCurso] = useState("");

  // Estados para agregar estudiantes a grupo
  const [carnetsEstudiantes, setCarnetsEstudiantes] = useState("");
  const [numeroGrupoEstudiantes, setNumeroGrupoEstudiantes] = useState("");
  const [codigoCursoEstudiantes, setCodigoCursoEstudiantes] = useState("");

  //Estado para agregar secciones default de secciones de documentos

  const [codigoDocuSecciones, setCodigoDocuSecciones] = useState("");

  //Estado para agregar rubros default en las evaluaciones de curso

  const [codigoCursoRubrosDefault, setCodigoCursoRubrosDefault] = useState("");

  const handleCrearCurso = async (e) => {
  e.preventDefault();

  // Convertir "1,2" → [1, 2]
  const listaProfesores = cedulasProfesores
    .split(',')
    .map(s => parseInt(s.trim()))
    .filter(n => !isNaN(n));

  const listaGrupos = grupoCurso
    .split(',')
    .map(g => parseInt(g.trim()))
    .filter(n => !isNaN(n));

  const requestData = {
      course_code: codigoCurso,
      name: nombreCurso,
      credits: creditosCurso,
      career: carreraCurso,
      group_ids: [],               
      professors_ids: listaProfesores
  };

  try {
    const response = await axios.post("https://localhost:7199/Course/create_course", requestData);

    if (response.data.status === "OK") {
      alert(response.data.message);
    } else {
      alert("Error al crear curso: " + (response.data.message || "Error desconocido"));
    }
  } catch (error) {
    console.error(error);
    alert("Error al conectar con el servidor");
  }
};


  const handleVisualizarCursos = async (e) => {
  e.preventDefault();

  const requestData = {
    course_code: codigoCurso,
    name: "",
    credits: null,
    career: "",
    group_ids: [],
    professors_ids: []
  };

  try {
    const response = await axios.post("https://localhost:7199/Course/view_course", requestData);

    if (response.data.status === "OK") {
      const curso = response.data.message;
      console.log("Curso encontrado:", curso);
      setCursoVisualizado(curso); // 👈 Aquí guardas los datos
    } else {
      alert("Error al visualizar curso: " + (response.data.message || "Error desconocido"));
      setCursoVisualizado(null); // Limpias si hubo error
    }
  } catch (error) {
    console.error(error);
    alert("Error al conectar con el servidor");
    setCursoVisualizado(null); // Limpias si hubo error
  }
};

  const handleDeshabilitarCurso = async (e) => {
  e.preventDefault();

  const requestData = {
      course_code: codigoCurso
  };

  try {
    const response = await axios.post("https://localhost:7199/Course/disable_course", requestData);

    if (response.data.status === "OK") {
      alert(response.data.message);
    } else {
      alert("Error al deshabilitar curso: " + (response.data.message || "Error desconocido"));
    }
  } catch (error) {
    console.error(error);
    alert("Error al conectar con el servidor");
  }
};

 const handleAgregarGrupo = async () => {
    const requestData = {
      course_code: codigoAgregarGrupo // Ajusta la clave si tu backend usa otro nombre
    };

    try {
      const response = await axios.post("https://localhost:7199/Course/add_group", requestData);

      if (response.data.status === "OK") {
        alert("Grupo agregado correctamente.");
        console.log("Respuesta:", response.data.message);
      } else {
        alert("Error al agregar grupo: " + (response.data.message || "Error desconocido"));
      }
    } catch (error) {
      console.error(error);
      alert("Error al conectar con el servidor.");
    }
  };


  const handleInicializarSemestre = async (e) => {
  e.preventDefault();

  const requestData = {
      year: parseInt(aSemestre),
      period: parseInt(periodoSemestre)
  };

  try {
    const response = await axios.post("https://localhost:7199/Semester/initialize_semester", requestData);

    if (response.data.status === "OK") {
      alert("Semestre inicializado correctamente.");
      console.log("Respuesta:", response.data.message);
    } else {
      alert("Error al inicializar semestre: " + (response.data.message || "Error desconocido"));
    }
  } catch (error) {
    console.error(error);
    alert("Error al conectar con el servidor.");
  }
};

  const handleAgregarCursoASemestre = async (e) => {
  e.preventDefault();

  const requestData = {
      course_code: codigoCursoSemestre,
      year: parseInt(aSemestreCurso),
      period: parseInt(periodoSemestreCurso)
  };

  try {
    const response = await axios.post("https://localhost:7199/Semester/add_course_to_semester", requestData);

    if (response.data.status === "OK") {
      alert("Curso agregado al semestre correctamente.");
      console.log("Respuesta:", response.data.message);
    } else {
      alert("Error al agregar curso al semestre: " + (response.data.message || "Error desconocido"));
    }
  } catch (error) {
    console.error(error);
    alert("Error al conectar con el servidor.");
  }
};

  const handleAgregarEstudiantesAGrupo = async (e) => {
  e.preventDefault();

  const listaCarnets = carnetsEstudiantes
    .split(',')
    .map(c => c.trim())
    .filter(c => c.length > 0);

  for (const carnet of listaCarnets) {
    const requestData = {
        student_card: carnet,
        group_number: parseInt(numeroGrupoEstudiantes),
        course_code: codigoCursoEstudiantes
    };

    try {
      const response = await axios.post("https://localhost:7199/Student/add_student_to_group", requestData);

      if (response.data.status === "OK") {
        console.log(`Estudiante ${carnet} agregado correctamente.`);
      } else {
        console.warn(`Error al agregar estudiante ${carnet}:`, response.data.message);
      }
    } catch (error) {
      console.error(`Error al conectar con el servidor para el estudiante ${carnet}:`, error);
    }
  }

  alert("Proceso de adición de estudiantes finalizado.");
};

  const handleAgregarDocuSeccionesPorDefecto = async (e) => {
  e.preventDefault();

  const requestData = {
      course_code: codigoDocuSecciones,
      sections: ["Presentaciones", "Quices", "Exámenes", "Proyectos"]
  };

  try {
    const response = await axios.post("https://localhost:7199/Course/add_default_document_sections", requestData);

    if (response.data.status === "OK") {
      console.log("Secciones agregadas correctamente.");
    } else {
      console.warn("Error al agregar secciones:", response.data.message);
    }
  } catch (error) {
    console.error("Error al conectar con el servidor:", error);
  }
};

const handleAgregarRubrosDefaultACurso = async (e) => {
  e.preventDefault();

  const requestData = {
      course_code: codigoCursoRubrosDefault,
      sections: ["Quices", "Exámenes", "Proyectos"],
      percentages: [30.0, 30.0, 40.0]
  };

  try {
    const response = await axios.post("https://localhost:7199/Course/add_default_grades", requestData);

    if (response.data.status === "OK") {
      console.log("Rubros por defecto agregados correctamente.");
    } else {
      console.warn("Error al agregar rubros:", response.data.message);
    }
  } catch (error) {
    console.error("Error al conectar con el servidor:", error);
  }
};

const handleExcelUpload = (e) => {
  const file = e.target.files[0];
  if (!file) return;

  const reader = new FileReader();
  reader.onload = (evt) => {
    const data = evt.target.result;
    const workbook = XLSX.read(data, { type: "binary" });
    const sheet = workbook.Sheets[workbook.SheetNames[0]];
    const json = XLSX.utils.sheet_to_json(sheet);

    // Llama a la función que envía línea por línea
    enviarDatosFilaPorFila(json);
  };
  reader.readAsBinaryString(file);
};

const enviarDatosFilaPorFila = async (filas) => {
  for (const row of filas) {
    // Construyes el objeto para enviar con la info de esta fila
    const dataSemestre = {
      year: parseInt(row["Año"]),
      period: parseInt(row["Periodo"])
    };

    const dataCurso = {
      course_code: row["Código Curso"],
      year: parseInt(row["Año"]),
      period: parseInt(row["Periodo"])
    };

    const listaCarnets = row["Carnets Estudiantes"]
      ? row["Carnets Estudiantes"].split(",").map(c => c.trim()).filter(c => c.length > 0)
      : [];

    const dataSecciones = {
      course_code: row["Código Curso"],
      sections: ["Presentaciones", "Quices", "Exámenes", "Proyectos"]
    };

    const dataRubros = {
      course_code: row["Código Curso"],
      sections: ["Quices", "Exámenes", "Proyectos"],
      percentages: [30.0, 30.0, 40.0]
    };

    try {
      // Inicializar semestre (si quieres hacerlo por cada fila)
      await axios.post("https://localhost:7199/Semester/initialize_semester", {
        data_input_initialize_semester: dataSemestre
      });

      // Agregar curso
      await axios.post("https://localhost:7199/Semester/add_course_to_semester", {
        data_input_add_course_to_semester: dataCurso
      });

      // Agregar estudiantes al grupo, uno por uno
      for (const carnet of listaCarnets) {
        await axios.post("https://localhost:7199/Student/add_student_to_group", {
          data_input_add_student_to_group: {
            student_card: carnet,
            group_number: parseInt(row["Número Grupo"]),
            course_code: row["Código Curso"]
          }
        });
      }

      // Agregar secciones por defecto
      await axios.post("https://localhost:7199/Course/add_default_document_sections", {
        data_input_add_default_document_sections: dataSecciones
      });

      // Agregar rubros por defecto
      await axios.post("https://localhost:7199/Course/add_default_grades", {
        data_input_add_default_grades: dataRubros
      });

      console.log(`Fila con curso ${row["Código Curso"]} procesada correctamente.`);
    } catch (error) {
      console.error(`Error al procesar fila con curso ${row["Código Curso"]}:`, error);
    }
  }
  alert("Proceso completado.");
};


  return (
    <div className={styles.container}>
      <h1 className="text-center mb-2">CE Digital</h1>
      <h2 className="text-center mb-4">Administrador</h2>
      <h3 className={styles.title}>Gestión de Cursos</h3>
      <form>
      <label className={styles.label}>Código del Curso:</label>
      <input
        type="text"
        className={styles.input}
        value={codigoCurso}
        onChange={(e) => setCodigoCurso(e.target.value)}
      />

      <label className={styles.label}>Nombre del Curso:</label>
      <input
        type="text"
        className={styles.input}
        value={nombreCurso}
        onChange={(e) => setNombreCurso(e.target.value)}
      />

      <label className={styles.label}>Cantidad de Créditos:</label>
      <input
        type="number"
        className={styles.input}
        value={creditosCurso}
        onChange={(e) => setCreditosCurso(e.target.value)}
      />

      <label className={styles.label}>Carrera:</label>
      <input
        type="text"
        className={styles.input}
        value={carreraCurso}
        onChange={(e) => setCarreraCurso(e.target.value)}
      />

      <label className={styles.label}>Cédulas de Profesores Asociados (separar con comas si son varios):</label>
      <input
        type="text"
        className={styles.input}
        value={cedulasProfesores}
        onChange={(e) => {
          const valorFiltrado = e.target.value.replace(/[^0-9,]/g, ""); // solo números y comas
          setCedulasProfesores(valorFiltrado);
        }}
      />

      <div className={styles.buttonGroup}>
        <button
          type="button"
          className={`${styles.btn} ${styles.success}`}
          onClick={handleCrearCurso}
        >
          Crear Curso
        </button>

        <button
          type="button"
          className={`${styles.btn} ${styles.primary}`}
          onClick={handleVisualizarCursos}
        >
          Visualizar Cursos
        </button>

        <button
          type="button"
          className={`${styles.btn} ${styles.danger}`}
          onClick={handleDeshabilitarCurso}
        >
          Deshabilitar Curso
        </button>
      </div>
      <p style={{ marginTop: "10px", color: "#555" }}>
      <strong>Nota:</strong> Para visualizar o eliminar un curso, solo es necesario ingresar el <strong>código del curso</strong>.
      </p>

      {cursoVisualizado && (
        <div className={styles.resultBox} style={{ marginTop: "2rem", background: "#f9f9f9", padding: "1rem", border: "1px solid #ccc", borderRadius: "8px" }}>
          <h4 style={{ marginBottom: "1rem" }}>📘 Información del Curso</h4>
          <p><strong>Código:</strong> {cursoVisualizado.course_code}</p>
          <p><strong>Nombre:</strong> {cursoVisualizado.name}</p>
          <p><strong>Créditos:</strong> {cursoVisualizado.credits}</p>
          <p><strong>Carrera:</strong> {cursoVisualizado.career}</p>
          <p><strong>Grupos:</strong> {cursoVisualizado.group_ids}</p>
          <p><strong>Profesores:</strong> {cursoVisualizado.professors_ids.join(", ")}</p>
        </div>
      )}

      <h3 className={styles.title} style={{ marginTop: "3rem" }}>
          Agregar grupo a un curso
        </h3>
        <form>
          <label className={styles.label}>Código del Curso:</label>
          <input
            type="text"
            className={styles.input}
            value={codigoAgregarGrupo}
            onChange={(e) => setCodigoAgregarGrupo(e.target.value)}
          />
          <div className={styles.buttonGroup}>
            <button
              type="button"
              className={`${styles.btn} ${styles.success}`}
              onClick={handleAgregarGrupo}
            >
              Agregar Grupo
            </button>
          </div>
        </form>


      <h2 className={styles.title} style={{ marginTop: "3rem" }}>
        Inicializar Semestre
      </h2>

    </form>
        <h3 className={styles.title} style={{ marginTop: "3rem" }}>
        Crear Semestre
      </h3>
      <form>
        <label className={styles.label}>Año:</label>
        <input
          type="number"
          className={styles.input}
          value={aSemestre}
          onChange={(e) => setASemestre(e.target.value)}
        />

        <label className={styles.label}>Periodo:</label>
        <select
          className={styles.input}
          value={periodoSemestre}
          onChange={(e) => setPeriodoSemestre(e.target.value)}
        >
          <option value="">Seleccione</option>
          <option value="1">1</option>
          <option value="2">2</option>
          <option value="V">V</option>
        </select>

        <div className={styles.buttonGroup}>
          <button
            type="button"
            className={`${styles.btn} ${styles.success}`}
            onClick={handleInicializarSemestre}
          >
            Inicializar Semestre
          </button>
        </div>
      </form>

       <h3 className={styles.title} style={{ marginTop: "3rem" }}>
        Agregar Curso a Semestre
      </h3>
      <form>
        <label className={styles.label}>Código de Curso:</label>
        <input
          type="text"
          className={styles.input}
          value={codigoCursoSemestre}
          onChange={(e) => setCodigoCursoSemestre(e.target.value)}
        />

        <label className={styles.label}>Año del semestre:</label>
        <input
          type="number"
          className={styles.input}
          value={aSemestreCurso}
          onChange={(e) => setASemestreCurso(e.target.value)}
        />

        <label className={styles.label}>Periodo del semestre:</label>
        <select
          className={styles.input}
          value={periodoSemestreCurso}
          onChange={(e) => setPeriodoSemestreCurso(e.target.value)}
        >
          <option value="">Seleccione</option>
          <option value="1">1</option>
          <option value="2">2</option>
          <option value="V">V</option>
        </select>

        <div className={styles.buttonGroup}>
          <button
            type="button"
            className={`${styles.btn} ${styles.success}`}
            onClick={handleAgregarCursoASemestre}
          >
            Agregar Curso a Semestre
          </button>
        </div>
      </form>

        <h3 className={styles.title} style={{ marginTop: "3rem" }}>
        Agregar Estudiantes a Grupo
      </h3>
      <form>
        <label className={styles.label}>Carnets de Estudiantes (separar con comas):</label>
        <input
          type="text"
          className={styles.input}
          value={carnetsEstudiantes}
          onChange={(e) => {
            const valorFiltrado = e.target.value.replace(/[^0-9,]/g, "");
            setCarnetsEstudiantes(valorFiltrado);
          }}
        />
        <label className={styles.label}>Número de Grupo:</label>
        <input
          type="text"
          className={styles.input}
          value={numeroGrupoEstudiantes}
          onChange={(e) => setNumeroGrupoEstudiantes(e.target.value)}
        />
        <label className={styles.label}>Código del Curso:</label>
        <input
          type="text"
          className={styles.input}
          value={codigoCursoEstudiantes}
          onChange={(e) => setCodigoCursoEstudiantes(e.target.value)}
          />
          <div className={styles.buttonGroup}>
          <button
          type="button"
          className={`${styles.btn} ${styles.success}`}
          onClick={handleAgregarEstudiantesAGrupo}
          >
          Agregar Estudiantes a Grupo
          </button>
          </div>
          </form>

          <h3 className={styles.title} style={{ marginTop: "3rem" }}>
          Agregar Secciones de Documentos por Defecto a Curso
        </h3>
        <form>
          <label className={styles.label}>Código del Curso:</label>
          <input
            type="text"
            className={styles.input}
            value={codigoDocuSecciones}
            onChange={(e) => setCodigoDocuSecciones(e.target.value)}
          />
          <div className={styles.buttonGroup}>
            <button
              type="button"
              className={`${styles.btn} ${styles.success}`}
              onClick={handleAgregarDocuSeccionesPorDefecto}
            >
              Agregar Secciones
            </button>
          </div>
        </form>

        <h3 className={styles.title} style={{ marginTop: "3rem" }}>
          Agregar Rubros Default a Curso
        </h3>
        <form>
          <label className={styles.label}>Código del Curso:</label>
          <input
            type="text"
            className={styles.input}
            value={codigoCursoRubrosDefault}
            onChange={(e) => setCodigoCursoRubrosDefault(e.target.value)}
          />

          <div className={styles.buttonGroup}>
            <button
              type="button"
              className={`${styles.btn} ${styles.success}`}
              onClick={handleAgregarRubrosDefaultACurso}
            >
              Agregar Rubros Default
            </button>
          </div>
        </form>

        <h3 className={styles.title} style={{ marginTop: "3rem" }}>
          Cargar Excel para Inicializar Semestre
        </h3>
        <input
          type="file"
          accept=".xlsx, .xls"
          onChange={handleExcelUpload}
          className={styles.input}
        />

    </div>
  );
}

export default AdminPg;
