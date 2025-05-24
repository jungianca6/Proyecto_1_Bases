import React, {useState} from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './ReporteEstudianteProfesorPg.module.css';
import axios from "axios";


function ReporteEstudiantesProfesorPg() {

    const [formData, setFormData] = useState({
        codigoCurso: "",
        grupoCurso: "",
    });

    const [reporteEstudiante, setReporteEstudiante] = useState("");
    const [loading, setLoading] = useState(false)

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleReporteEstudiante = async () => {
        if (!formData.codigoCurso || !formData.grupoCurso) {
            alert("Por favor ingrese el código del curso y el número de grupo");
            return;
        }

        setLoading(true);
        setReporteEstudiante("");

        try {
            const response = await axios.post("https://localhost:7199/Report/enrolled_students_report", {
                course_code: formData.codigoCurso,
                group_number: formData.grupoCurso
            });

            if (response.data.status === "OK") {
                const data = response.data.message;
                console.log("Datos recibidos del backend:", data);


                if (response.data.message.students) {
                    const reporteFormateado = formatReport(response.data.message.students);
                    setReporteEstudiante(reporteFormateado);
                } else {
                    setReporteEstudiante("No se encontraron estudiantes para este grupo.");
                }
            } else {
                setReporteEstudiante("Error al obtener el reporte: " + (response.data.message || "Error desconocido"));
            }
        } catch (error) {
            console.error("Error al obtener el reporte:", error);
            setReporteEstudiante("Error al conectar con el servidor");
        } finally {
            setLoading(false);
        }
    };

    const formatReport = (students) => {
        if (!students || students.length === 0) {
            return "No hay estudiantes matriculados en este grupo.";
        }

        return students.map(student => {
            return `Estudiante: ${student.student_name}
        ID: ${student.student_id}
        Email: ${student.email || "No disponible"}
        Teléfono: ${student.phone || "No disponible"}
        -------------------------\n`;
        }).join("");
    };

    return(<div className={styles.reporteestudiantesWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Reporte de estudiantes</h3>

            <Card className={styles.reporteSection}>
                <Card.Header as="h5">Reporte de estudiantes</Card.Header>
                <Card.Body>
                    <Form.Control
                        as="textarea"
                        rows={5}
                        value={reporteEstudiante}
                        readOnly
                        className={styles.textArea}
                        placeholder={loading ? "Cargando reporte..." : "El reporte aparecerá aquí"}
                    />
                </Card.Body>
            </Card>

        <Card className={styles.reporteCard}>
            <Card.Header as="h5">Reporte de estudiantes matriculados</Card.Header>
            <Card.Body>
                <Form.Group className="mb-3">
                    <Form.Label>Código de curso</Form.Label>
                    <Form.Control
                        type="text"
                        name="codigoCurso"
                        value={formData.codigoCurso}
                        onChange={handleChange}
                        placeholder="Ingrese el código del curso"
                    />
                </Form.Group>

                <Form.Group className="mb-3">
                    <Form.Label>Número de grupo</Form.Label>
                    <Form.Control
                        type="text"
                        rows={3}
                        name="grupoCurso"
                        value={formData.grupoCurso}
                        onChange={handleChange}
                        placeholder="Escriba el número del grupo"
                    />
                </Form.Group>

                <Button
                    onClick={handleReporteEstudiante}
                    className={styles.publicarButton}>
                    Obtener reporte
                </Button>
            </Card.Body>
        </Card>
    </div>
    );
}
export default ReporteEstudiantesProfesorPg;