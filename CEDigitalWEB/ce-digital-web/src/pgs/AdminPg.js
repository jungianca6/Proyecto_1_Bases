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
    data_input_admin_create_course: {
      course_code: codigoCurso,
      name: nombreCurso,
      credits: parseInt(creditosCurso),
      career: carreraCurso,
      group_ids: listaGrupos,               
      professors_ids: listaProfesores
    }
  };

  try {
    const response = await axios.post("https://localhost:7190/Admin/CrearCurso", requestData);

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
    data_input_admin_view_course: {
      course_code: codigoCurso,
      name: null,
      credits: null,
      career: null,
      group_ids: null,
      professors_ids: null
    }
  };

  try {
    const response = await axios.post("https://localhost:7190/Admin/VisualizarCurso", requestData);

    if (response.data.status === "OK") {
      const curso = response.data.message.data_output_admin_view_course;
      console.log("Curso encontrado:", curso);
      alert(`Curso: ${curso.name}\nCódigo: ${curso.course_code}\nCréditos: ${curso.credits}\nCarrera: ${curso.career}\nGrupo: ${curso.group_ids}\nSemestres: ${curso.professors_ids.join(", ")}`);
    } else {
      alert("Error al visualizar curso: " + (response.data.message || "Error desconocido"));
    }
  } catch (error) {
    console.error(error);
    alert("Error al conectar con el servidor");
  }
};

  const handleDeshabilitarCurso = async (e) => {
  e.preventDefault();

  const requestData = {
    data_input_admin_disable_course: {
      course_code: codigoCurso
    }
  };

  try {
    const response = await axios.post("https://localhost:7190/Admin/DeshabilitarCurso", requestData);

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

  const handleInicializarSemestre = async (e) => {
  e.preventDefault();

  const requestData = {
    data_input_initialize_semester: {
      year: parseInt(aSemestre),
      period: parseInt(periodoSemestre)
    }
  };

  try {
    const response = await axios.post("https://localhost:7190/Admin/InicializarSemestre", requestData);

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
    data_input_add_course_to_semester: {
      course_code: codigoCursoSemestre,
      year: parseInt(aSemestreCurso),
      period: parseInt(periodoSemestreCurso)
    }
  };

  try {
    const response = await axios.post("https://localhost:7190/Admin/AgregarCursoASemestre", requestData);

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
      data_input_add_student_to_group: {
        student_card: carnet,
        group_number: parseInt(numeroGrupoEstudiantes),
        course_code: codigoCursoEstudiantes
      }
    };

    try {
      const response = await axios.post("https://localhost:7190/Admin/AgregarEstudianteAGrupo", requestData);

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
    data_input_add_default_document_sections: {
      course_code: codigoDocuSecciones,
      sections: ["Presentaciones", "Quices", "Exámenes", "Proyectos"]
    }
  };

  try {
    const response = await axios.post("https://localhost:7190/Admin/AgregarSeccionesPorDefecto", requestData);

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
    data_input_add_default_grades: {
      course_code: codigoCursoRubrosDefault,
      sections: ["Quices", "Exámenes", "Proyectos"],
      percentages: [30.0, 30.0, 40.0]
    }
  };

  try {
    const response = await axios.post("https://localhost:7190/Admin/AgregarRubrosPorDefecto", requestData);

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

    // Asignar valores a los campos
    if (json.length > 0) {
      const row = json[0];

      const codigoCurso = row["Código Curso"];
      const año = row["Año"];
      const periodo = row["Periodo"];

      setASemestre(año);
      setASemestreCurso(año);
      setPeriodoSemestre(periodo);
      setPeriodoSemestreCurso(periodo);
      setCodigoCursoSemestre(codigoCurso);
      setCodigoCursoEstudiantes(codigoCurso);
      setCodigoDocuSecciones(codigoCurso);
      setCodigoCursoRubrosDefault(codigoCurso);
      setCarnetsEstudiantes(row["Carnets Estudiantes"]);
      setNumeroGrupoEstudiantes(row["Número Grupo"]);

    }
  };
  reader.readAsBinaryString(file);
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

      <label className={styles.label}>Números de Grupos Asociados (separar con comas si son varios):</label>
      <input
        type="text"
        className={styles.input}
        value={grupoCurso}
        onChange={(e) => {
          const valorFiltrado = e.target.value.replace(/[^0-9,]/g, ""); // solo números y comas
          setGrupoCurso(valorFiltrado);
        }}
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
