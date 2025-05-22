import React, { useState, useEffect } from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './DocumentosEstudiantePg.module.css';

function DocumentosEstudiantePg() {
  const [cuenta, setCuenta] = useState(null);
  const [cursoSeleccionado, setCursoSeleccionado] = useState(null);

  useEffect(() => {
    const cuentaGuardada = JSON.parse(localStorage.getItem("cuenta_actual"));
    const cursoGuardado = JSON.parse(localStorage.getItem("curso_seleccionado"));

    if (cuentaGuardada) {
      setCuenta(cuentaGuardada);
      console.log("Datos de cuenta:", cuentaGuardada);
    }

    if (cursoGuardado) {
      setCursoSeleccionado(cursoGuardado);
      console.log("Curso activo:", cursoGuardado);
    } else {
      console.warn("No hay curso seleccionado.");
    }
  }, []);

  return (
    <div className={styles.documentosWrapper}>
      <h1 className={styles.title}>CE Digital</h1>
      <h3 className={styles.subtitle}>Documentos</h3>

      {cuenta && (
        <h4 className={styles.infoLinea}>
          Estudiante: {cuenta.username}
        </h4>
      )}

      {cursoSeleccionado && (
        <h4 className={styles.infoLinea}>
          Curso actual: {cursoSeleccionado.course_code}: {cursoSeleccionado.course_name} - Grupo {cursoSeleccionado.group_number}
        </h4>
      )}
    </div>
  );
}

export default DocumentosEstudiantePg;
