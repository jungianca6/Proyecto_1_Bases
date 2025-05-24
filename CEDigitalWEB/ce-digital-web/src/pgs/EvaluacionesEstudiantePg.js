import React, { useState, useEffect } from "react";
import axios from "axios";
import styles from './EvaluacionesEstudiantePg.module.css';

function EvaluacionesEstudiantesPg() {
  const [cuenta, setCuenta] = useState(null);
  const [evaluaciones, setEvaluaciones] = useState([]);
  const [archivos, setArchivos] = useState({});
  const [entregasCompletadas, setEntregasCompletadas] = useState({});
  const [cursoSeleccionado, setCursoSeleccionado] = useState(null);

  useEffect(() => {
    const cuentaGuardada = JSON.parse(localStorage.getItem("cuenta_actual"));
    const cursoGuardado = JSON.parse(localStorage.getItem("curso_seleccionado"));

    if (cuentaGuardada) {
      setCuenta(cuentaGuardada);
    }

    if (cursoGuardado) {
      setCursoSeleccionado(cursoGuardado);
      console.log("Curso activo:", cursoGuardado);
    } else {
      alert("Por favor selecciona un curso desde el menú principal.");
    }
  }, []);

  useEffect(() => {
    if (cuenta && cursoSeleccionado) {
      fetchEvaluaciones(cuenta.primary_key);
    }
  }, [cuenta, cursoSeleccionado]);

  const fetchEvaluaciones = async (studentId) => {
    const payload = {
      student_id: studentId,
      course_code: cursoSeleccionado.course_code,
      group_number: cursoSeleccionado.group_number
    };

    try {
      const response = await axios.post("https://localhost:7199/Evaluation/view_student_evaluations", payload);

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
      const contenidoBase64 = e.target.result.split(',')[1];

      const payload = {
        student_id: cuenta.primary_key,
        course_code: cursoSeleccionado.course_code,
        group_number: cursoSeleccionado.group_number,
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

      <div className={styles.notasSection}>
        <h4 className={styles.sectionTitle}>Evaluaciones</h4>

        {evaluaciones.length === 0 ? (
          <p className={styles.textoBlanco}>No hay evaluaciones públicas disponibles.</p>
        ) : (
          evaluaciones.map((eva, index) => {
            const yaEntregado = eva.data_base_path_evalution && eva.evaluation_filename;
            const vencida = new Date(eva.evaluation_date) < new Date();

            return (
              <div key={index} className={styles.evaluacionItem}>
                <h5>{eva.evaluation_title} ({eva.rubric_name})</h5>
                <p><strong>Nota:</strong> {eva.grade}/100</p>
                <p><strong>Comentarios:</strong> {eva.feedback}</p>
                <p><strong>Fecha límite:</strong> {new Date(eva.evaluation_date).toLocaleDateString()}</p>

                <p>
                  <strong>Instrucciones:</strong>{" "}
                  {eva.data_base_path_professor && eva.professor_filename ? (
                    <a
                      href={eva.data_base_path_professor}
                      download={eva.professor_filename}
                      target="_blank"
                      rel="noopener noreferrer"
                      className={styles.linkArchivo}
                    >
                      Descargar {eva.professor_filename}
                    </a>
                  ) : "No disponibles"}
                </p>

                {yaEntregado ? (
                  <p>
                    <strong>Archivo entregado:</strong>{" "}
                    <a
                      href={eva.data_base_path_evalution}
                      download={eva.evaluation_filename}
                      target="_blank"
                      rel="noopener noreferrer"
                      className={styles.linkArchivo}
                    >
                      {eva.evaluation_filename}
                    </a>
                  </p>
                ) : vencida ? (
                  <p style={{ color: "orange", marginTop: "0.5rem" }}>
                    ⏳ Fecha límite vencida. No se puede realizar la entrega.
                  </p>
                ) : (
                  <>
                    <label htmlFor={`archivo-${index}`}>Entregar archivo:</label>
                    <input
                      id={`archivo-${index}`}
                      type="file"
                      className={styles.inputArchivo}
                      onChange={(e) => handleFileChange(e, eva.evaluation_title)}
                    />
                    <button
                      className={styles.entregarButton}
                      onClick={() => handleSubmitArchivo(eva.evaluation_title)}
                      disabled={!archivos[eva.evaluation_title]}
                    >
                      Enviar entrega
                    </button>
                  </>
                )}

                {entregasCompletadas[eva.evaluation_title] && (
                  <p style={{ color: "lightgreen", marginTop: "0.5rem" }}>
                    ✅ Entrega enviada correctamente.
                  </p>
                )}

                <hr />
              </div>
            );
          })
        )}
      </div>
    </div>
  );
}

export default EvaluacionesEstudiantesPg;
