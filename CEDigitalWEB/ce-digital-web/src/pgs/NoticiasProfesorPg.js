import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import { Button, Card, Form } from 'react-bootstrap';
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import styles from './NoticiasProfesorPg.module.css';

function NoticiasProfesorPg() {


    // Estados para el formulario de noticias
    const [tituloNoticia, setTituloNoticia] = useState("");
    const [mensajeNoticia, setMensajeNoticia] = useState("");
    const [fechaPublicacion, setFechaPublicacion] = useState("");
    const [cursoAsociado, setCursoAsociado] = useState("");

    const handlePublicarNoticia = () => {
        // Validación básica
        if (!tituloNoticia || !mensajeNoticia || !fechaPublicacion || !cursoAsociado) {
            alert("Por favor complete todos los campos");
            return;
        }

        // Aquí iría la lógica para enviar al backend
        alert(`Noticia "${tituloNoticia}" creada para el curso ${cursoAsociado}`);

        // Limpiar formulario después de "enviar"
        setTituloNoticia("");
        setMensajeNoticia("");
        setFechaPublicacion("");
        setCursoAsociado("");
    };

    return (
        <div className={styles.noticiasprofesorWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Noticias</h3>

            {/* Sección de Publicación de Noticias */}
            <Card className={styles.noticiasCard}>
                <Card.Header as="h5">Publicar Nueva Noticia</Card.Header>
                <Card.Body>
                    <Form.Group className="mb-3">
                        <Form.Label>Título de la Noticia</Form.Label>
                        <Form.Control
                            type="text"
                            value={tituloNoticia}
                            onChange={(e) => setTituloNoticia(e.target.value)}
                            placeholder="Ingrese el título de la noticia"
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Mensaje</Form.Label>
                        <Form.Control
                            as="textarea"
                            rows={3}
                            value={mensajeNoticia}
                            onChange={(e) => setMensajeNoticia(e.target.value)}
                            placeholder="Escriba el contenido de la noticia"
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Fecha de Publicación</Form.Label>
                        <Form.Control
                            type="date"
                            value={fechaPublicacion}
                            onChange={(e) => setFechaPublicacion(e.target.value)}
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Curso Asociado</Form.Label>
                        <Form.Control
                            type="text"
                            value={cursoAsociado}
                            onChange={(e) => setCursoAsociado(e.target.value)}
                            placeholder="Código del curso"
                        />
                    </Form.Group>

                    <Button
                        variant="primary"
                        onClick={handlePublicarNoticia}
                        className={styles.publicarButton}>
                        Publicar Noticia
                    </Button>
                </Card.Body>
            </Card>
        </div>
    );
}

export default NoticiasProfesorPg;