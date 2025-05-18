import React from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './DocumentosEstudiantePg.module.css';

function DocumentosEstudiantePg() {
    return(<div className={styles.documentosWrapper}>
        <h1 className={styles.title}>CE Digital</h1>
        <h3 className={styles.subtitle}>Documentos</h3>

    </div>
    );
}

export default DocumentosEstudiantePg;