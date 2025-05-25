import React, { useState } from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './RubrosProfesorPg.module.css';
import axios from "axios";

function RubrosProfesorPg() {
    const [formData, setFormData] = useState({
        codigoCurso: "",
        grupoNumero: "",
        rubros: [{
            nombre: "",
            porcentaje: ""
        }]
    });

    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleRubroChange = (index, e) => {
        const { name, value } = e.target;
        const updatedRubros = [...formData.rubros];
        updatedRubros[index][name] = value;

        setFormData(prev => ({
            ...prev,
            rubros: updatedRubros
        }));
    };

    const addRubro = () => {
        setFormData(prev => ({
            ...prev,
            rubros: [...prev.rubros, { nombre: "", porcentaje: "" }]
        }));
    };

    const removeRubro = (index) => {
        const updatedRubros = formData.rubros.filter((_, i) => i !== index);
        setFormData(prev => ({
            ...prev,
            rubros: updatedRubros
        }));
    };

    const handleSubmit = async () => {
        setLoading(true);

        try {
            // Preparar los datos para el backend
            const requestData = {
                course_code: formData.codigoCurso,
                group_number: parseInt(formData.grupoNumero),
                grading_items: formData.rubros.map((rubro, index) => ({
                    grading_item_id: index,
                    name: rubro.nombre,
                    percentage: parseFloat(rubro.porcentaje),
                    group_id: parseInt(formData.grupoNumero)
                }))
            };

            const response = await axios.post(
                "https://localhost:7199/Group/add_grading_items",
                requestData,
                {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                }
            );

            if (response.data.status === "OK") {
                console.log("Rubros agregados exitosamente:", response.data);
                alert("Rubros guardados correctamente");
            } else {
                console.error("Error en la respuesta:", response.data);
                alert("Error al guardar los rubros");
            }
        } catch (err) {
            console.error("Error al enviar los datos:", err);
            alert("Error al conectar con el servidor");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className={styles.rubrosWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Rubros</h3>

            <Card className={styles.rubrosCard}>
                <Card.Header as="h5">Rubros</Card.Header>
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
                            type="number"
                            name="grupoNumero"
                            value={formData.grupoNumero}
                            onChange={handleChange}
                            placeholder="Escriba el número del grupo"
                        />
                    </Form.Group>

                    <div className={styles.rubrosContainer}>
                        {formData.rubros.map((rubro, index) => (
                            <div key={index} className={styles.rubroItem}>
                                <Form.Group className="mb-3">
                                    <Form.Label>Nombre del rubro</Form.Label>
                                    <Form.Control
                                        type="text"
                                        name="nombre"
                                        value={rubro.nombre}
                                        onChange={(e) => handleRubroChange(index, e)}
                                        placeholder="Nombre del rubro"
                                    />
                                </Form.Group>

                                <Form.Group className="mb-3">
                                    <Form.Label>Porcentaje</Form.Label>
                                    <Form.Control
                                        type="number"
                                        name="porcentaje"
                                        value={rubro.porcentaje}
                                        onChange={(e) => handleRubroChange(index, e)}
                                        placeholder="Porcentaje"
                                    />
                                </Form.Group>

                                {formData.rubros.length > 1 && (
                                    <Button
                                        variant="danger"
                                        size="sm"
                                        onClick={() => removeRubro(index)}
                                        className={styles.removeButton}
                                    >
                                        Eliminar
                                    </Button>
                                )}
                            </div>
                        ))}

                        <Button
                            variant="secondary"
                            onClick={addRubro}
                            className={styles.addButton}
                        >
                            Agregar Rubro
                        </Button>
                    </div>

                    <Button
                        onClick={handleSubmit}
                        disabled={loading || !formData.codigoCurso || !formData.grupoNumero}
                        className={styles.rubrosButton}
                    >
                        {loading ? "Guardando..." : "Guardar Rubros"}
                    </Button>
                </Card.Body>
            </Card>
        </div>
    );
}

export default RubrosProfesorPg;