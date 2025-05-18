
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

    return (
        <div className={styles.profesorWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Profesor</h3>

            {/* Botones de navegación */}
            <Button
                className={styles.cursosButton}>
                Cursos
            </Button>

            <Button
                className={styles.evaluacionesButton}>
                Evaluaciones
            </Button>

            <Button
                className={styles.documentosButton}>
                Documentos
            </Button>

            <Button
                className={styles.rubrosButton}>
                Rubros
            </Button>

            <Button
                className={styles.noticiasButton}
                onClick={handleNoticiasClick}>
                Noticias
            </Button>

            <Button
                className={styles.notasButton}>
                Reporte de notas
            </Button>

            <Button
                className={styles.reporteButton}>
                Reporte de estudiantes
            </Button>
        </div>
    );
}

export default ProfesorPg;