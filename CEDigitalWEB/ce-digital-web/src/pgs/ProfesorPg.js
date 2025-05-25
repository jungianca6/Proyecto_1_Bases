
import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import { Button, Card, Form } from 'react-bootstrap';
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import styles from './ProfesorPg.module.css';

function ProfesorPg() {
    const navigate = useNavigate();

    const handleNoticiasClick = () => {
        navigate('/profesor/noticias');
    }

    const handleDocumentosClick = () => {
        navigate('/profesor/documentos');
    }

    const handleEvaluacionesClick = () => {
        navigate('/profesor/evaluaciones');
    }

    const handlerepNotasClick = () => {
        navigate('/profesor/reportenotas');
    }

    const handlerepEstudiantesClick = () => {
        navigate('/profesor/reporteEstudiantes');
    }

    return (
        <div className={styles.profesorWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Profesor</h3>

            {/* Botones de navegación */}
            
            <Button
                className={styles.evaluacionesButton}
                onClick={handleEvaluacionesClick}>
                Evaluaciones
            </Button>

            <Button
                className={styles.documentosButton}
                onClick={handleDocumentosClick}>
                Documentos
            </Button>

            <Button
                className={styles.noticiasButton}
                onClick={handleNoticiasClick}>
                Noticias
            </Button>

            <Button
                className={styles.notasButton}
                onClick={handlerepNotasClick}>
                Reporte de notas
            </Button>

            <Button
                className={styles.reporteButton}
                onClick={handlerepEstudiantesClick}>
                Reporte de estudiantes
            </Button>
        </div>
    );
}

export default ProfesorPg;