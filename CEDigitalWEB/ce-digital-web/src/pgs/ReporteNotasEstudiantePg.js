import React, { useState, useEffect } from "react";
import axios from "axios";
import styles from './ReporteNotasEstudiantePg.module.css';

function ReporteNotasEstudiantePg() {
  return (
    <div className={styles.reporteContainer}>
        <h1 className={styles.title}>CE Digital</h1>
        <h3 className={styles.subtitle}>Reporte de notas</h3>
    </div>
  );
}

export default ReporteNotasEstudiantePg;
