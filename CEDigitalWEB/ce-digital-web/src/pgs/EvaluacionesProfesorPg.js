import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './EvaluacionesProfesorPg.module.css';

function DocumentosProfesorPg() {
    return(<div className={styles.evaluacionesProfeWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Evaluaciones</h3>

        </div>
    );
}
export default DocumentosProfesorPg;