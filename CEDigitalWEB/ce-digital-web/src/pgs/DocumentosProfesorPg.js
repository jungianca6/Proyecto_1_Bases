import React, { useState } from "react";
import {Button, Card, Form, ListGroup} from 'react-bootstrap';
import styles from './DocumentosProfesorPg.module.css';
import axios from "axios";

function DocumentosProfesorPg() {
    const [selectedFile, setSelectedFile] = useState(null);
    const [uploadedDocuments, setUploadedDocuments] = useState([]);
    const [loading, setLoading] = useState(false);

    const [formData, setFormData] = useState({
        cursoCodigo: "",
        grupoID: "",
        seccionDocumento: "",
        nombreArchivo: "",
    });

    const [docuData, setDocuData] = useState({
        cursoCodigo: "",
        numeroGrupo: "",
        grupoID: "",
        seccionDocumento: "",
    });


    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handledocuChange = (e) => {
        const { name, value } = e.target;
        setDocuData(prev => ({
            ...prev,
            [name]: value
        }))
    };


    const handleFileChange = (e) => {
        const file = e.target.files[0];
        if (file) {
            if (file.type === "application/pdf") {
                setSelectedFile(file);
                // Actualiza el nombreArchivo con el nombre del archivo (sin la extensión .pdf)
                setFormData(prev => ({
                    ...prev,
                    nombreArchivo: file.name.replace(/\.pdf$/i, '') + ".pdf"
                }));
            } else {
                setSelectedFile(null);
                alert("Por favor, seleccione un archivo PDF");
            }
        }
    };

    const handleUpload = async () => {
        if (!selectedFile) {
            alert("Por favor, seleccione un archivo PDF primero");
            return;
        }
        setLoading(true);

        try {
            const response = await axios.post("https://localhost:7199/Document/add_document", {
                group_id: formData.grupoID,
                document_section: formData.seccionDocumento,
                file_name: formData.nombreArchivo,
                uploaded_by_professor: 1
            });

            if (response.data.status === "OK") {
                const data = response.data.message;
                console.log("Datos recibidos del backend:", data);

            } else {
                const data = response.data.message;
                console.log("Error:", data);
            }

            const newDocument = {
                name: selectedFile.name,
                size: formatFileSize(selectedFile.size),
                uploadDate: new Date().toLocaleString(),
                url: URL.createObjectURL(selectedFile)
            };

            setUploadedDocuments(prev => [newDocument, ...prev]);
            setSelectedFile(null);

            // Limpiar el input de archivo
            document.getElementById("pdfUpload").value = "";
        } catch (err) {
            console.log("Error al subir el archivo: " + err.message);
        } finally {
            setLoading(false);
        }
    };
    const handleDelete = async () => {
        setLoading(true);

        try {
            const response = await axios.post("https://localhost:7199/Document/delete_document", {
                course_code: "",
                group_id: formData.grupoID,
                document_section: formData.seccionDocumento,
                file_name: formData.nombreArchivo,
            });

            if (response.data.status === "OK") {
                const data = response.data.message;
                console.log("Datos recibidos del backend:", data);

                setUploadedDocuments(prev =>
                    prev.filter(doc =>
                        doc.name !== formData.nombreArchivo
                    )
                );

            } else {
                const data = response.data.message;
                console.log("Error:", data);
            }
        } catch (err) {
            console.log("Error al eliminar el archivo: " + err.message);
        } finally {
            setLoading(false);
        }
    };

    const formatFileSize = (bytes) => {
        if (bytes === 0) return "0 Bytes";
        const k = 1024;
        const sizes = ["Bytes", "KB", "MB", "GB"];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i];
    };

    return (
        <div className={styles.documentosProfeWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Documentos</h3>

            <Card className={styles.uploadCard}>
                <Card.Header>Subir nuevo documento</Card.Header>
                <Card.Body>
                    <Form.Group className="mb-3">
                        <Form.Label>ID de grupo</Form.Label>
                        <Form.Control
                            type="text"
                            name="grupoID"
                            value={formData.grupoID}
                            onChange={handleChange}
                            placeholder="Escriba el ID del grupo"
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Sección de documento</Form.Label>
                        <Form.Control
                            type="text"
                            name="seccionDocumento"
                            value={formData.seccionDocumento}
                            onChange={handleChange}
                            placeholder="Escriba la sección de documento"
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Nombre de archivo</Form.Label>
                        <Form.Control
                            type="text"
                            name="nombreArchivo"
                            value={formData.nombreArchivo}
                            onChange={handleChange}
                            placeholder="Nombre del archivo"
                        />
                    </Form.Group>

                    <div className={styles.uploadContainer}>
                        <input
                            type="file"
                            id="pdfUpload"
                            accept=".pdf"
                            onChange={handleFileChange}
                            className={styles.fileInput}
                        />
                        <label htmlFor="pdfUpload" className={styles.uploadLabel}>
                            Seleccionar PDF
                        </label>
                        {selectedFile && (
                            <div className={styles.fileInfo}>
                                <span>{selectedFile.name}</span>
                                <span>{formatFileSize(selectedFile.size)}</span>
                            </div>
                        )}

                        <Button
                            onClick={handleUpload}
                            disabled={!selectedFile || loading}
                            className={styles.uploadButton}
                        >
                            {loading ? "Subiendo..." : "Subir documento"}
                        </Button>

                        <Button
                            onClick={handleDelete}
                            className={styles.uploadButton}
                        >
                            {loading ? "Eliminando..." : "Eliminar documento"}
                        </Button>
                    </div>
                </Card.Body>
            </Card>

            {uploadedDocuments.length > 0 && (
                <Card className={styles.documentsCard}>
                    <Card.Header>Documentos subidos</Card.Header>
                    <Card.Body>
                        <ListGroup>
                            {uploadedDocuments.map((doc, index) => (
                                <ListGroup.Item key={index} className={styles.documentItem}>
                                    <div className={styles.documentInfo}>
                                        <span className={styles.docName}>{doc.name}</span>
                                        <span className={styles.docMeta}>
                                            {doc.size} - Subido el {doc.uploadDate}
                                        </span>
                                    </div>
                                    <Button
                                        variant="primary"
                                        size="sm"
                                        href={doc.url}
                                        target="_blank"
                                        rel="noopener noreferrer"
                                    >
                                        Ver
                                    </Button>
                                </ListGroup.Item>
                            ))}
                        </ListGroup>
                    </Card.Body>
                </Card>
            )}
            <Card className={styles.docSectionCard}>
                <Card.Header>Secciones de documento</Card.Header>
                <Card.Body>
                    <Form.Group className="mb-3">
                        <Form.Label>ID de grupo</Form.Label>
                        <Form.Control
                            type="text"
                            name="grupoID"
                            value={docuData.grupoID}
                            onChange={handledocuChange}
                            placeholder="Escriba el ID del grupo"
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Sección de documento</Form.Label>
                        <Form.Control
                            type="text"
                            name="seccionDocumento"
                            value={docuData.seccionDocumento}
                            onChange={handledocuChange}
                            placeholder="Escriba la sección de documento"
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Número de grupo</Form.Label>
                        <Form.Control
                            type="text"
                            name="numeroGrupo"
                            value={docuData.numeroGrupo}
                            onChange={handledocuChange}
                            placeholder="Número de grupo"
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>Código de curso</Form.Label>
                        <Form.Control
                            type="text"
                            name="cursoCodigo"
                            value={docuData.cursoCodigo}
                            onChange={handledocuChange}
                            placeholder="Código de curso"
                        />
                    </Form.Group>

                    <div className={styles.buttonGroup}>
                        <Button
                            variant="primary"
                            //onClick={handlePublicarNoticia}
                            className={styles.actionButton}>
                            Añadir
                        </Button>
                        
                        <Button
                            variant="danger"
                            //onClick={handleEliminarNoticia}
                            className={styles.actionButton}>
                            Eliminar
                        </Button>
                    </div>
                </Card.Body>
            </Card>

        </div>
    );
}

export default DocumentosProfesorPg;