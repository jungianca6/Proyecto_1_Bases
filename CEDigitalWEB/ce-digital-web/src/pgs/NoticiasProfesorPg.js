import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import { useNavigate } from "react-router-dom";
import React, { useState } from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './NoticiasProfesorPg.module.css';

function NoticiasProfesorPg() {
    const [formData, setFormData] = useState({
        tituloNoticia: "",
        mensajeNoticia: "",
        fechaPublicacion: "",
        cursoAsociado: ""
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handlePublicarNoticia = () => {
        const { tituloNoticia, mensajeNoticia, fechaPublicacion, cursoAsociado } = formData;

        if (!tituloNoticia || !mensajeNoticia || !fechaPublicacion || !cursoAsociado) {
            alert("Por favor complete todos los campos");
            return;
        }

        alert(`Noticia "${tituloNoticia}" creada para el curso ${cursoAsociado}`);
        setFormData({
            tituloNoticia: "",
            mensajeNoticia: "",
            fechaPublicacion: "",
            cursoAsociado: ""
        });
    };

    return (
        <div className={styles.noticiasprofesorWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Noticias</h3>

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

            <Card className={styles.noticiasCard}>
                <Card.Header as="h5">Publicar Nueva Noticia</Card.Header>
                <Card.Body>
                    <Form.Group className="mb-3">
                        <Form.Label>Título de la Noticia</Form.Label>
                        <Form.Control
                            type="text"
                            name="tituloNoticia"
                            value={formData.tituloNoticia}
                            onChange={handleChange}
                            placeholder="Ingrese el título de la noticia"
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Mensaje</Form.Label>
                        <Form.Control
                            as="textarea"
                            rows={3}
                            name="mensajeNoticia"
                            value={formData.mensajeNoticia}
                            onChange={handleChange}
                            placeholder="Escriba el contenido de la noticia"
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Fecha de Publicación</Form.Label>
                        <Form.Control
                            type="date"
                            name="fechaPublicacion"
                            value={formData.fechaPublicacion}
                            onChange={handleChange}
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Curso Asociado</Form.Label>
                        <Form.Control
                            type="text"
                            name="cursoAsociado"
                            value={formData.cursoAsociado}
                            onChange={handleChange}
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