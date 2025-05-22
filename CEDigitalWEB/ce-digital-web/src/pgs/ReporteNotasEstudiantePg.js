import React, { useState, useEffect } from "react";
import axios from "axios";
import styles from './ReporteNotasEstudiantePg.module.css';

function ReporteNotasEstudiantePg() {
  const [cuenta, setCuenta] = useState(null);
  const [cursoSeleccionado, setCursoSeleccionado] = useState(null);
  const [evaluaciones, setEvaluaciones] = useState([]);

  useEffect(() => {
    const cuentaGuardada = JSON.parse(localStorage.getItem("cuenta_actual"));
    const cursoGuardado = JSON.parse(localStorage.getItem("curso_seleccionado"));

    if (cuentaGuardada) {
      setCuenta(cuentaGuardada);
      console.log("Cuenta actual:", cuentaGuardada);
    }

    if (cursoGuardado) {
      setCursoSeleccionado(cursoGuardado);
      console.log("Curso seleccionado:", cursoGuardado);
    }

    // Hacer la consulta solo si ambos están definidos
    if (cuentaGuardada && cursoGuardado) {
      obtenerReporteNotas(cuentaGuardada.primary_key, cursoGuardado.course_code, cursoGuardado.group_number);
    }
  }, []);

  const obtenerReporteNotas = async (student_id, course_code, group_number) => {
    try {
      const response = await axios.post("https://localhost:7199/Report/student_grades_report", {
        student_id,
        course_code,
        group_number
      });

      if (response.data.status === "OK") {
        const evals = response.data.message.evaluations || [];
        setEvaluaciones(evals);
        console.log("Evaluaciones recibidas:", evals);
      } else {
        console.warn("No se pudo obtener el reporte de notas.");
      }
    } catch (error) {
      console.error("Error al obtener el reporte:", error);
    }
  };

  return (
    <div className={styles.reporteContainer}>
      <h1 className={styles.title}>CE Digital</h1>
      <h3 className={styles.subtitle}>Reporte de notas</h3>

      {cuenta && (
        <h4 className={styles.infoLinea}>
          Estudiante: {cuenta.username}
        </h4>
      )}

      {cursoSeleccionado && (
        <h4 className={styles.infoLinea}>
          Curso: {cursoSeleccionado.course_code}: {cursoSeleccionado.course_name} - Grupo {cursoSeleccionado.group_number}
        </h4>
      )}

      <div className={styles.listaNotas}>
        {evaluaciones.length === 0 ? (
          <p className={styles.textoBlanco}>No hay evaluaciones registradas para este estudiante.</p>
        ) : (
          evaluaciones.map((eva, index) => (
            <div key={index} className={styles.notaItem}>
              <p><strong>Rúbrica:</strong> {eva.rubric_name}</p>
              <p><strong>Evaluación:</strong> {eva.evaluation_title}</p>
              <p><strong>Nota:</strong> {eva.grade}/100</p>
              <hr />
            </div>
          ))
        )}
      </div>
    </div>
  );
}

export default ReporteNotasEstudiantePg;
