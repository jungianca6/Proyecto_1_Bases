import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React, {useState} from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './ReporteNotasProfesorPg.module.css';

function ReporteNotasProfesorPg() {

    const [formData, setFormData] = useState({
        codigoCurso: "",
        grupoCurso: "",
        studentID: "",
    });

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
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
                        placeholder="Reporte de notas"
                        readOnly
                        className={styles.textArea}
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
                            rows={3}
                            name="grupoCurso"
                            value={formData.grupoCurso}
                            onChange={handleChange}
                            placeholder="Escriba el número del grupo"
                        />
                    </Form.Group>

                    <Form.Group className="mb-3">
                        <Form.Label>ID del estudiante</Form.Label>
                        <Form.Control
                            type="text"
                            rows={3}
                            name="studentID"
                            value={formData.studentID}
                            onChange={handleChange}
                            placeholder="Escriba el ID del estudiante"
                        />
                    </Form.Group>

                    <Button
                        className={styles.publicarButton}>
                        Obtener reporte
                    </Button>
                </Card.Body>
            </Card>


        </div>
    );
}
export default ReporteNotasProfesorPg;