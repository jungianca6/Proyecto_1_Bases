import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import axios from "axios";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './EstudiantePg.module.css';

function EstudiantePg() {
    const [cuenta, setCuenta] = useState(null);
    const [noticias, setNoticias] = useState([]); // ← NUEVO
    const navigate = useNavigate();

    useEffect(() => {
        const cuentaGuardada = JSON.parse(localStorage.getItem("cuenta_actual"));
        if (cuentaGuardada) {
            setCuenta(cuentaGuardada);
            console.log("Datos de cuenta:", cuentaGuardada);
            fetchNoticias(cuentaGuardada.primary_key); // ← obtener noticias
        }
    }, []);

    const fetchNoticias = async (studentId) => {
    try {
        const response = await axios.post("https://localhost:7199/News/view_news", {
            student_id: studentId
        });

        if (response.data.status === "success" || response.data.status === "OK") {
            const noticiasOrdenadas = (response.data.message.news || []).sort((a, b) =>
                new Date(b.publish_date) - new Date(a.publish_date)
            );
            setNoticias(noticiasOrdenadas);
            console.log("Noticias recibidas (ordenadas):", noticiasOrdenadas);
        } else {
            console.warn("No se pudieron obtener noticias.");
        }
    } catch (error) {
        console.error("Error al obtener noticias:", error);
    }
};


    const handleCursosClick = () => {
        alert("Navegando a Cursos");
    };

    const handleEvaluacionesClick = () => {
        navigate('/estudiante/evaluaciones');
    };

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
                    {noticias.length === 0 ? (
                        <p>No hay noticias disponibles.</p>
                    ) : (
                        noticias.map((n, index) => (
                            <div key={index} className={styles.noticiaItem}>
                                <h5>{n.title}</h5>
                                <p>{n.message}</p>
                                <p><strong>Autor:</strong> {n.author}</p>
                                <p><strong>Fecha:</strong> {new Date(n.publish_date).toLocaleDateString()}</p>
                                <hr />
                            </div>
                        ))
                    )}
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
