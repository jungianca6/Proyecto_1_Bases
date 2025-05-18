import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './ReporteNotasProfesorPg.module.css';

function ReporteNotasProfesorPg() {
    return(<div className={styles.reporteNotasWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Reporte de notas</h3>

        </div>
    );
}
export default ReporteNotasProfesorPg;