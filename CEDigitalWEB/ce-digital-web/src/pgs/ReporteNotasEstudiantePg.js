import React, { useState, useEffect } from "react";
import axios from "axios";
import styles from './ReporteNotasEstudiantePg.module.css';

function ReporteNotasEstudiantePg() {
  const [cuenta, setCuenta] = useState(null);
  const [cursoSeleccionado, setCursoSeleccionado] = useState(null);

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
  }, []);

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
    </div>
  );
}

export default ReporteNotasEstudiantePg;
