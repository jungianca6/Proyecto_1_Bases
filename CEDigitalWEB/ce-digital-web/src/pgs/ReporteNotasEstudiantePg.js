import { useState, useEffect } from "react";
import axios from "axios";
import styles from './ReporteNotasEstudiantePg.module.css';

function ReporteNotasEstudiantePg() {
  return (
    <div className={styles.reporteContainer}>
      <h2 className={styles.reporteTitulo}>Reporte de Notas</h2>
    </div>
  );
}

export default ReporteNotasEstudiantePg;
