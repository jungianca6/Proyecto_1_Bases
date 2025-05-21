import React, { useState, useEffect } from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './DocumentosEstudiantePg.module.css';

function DocumentosEstudiantePg() {

    const [cuenta, setCuenta] = useState(null);

    useEffect(() => {
        const cuentaGuardada = JSON.parse(localStorage.getItem("cuenta_actual"));
        if (cuentaGuardada) {
            setCuenta(cuentaGuardada);
            console.log("Datos de cuenta:", cuentaGuardada);
        }
    }, []);


    return(<div className={styles.documentosWrapper}>
        <h1 className={styles.title}>CE Digital</h1>
        <h3 className={styles.subtitle}>Documentos</h3>

    </div>
    );
}

export default DocumentosEstudiantePg;