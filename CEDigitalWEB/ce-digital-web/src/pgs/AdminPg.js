import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import styles from "./AdminPg.module.css"; 
import { Button, Card, Form } from 'react-bootstrap';

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

  // Separar las cédulas por comas y eliminar espacios extra
  const listaCedulas = cedulasProfesores
    .split(',')
    .map(c => c.trim())
    .filter(c => c.length > 0); // eliminar entradas vacías

  const curso = {
    codigo: codigoCurso,
    nombre: nombreCurso,
    creditos: parseInt(creditosCurso),
    carrera: carreraCurso,
    grupo: grupoCurso,
    cedulasProfesores: listaCedulas, // ahora es un array
  };
  console.log("Crear curso:", curso);
};
  const handleVisualizarCursos = async (e) => {
    e.preventDefault();
    console.log("Visualizar cursos");
  };

  const handleDeshabilitarCurso = async (e) => {
    e.preventDefault();
    console.log("Deshabilitar curso con código:", codigoCurso);
  };

  const handleInicializarSemestre = (e) => {
    e.preventDefault();

    const semestre = {
      anio: aSemestre,
      periodo: periodoSemestre,
    };

    console.log("Inicializar semestre:", semestre);
  };

  const handleAgregarCursoASemestre = (e) => {
    e.preventDefault();

    const data = {
      codigoCurso: codigoCursoSemestre,
      anio: aSemestreCurso,
      periodo: periodoSemestreCurso,
    };

    console.log("Agregar curso a semestre:", data);
  };

  const handleAgregarEstudiantesAGrupo = (e) => {
    e.preventDefault();
    // Separar los carnets por coma y limpiar espacios
    const listaCarnets = carnetsEstudiantes
      .split(',')
      .map(c => c.trim())
      .filter(c => c.length > 0);
    const data = {
      carnetsEstudiantes: listaCarnets,
      numeroGrupo: numeroGrupoEstudiantes,
      codigoCurso: codigoCursoEstudiantes,
    };
    console.log("Agregar estudiantes a grupo:", data);
  };

  const handleAgregarDocuSeccionesPorDefecto = (e) => {
  e.preventDefault();

  const data = {
    codigoCurso: codigoDocuSecciones,
    secciones: ["Presentaciones", "Quices", "Exámenes", "Proyectos"]
  };

  console.log("Agregar secciones por defecto:", data);
};

const handleAgregarRubrosDefaultACurso = (e) => {
  e.preventDefault();

  const data = {
    codigoCurso: codigoCursoRubrosDefault,
    rubros: ["Quices", "Exámenes", "Proyectos"],
    porcentajes: [30, 30, 40]
  };

  console.log("Agregar rubros por defecto a curso:", data);
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

      <label className={styles.label}>Número de Grupo Asociado:</label>
      <input
        type="text"
        className={styles.input}
        value={grupoCurso}
        onChange={(e) => setGrupoCurso(e.target.value)}
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
    </form>

        <h3 className={styles.title} style={{ marginTop: "3rem" }}>
        Inicializar Semestre
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
        <input
          type="text"
          className={styles.input}
          value={periodoSemestre}
          onChange={(e) => setPeriodoSemestre(e.target.value)}
        />

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
        <input
          type="text"
          className={styles.input}
          value={periodoSemestreCurso}
          onChange={(e) => setPeriodoSemestreCurso(e.target.value)}
        />

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

    </div>
  );
}

export default AdminPg;
