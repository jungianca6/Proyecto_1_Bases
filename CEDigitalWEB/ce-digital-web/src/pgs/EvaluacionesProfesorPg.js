import React, {useState} from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './EvaluacionesProfesorPg.module.css';
import axios from "axios";

function DocumentosProfesorPg() {

    const [selectedFile, setSelectedFile] = useState(null);
    //const [uploadedDocuments, setUploadedDocuments] = useState([]);
    const [loading, setLoading] = useState(false);

    const [formData, setFormData] = useState({
        cursoCodigo: "",
        grupoNumero: "",
        rubroNombre: "",
        evaluacionNombre: "",
        rubroPorcentaje: "",
        evaluacionPorcentaje: "",
        fechaEntrega: "",
        descripcion: "",
        tipoEvaluacion: "",
        nombreArchivo: "",
        pathArchivo: "",
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const formatFileSize = (bytes) => {
        if (bytes === 0) return "0 Bytes";
        const k = 1024;
        const sizes = ["Bytes", "KB", "MB", "GB"];
        const i = Math.floor(Math.log(bytes) / Math.log(k));
        return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + " " + sizes[i];
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
            const response = await axios.post("https://localhost:7199//Group/assign_evaluation", {
                course_code: formData.course_code,
                group_number: formData.grupoNumero,
                grading_item_name: formData.rubroNombre,
                evaluation_name: formData.evaluacionNombre,
                grading_item_percentage: formData.rubroPorcentaje,
                evaluation_percentage: formData.evaluacionPorcentaje,
                delivery_date: formData.fechaEntrega,
                description: formData.descripcion,
                evaluation_type: formData.tipoEvaluacion,
                professor_filename: formData.nombreArchivo,
                data_base_path_professor: "C:/Users/Dell/Desktop/Proyecto_1_Bases/CEDigital/data_base_files/Professor_evaluation"
            });

            if (response.data.status === "OK") {
                const data = response.data.message;
                console.log("Datos recibidos:", data);

            } else {
                const data = response.data.message;
                console.log("Error:", data);
            }

            // 2. Forzar descarga del archivo en el cliente
            const downloadUrl = URL.createObjectURL(selectedFile);
            const downloadLink = document.createElement("a");
            downloadLink.href = downloadUrl;
            downloadLink.download = selectedFile.name; // Nombre del archivo
            document.body.appendChild(downloadLink);
            downloadLink.click(); // Se abre el diálogo de descarga
            document.body.removeChild(downloadLink);
            URL.revokeObjectURL(downloadUrl);

            setSelectedFile(null);

            // Limpiar el input de archivo
            document.getElementById("pdfUpload").value = "";
        } catch (err) {
            console.log("Error al asignar evaluación: " + err.message);
        } finally {
            setLoading(false);
        }
    };

    return(<div className={styles.evaluacionesProfeWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Evaluaciones</h3>

            <Card className={styles.assignCard}>
                <Card.Header>Asignar nueva evaluación</Card.Header>
                <Card.Body>
                    <Form.Group className="mb-1">
                    <Form.Label>Código de curso</Form.Label>
                    <Form.Control
                        type="text"
                        name="cursoCodigo"
                        value={formData.cursoCodigo}
                        onChange={handleChange}
                        placeholder="Código de curso"
                    />
                </Form.Group>

                    <Form.Group className="mb-1">
                        <Form.Label>Número de grupo</Form.Label>
                        <Form.Control
                            type="text"
                            name="grupoNumero"
                            value={formData.grupoNumero}
                            onChange={handleChange}
                            placeholder="Número de grupo"
                        />
                    </Form.Group>

                    <Form.Group className="mb-1">
                        <Form.Label>Nombre de evaluación</Form.Label>
                        <Form.Control
                            type="text"
                            name="evaluacionNombre"
                            value={formData.evaluacionNombre}
                            onChange={handleChange}
                            placeholder="Nombre de evaluación"
                        />
                    </Form.Group>

                    <Form.Group className="mb-1">
                        <Form.Label>Porcentaje de rubro</Form.Label>
                        <Form.Control
                            type="text"
                            name="rubroPorcentaje"
                            value={formData.rubroPorcentaje}
                            onChange={handleChange}
                            placeholder="Porcentaje de rubro"
                        />
                    </Form.Group>

                    <Form.Group className="mb-1">
                        <Form.Label>Porcentaje de evaluación</Form.Label>
                        <Form.Control
                            type="text"
                            name="evaluacionPorcentaje"
                            value={formData.evaluacionPorcentaje}
                            onChange={handleChange}
                            placeholder="Porcentaje de evaluación"
                        />
                    </Form.Group>

                    <Form.Group className="mb-1">
                        <Form.Label>Fecha de entrega</Form.Label>
                        <Form.Control
                            type="text"
                            name="fechaEntrega"
                            value={formData.fechaEntrega}
                            onChange={handleChange}
                            placeholder="Fecha de entrega"
                        />
                    </Form.Group>

                    <Form.Group className="mb-1">
                        <Form.Label>Descripción</Form.Label>
                        <Form.Control
                            type="text"
                            name="descripcion"
                            value={formData.descripcion}
                            onChange={handleChange}
                            placeholder="Descripción"
                        />
                    </Form.Group>

                    <Form.Group className="mb-1">
                        <Form.Label>Tipo de evaluación</Form.Label>
                        <Form.Control
                            type="text"
                            name="tipoEvaluacion"
                            value={formData.tipoEvaluacion}
                            onChange={handleChange}
                            placeholder="Tipo de evaluación"
                        />
                    </Form.Group>

                    <Form.Group className="mb-1">
                        <Form.Label>Nombre de archivo</Form.Label>
                        <Form.Control
                            type="text"
                            name="nombreArchivo"
                            value={formData.nombreArchivo}
                            onChange={handleChange}
                            placeholder="Nombre del archivo"
                        />
                    </Form.Group>

                    <div className={styles.assignButton}>
                        <input
                            type="file"
                            id="pdfUpload"
                            accept=".pdf"
                            onChange={handleFileChange}
                            className={styles.fileInput}
                        />
                        <label htmlFor="pdfUpload" className={styles.assignLabel}>
                            Seleccionar PDF
                        </label>
                        {selectedFile && (
                            <div className={styles.fileInfo}>
                                <span>{selectedFile.name}</span>
                                <span>{formatFileSize(selectedFile.size)}</span>
                            </div>
                        )}

                        <Button
                            //onClick={handleUpload}
                            disabled={!selectedFile || loading}
                            className={styles.assignButton}
                        >
                            {loading ? "Subiendo..." : "Subir documento"}
                        </Button>

                        <Button
                            //onClick={handleDelete}
                            className={styles.assignButton}
                        >
                            {loading ? "Eliminando..." : "Eliminar documento"}
                        </Button>

                    </div>
                </Card.Body>
            </Card>

        </div>
    );


}
export default DocumentosProfesorPg;