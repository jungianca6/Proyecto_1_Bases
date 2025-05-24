import React, { useState } from "react";
import { Button, Card, Form } from 'react-bootstrap';
import axios from "axios";
import styles from './NoticiasProfesorPg.module.css';

function NoticiasProfesorPg() {
    const [formData, setFormData] = useState({
        tituloNoticia: "",
        mensajeNoticia: "",
        autor: "",
        cursoAsociado: ""
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handlePublicarNoticia = async () => {
        const { tituloNoticia, mensajeNoticia, autor, cursoAsociado } = formData;

        if (!tituloNoticia || !mensajeNoticia || !autor || !cursoAsociado) {
            alert("Por favor complete todos los campos");
            return;
        }

        const noticia = {
            title: tituloNoticia,
            message: mensajeNoticia,
            publication_date: new Date().toISOString(),
            author: autor,
            course_code: cursoAsociado
        };

        try {
            await axios.post("https://localhost:7199/News/add_new", noticia);
            alert(`Noticia "${tituloNoticia}" creada correctamente.`);
            setFormData({
                tituloNoticia: "",
                mensajeNoticia: "",
                autor: "",
                cursoAsociado: ""
            });
        } catch (error) {
            console.error("Error al publicar noticia:", error);
            alert("Ocurrió un error al publicar la noticia.");
        }
    };

    const handleEliminarNoticia = async () => {
        const { tituloNoticia, cursoAsociado } = formData;

        if (!tituloNoticia || !cursoAsociado) {
            alert("Para eliminar una noticia, complete el título y el código del curso.");
            return;
        }

        const payload = {
            title: tituloNoticia,
            course_code: cursoAsociado
        };

        try {
            await axios.post("https://localhost:7199/News/delete_new", payload);
            alert(`Noticia "${tituloNoticia}" eliminada correctamente.`);
            setFormData({
                tituloNoticia: "",
                mensajeNoticia: "",
                autor: "",
                cursoAsociado: ""
            });
        } catch (error) {
            console.error("Error al eliminar noticia:", error);
            alert("Ocurrió un error al eliminar la noticia.");
        }
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
                        <Form.Label>Autor</Form.Label>
                        <Form.Control
                            type="text"
                            name="autor"
                            value={formData.autor}
                            onChange={handleChange}
                            placeholder="Nombre del autor"
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

                    <Form.Text className="text-muted">
                        Para eliminar una noticia, solo es necesario rellenar el título y el curso asociado.
                    </Form.Text>

                    <div className={styles.buttonGroup}>
                        <Button
                            variant="primary"
                            onClick={handlePublicarNoticia}
                            className={styles.actionButton}>
                            Publicar
                        </Button>

                        <Button
                            variant="primary"
                            className={styles.actionButton}>
                            Editar
                        </Button>

                        <Button
                            variant="danger"
                            onClick={handleEliminarNoticia}
                            className={styles.actionButton}>
                            Eliminar
                        </Button>
                    </div>
                </Card.Body>
            </Card>
        </div>
    );
}

export default NoticiasProfesorPg;
