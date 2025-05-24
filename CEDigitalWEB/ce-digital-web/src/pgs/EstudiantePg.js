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
    const [noticias, setNoticias] = useState([]); 
    const [cursos, setCursos] = useState([]);
    const [cursoSeleccionado, setCursoSeleccionado] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
  try {
    const cuentaRaw = localStorage.getItem("cuenta_actual");
    const cursoRaw = localStorage.getItem("curso_seleccionado");

    const cuentaGuardada = cuentaRaw && cuentaRaw !== "undefined" ? JSON.parse(cuentaRaw) : null;
    const cursoGuardado = cursoRaw && cursoRaw !== "undefined" ? JSON.parse(cursoRaw) : null;

    if (cuentaGuardada) {
      setCuenta(cuentaGuardada);
      console.log("Datos de cuenta:", cuentaGuardada);
      fetchNoticias(cuentaGuardada.primary_key);
      fetchCursos(cuentaGuardada.primary_key);
    }

    if (cursoGuardado) {
      setCursoSeleccionado(cursoGuardado);
    }

  } catch (error) {
    console.error("Error leyendo localStorage:", error);
    alert("Ocurrió un error al cargar la información guardada. Por favor, reinicie sesión.");
  }
}, []);

    const fetchCursos = async (studentId) => {
    try {
        const response = await axios.post("https://localhost:7199/Student/view_student_courses", {
            student_id: studentId
        });

        if (response.data.status === "success" || response.data.status === "OK") {
            setCursos(response.data.message.courses || []);
            console.log("Cursos del estudiante:", response.data.message.courses);
        } else {
            console.warn("No se pudieron obtener cursos.");
        }
    } catch (error) {
        console.error("Error al obtener cursos:", error);
    }
};

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

    const handleCursoSeleccionado = (e) => {
        const index = e.target.value;
        const curso = cursos[index];
        setCursoSeleccionado(curso);
        localStorage.setItem("curso_seleccionado", JSON.stringify(curso));
        console.log("Curso seleccionado:", curso);
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

            {/* Dropdown de cursos */}
            {cursos.length > 0 && (
    <div className={styles.cursoDropdownWrapper}>
        <label htmlFor="cursosSelect" className={styles.cursoDropdownLabel}>Seleccionar Curso:</label>
        <select
            id="cursosSelect"
            className={styles.cursoDropdownSelect}
            value={
                cursoSeleccionado
                    ? cursos.findIndex(c => c.course_code === cursoSeleccionado.course_code && c.group_number === cursoSeleccionado.group_number)
                    : ""
            }
            onChange={handleCursoSeleccionado}
        >
            <option value="">-- Selecciona un curso --</option>
            {cursos.map((curso, index) => (
                <option key={index} value={index}>
                    {curso.course_code}: {curso.course_name} Grupo: {curso.group_number}
                </option>
            ))}
        </select>
    </div>
)}


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
                className={styles.reporteNotasButton}
                onClick={() => navigate('/estudiante/reportenotas')}>
                Reporte de Notas
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
