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
    // Estados para el contenido de las áreas de texto
    const [noticias, setNoticias] = React.useState("");
    const [notas, setNotas] = React.useState("");

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
            {/* Sección Noticias */}
            <Card className={styles.noticiasSection}>
                <Card.Header as="h5">Noticias</Card.Header>
                <Card.Body>
                    <Form.Control
                        as="textarea"
                        rows={5}
                        value={noticias}
                        onChange={(e) => setNoticias(e.target.value)}
                        placeholder="Aquí se mostrarán las noticias importantes..."
                        readOnly
                        className={styles.textArea}
                    />
                </Card.Body>
            </Card>

            {/* Sección Notas */}
            <Card className={styles.notasSection}>
                <Card.Header as="h5">Notas</Card.Header>
                <Card.Body>
                    <Form.Control
                        as="textarea"
                        rows={5}
                        value={notas}
                        onChange={(e) => setNotas(e.target.value)}
                        placeholder="Aquí se mostrarán tus calificaciones..."
                        readOnly
                        className={styles.textArea}
                    />
                </Card.Body>
            </Card>

            {/* Botones de navegación */}
            <Button
                className={styles.cursosButton}
                onClick={handleCursosClick}>
                Cursos
            </Button>

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
        </div>
    );
}

export default EstudiantePg;