import React, { useState, useEffect } from "react";
import axios from "axios";
import { Card } from 'react-bootstrap';
import styles from './DocumentosEstudiantePg.module.css';

function DocumentosEstudiantePg() {
  const [cuenta, setCuenta] = useState(null);
  const [cursoSeleccionado, setCursoSeleccionado] = useState(null);
  const [documentosPorCarpeta, setDocumentosPorCarpeta] = useState({});

  useEffect(() => {
    const cuentaGuardada = JSON.parse(localStorage.getItem("cuenta_actual"));
    const cursoGuardado = JSON.parse(localStorage.getItem("curso_seleccionado"));

    if (cuentaGuardada) {
      setCuenta(cuentaGuardada);
    }

    if (cursoGuardado) {
      setCursoSeleccionado(cursoGuardado);
    }
  }, []);

  useEffect(() => {
    if (cuenta && cursoSeleccionado) {
      fetchDocumentos();
    }
  }, [cuenta, cursoSeleccionado]);

  const fetchDocumentos = async () => {
    try {
      const response = await axios.post("https://localhost:7199/Document/view_student_documents", {
        student_id: cuenta.primary_key,
        course_code: cursoSeleccionado.course_code,
        group_number: cursoSeleccionado.group_number
      });

      if (response.data.status === "OK") {
        const documentos = response.data.message.documents || [];

        // Filtrar solo los subidos por el profesor
        const documentosFiltrados = documentos.filter(d => d.uploaded_by_professor);

        // Agrupar por folder_name
        const agrupados = {};
        documentosFiltrados.forEach(doc => {
          if (!agrupados[doc.folder_name]) {
            agrupados[doc.folder_name] = [];
          }
          agrupados[doc.folder_name].push(doc);
        });

        setDocumentosPorCarpeta(agrupados);
      } else {
        console.warn("No se pudieron obtener los documentos.");
      }
    } catch (error) {
      console.error("Error al obtener los documentos:", error);
    }
  };

  return (
    <div className={styles.documentosWrapper}>
      <h1 className={styles.title}>CE Digital</h1>
      <h3 className={styles.subtitle}>Documentos</h3>

      {cuenta && (
        <h4 className={styles.infoLinea}>
          Estudiante: {cuenta.username}
        </h4>
      )}

      {cursoSeleccionado && (
        <h4 className={styles.infoLinea}>
          Curso actual: {cursoSeleccionado.course_code}: {cursoSeleccionado.course_name} - Grupo {cursoSeleccionado.group_number}
        </h4>
      )}

      {Object.keys(documentosPorCarpeta).length === 0 ? (
        <p className={styles.textoBlanco}>No hay documentos disponibles.</p>
      ) : (
        Object.entries(documentosPorCarpeta).map(([folder, docs], idx) => (
          <Card key={idx} className={styles.folderCard}>
            <Card.Header as="h5">📁 {folder}</Card.Header>
            <Card.Body>
              {docs.map((doc, i) => (
                <div key={i} className={styles.documentItem}>
                <p><strong>Archivo:</strong> {doc.filename}</p>
                <p><strong>Fecha de subida:</strong> {new Date(doc.upload_date).toLocaleString()}</p>

                <div className={styles.acciones}>
                  {/* Ver archivo en nueva pestaña */}
                  <a
                    href={doc.file_data_base_path}
                    target="_blank"
                    rel="noopener noreferrer"
                    className={styles.linkArchivo}
                  >
                    Ver archivo
                  </a>

                  {/* Descargar archivo */}
                  <a
                    href={doc.file_data_base_path}
                    download={doc.filename}
                    className={styles.linkArchivo}
                    style={{ marginLeft: "1rem" }}
                  >
                    Descargar
                  </a>
                </div>

                <hr />
              </div>

              ))}
            </Card.Body>
          </Card>
        ))
      )}
    </div>
  );
}

export default DocumentosEstudiantePg;
