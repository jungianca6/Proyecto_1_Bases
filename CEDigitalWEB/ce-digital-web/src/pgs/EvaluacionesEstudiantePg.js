
import React from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './EstudiantePg.module.css'

function EvaluacionesEstudiantesPg() {
    return <div className={styles.estudianteWrapper}>
        <h1 className={styles.title}>CE Digital</h1>
        <h3 className={styles.subtitle}>Evaluaciones</h3>

        <h3 className={styles.listaEvaluacion}>Estudiante</h3>
        <h3 className={styles.notaEvaluacion}>-/100</h3>


        <Card className={styles.notasSection}>
            <Card.Header as="h5">Notas</Card.Header>
            <Card.Body>
                <Form.Control
                    as="textarea"
                    rows={5}
                    placeholder="Aquí se mostrarán tus calificaciones..."
                    readOnly
                    className={styles.textArea}
                />
            </Card.Body>
        </Card>
    </div>;
}

export default EvaluacionesEstudiantesPg;