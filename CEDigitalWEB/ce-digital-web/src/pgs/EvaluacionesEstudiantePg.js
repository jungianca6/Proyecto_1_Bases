import React, { useState, useEffect } from "react";
import axios from "axios";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './EvaluacionesEstudiantePg.module.css';

function EvaluacionesEstudiantesPg() {
    const [cuenta, setCuenta] = useState(null);
    const [evaluaciones, setEvaluaciones] = useState([]);
    const [archivos, setArchivos] = useState({});
    const [entregasCompletadas, setEntregasCompletadas] = useState({}); // ← NUEVO

    useEffect(() => {
        const cuentaGuardada = JSON.parse(localStorage.getItem("cuenta_actual"));
        if (cuentaGuardada) {
            setCuenta(cuentaGuardada);
            fetchEvaluaciones(cuentaGuardada.primary_key);
        }
    }, []);

    const fetchEvaluaciones = async (studentId) => {
        try {
            const response = await axios.post("https://localhost:7199/Evaluation/view_student_evaluations", {
                student_id: studentId
            });

            if (response.data.status === "success") {
                const todas = response.data.message.evaluations || [];
                const publicas = todas.filter(e => e.is_public === true);
                setEvaluaciones(publicas);
            } else {
                console.warn("Evaluaciones no disponibles.");
            }
        } catch (error) {
            console.error("Error al obtener evaluaciones:", error);
        }
    };

    const handleFileChange = (event, titulo) => {
        setArchivos(prev => ({
            ...prev,
            [titulo]: event.target.files[0]
        }));
    };

    const handleSubmitArchivo = async (titulo) => {
        const archivo = archivos[titulo];
        const evaluacion = evaluaciones.find(e => e.evaluation_title === titulo);

        if (!archivo || !cuenta || !evaluacion) {
            alert("Faltan datos para enviar la entrega.");
            return;
        }

        const reader = new FileReader();

        reader.onload = async (e) => {
            const contenidoBase64 = e.target.result.split(',')[1]; // sin encabezado base64

            const payload = {
                student_id: cuenta.primary_key,
                course_code: "",
                group_number: null,
                grading_item_name: evaluacion.rubric_name,
                evaluation_title: evaluacion.evaluation_title,
                file_name: archivo.name,
                file_path: contenidoBase64
            };

            try {
                const response = await axios.post("https://localhost:7199/Evaluation/send_student_evaluation", payload);

                if (response.data.status === "success") {
                    alert(`Entrega enviada correctamente para "${titulo}"`);
                    setEntregasCompletadas(prev => ({
                        ...prev,
                        [titulo]: true
                    }));
                } else {
                    alert(`Error al enviar entrega: ${response.data.message}`);
                }
            } catch (error) {
                console.error("Error al enviar entrega:", error);
                alert("Hubo un error al enviar la entrega.");
            }
        };

        reader.readAsDataURL(archivo);
    };

    return (
        <div className={styles.evaluacionesWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Evaluaciones</h3>

            {cuenta && <h3 className={styles.listaEvaluacion}>Estudiante: {cuenta.username}</h3>}

            <Card className={styles.notasSection}>
                <Card.Header as="h5">Evaluaciones</Card.Header>
                <Card.Body>
                    {evaluaciones.length === 0 ? (
                        <p>No hay evaluaciones públicas disponibles.</p>
                    ) : (
                        evaluaciones.map((eva, index) => (
                            <div key={index} className={styles.evaluacionItem}>
                                <h5>{eva.evaluation_title} ({eva.rubric_name})</h5>
                                <p><strong>Nota:</strong> {eva.grade}/100</p>
                                <p><strong>Comentarios:</strong> {eva.feedback}</p>
                                <p><strong>Fecha:</strong> {new Date(eva.evaluation_date).toLocaleDateString()}</p>

                                <Form.Group controlId={`file-${index}`} className="mb-2">
                                    <Form.Label>Entregar archivo:</Form.Label>
                                    <Form.Control
                                        type="file"
                                        onChange={(e) => handleFileChange(e, eva.evaluation_title)}
                                    />
                                </Form.Group>

                                <Button
                                    variant="primary"
                                    onClick={() => handleSubmitArchivo(eva.evaluation_title)}
                                    disabled={!archivos[eva.evaluation_title]}
                                >
                                    Enviar entrega
                                </Button>

                                {entregasCompletadas[eva.evaluation_title] && (
                                    <p style={{ color: "green", marginTop: "0.5rem" }}>
                                        ✅ Entrega enviada correctamente.
                                    </p>
                                )}

                                <hr />
                            </div>
                        ))
                    )}
                </Card.Body>
            </Card>
        </div>
    );
}

export default EvaluacionesEstudiantesPg;
