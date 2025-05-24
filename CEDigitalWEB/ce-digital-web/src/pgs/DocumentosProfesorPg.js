import React, { useState } from "react";
import { Button, Card, ListGroup, Alert } from 'react-bootstrap';
import styles from './DocumentosProfesorPg.module.css';
import axios from "axios";

function DocumentosProfesorPg() {
    const [selectedFile, setSelectedFile] = useState(null);
    const [uploadedDocuments, setUploadedDocuments] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);

    const handleFileChange = (e) => {
        const file = e.target.files[0];
        if (file) {
            if (file.type === "application/pdf") {
                setSelectedFile(file);
                setError(null);
            } else {
                setError("Por favor, seleccione un archivo PDF");
                setSelectedFile(null);
            }
        }
    };

    const handleUpload = async () => {
        if (!selectedFile) {
            setError("Por favor, seleccione un archivo PDF primero");
            return;
        }

        setLoading(true);
        setError(null);

        try {
            // Simulamos la subida al backend (en producción usarías axios o fetch)
            await new Promise(resolve => setTimeout(resolve, 1000));

            const newDocument = {
                name: selectedFile.name,
                size: formatFileSize(selectedFile.size),
                uploadDate: new Date().toLocaleString(),
                url: URL.createObjectURL(selectedFile) // En producción sería la URL del backend
            };

            setUploadedDocuments(prev => [newDocument, ...prev]);
            setSelectedFile(null);

            // Limpiar el input de archivo
            document.getElementById("pdfUpload").value = "";

        } catch (err) {
            setError("Error al subir el archivo: " + err.message);
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
                    </div>
                    {error && <Alert variant="danger" className={styles.alert}>{error}</Alert>}
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
        </div>
    );
}

export default DocumentosProfesorPg;