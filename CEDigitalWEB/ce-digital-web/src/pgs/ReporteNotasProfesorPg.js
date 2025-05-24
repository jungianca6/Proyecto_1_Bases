import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React, {useState} from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './ReporteNotasProfesorPg.module.css';
import axios from "axios";

function ReporteNotasProfesorPg() {

    const [formData, setFormData] = useState({
        codigoCurso: "",
        grupoCurso: "",
    });

    const [reporteNotas, setReporteNotas] = useState("");
    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleObtenerReporte = async () => {
        // Validación básica
        if (!formData.codigoCurso || !formData.grupoCurso) {
            alert("Por favor ingrese el código del curso y el número de grupo");
            return;
        }

        setLoading(true);
        setReporteNotas(""); // Limpiar reporte anterior

        try {
            const response = await axios.post("https://localhost:7199/Report/grades_report", {
                course_code: formData.codigoCurso,
                group_number: formData.grupoCurso
            });

            if (response.data.status === "OK") {

                const data = response.data.message;
                console.log("Datos recibidos del backend:", data);
                console.log("Notas:", data.grades);


                if (response.data.message.grades) {
                    const reporteFormateado = formatReport(response.data.message.grades);
                    setReporteNotas(reporteFormateado);
                } else {
                    setReporteNotas("No se encontraron estudiantes para este grupo.");
                }
            } else {
                setReporteNotas("Error al obtener el reporte: " + (response.data.message || "Error desconocido"));
            }
        } catch (error) {
            console.error("Error al obtener el reporte:", error);
            setReporteNotas("Error al conectar con el servidor");
        } finally {
            setLoading(false);
        }
    };

    // Función para formatear el reporte
    const formatReport = (students) => {
        if (!students) return "No hay datos disponibles";

        return students.map(student => {
            let reporte = `Estudiante: ${student.student_name} (ID: ${student.student_id})\n`;

            if (student.grades_by_rubric) {
                Object.entries(student.grades_by_rubric).forEach(([rubric, grade]) => {
                    reporte += `  ${rubric}: ${grade}\n`;
                });
            } else {
                reporte += "  No hay calificaciones registradas\n";
            }

            return reporte + "\n";
        }).join("");
    };

    return(<div className={styles.reporteNotasWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Reporte de notas</h3>

            <Card className={styles.notasSection}>
                <Card.Header as="h5">Reporte de notas</Card.Header>
                <Card.Body>
                    <Form.Control
                        as="textarea"
                        rows={5}
                        value={reporteNotas}
                        readOnly
                        className={styles.textArea}
                        placeholder={loading ? "Cargando reporte..." : "El reporte aparecerá aquí"}
                    />
                </Card.Body>
            </Card>

            <Card className={styles.notasCard}>
                <Card.Header as="h5">Reporte de notas</Card.Header>
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
                            name="grupoCurso"
                            value={formData.grupoCurso}
                            onChange={handleChange}
                            placeholder="Escriba el número del grupo"
                        />
                    </Form.Group>

                    <Button
                        onClick={handleObtenerReporte}
                        disabled={loading}
                        className={styles.publicarButton}>
                        {loading ? "Cargando..." : "Obtener reporte"}
                    </Button>
                </Card.Body>
            </Card>

        </div>
    );
}
export default ReporteNotasProfesorPg;