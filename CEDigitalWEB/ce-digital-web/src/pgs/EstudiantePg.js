import { useState, useEffect } from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './EstudiantePg.module.css';


function EstudiantePg() {

    const navigate = useNavigate();

    // Manejadores para los botones
    const handleCursosClick = () => {
        alert("Navegando a Cursos");
    };

    const handleEvaluacionesClick = () => {
        navigate('/estudiante/evaluaciones');
    }

    const handleDocumentosClick = () => {
        navigate('/estudiante/documentos');
    };

    return (
        <div className={styles.estudianteWrapper}>

            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Estudiante</h3>

            {/* Sección Noticias */}
            <Card className={styles.noticiasSection}>
                <Card.Header as="h5">Noticias</Card.Header>
                <Card.Body>
                    <Form.Control
                        as="textarea"
                        rows={5}
                        placeholder="Aquí se mostrarán las noticias importantes..."
                        readOnly
                        className={styles.textArea}
                    />
                </Card.Body>
            </Card>

            {/* Botones de navegación */}
            <Button
                variant="dark"
                className={styles.cursosButton}
                onClick={handleCursosClick}>
                Cursos
            </Button>

            <Button
                variant="dark"
                className={styles.evaluacionesButton}
                onClick={handleEvaluacionesClick}>
                Evaluaciones
            </Button>

            <Button
                variant="dark"
                className={styles.documentosButton}
                onClick={handleDocumentosClick}>
                Documentos
            </Button>
        </div>
    );
}

export default EstudiantePg;