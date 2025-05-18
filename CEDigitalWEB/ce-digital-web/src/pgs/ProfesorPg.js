import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './ProfesorPg.module.css';

function ProfesorPg() {

    const navigate = useNavigate();


    return(
        <div className={styles.profesorWrapper}>

            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Profesor</h3>

            {/* Botones de navegación */}
            <Button
                className={styles.cursosButton}>
                Cursos
            </Button>

            <Button
                className={styles.evaluacionesButton}>
                Evaluaciones
            </Button>

            <Button
                className={styles.documentosButton}>
                Documentos
            </Button>

    </div>
    );
  }

  export default ProfesorPg;