import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './ReporteEstudianteProfesorPg.module.css';

function ReporteEstudiantesProfesorPg() {
    return(<div className={styles.reporteestudiantesWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Reporte de estudiantes</h3>

        </div>
    );
}
export default ReporteEstudiantesProfesorPg;