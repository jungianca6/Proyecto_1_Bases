import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './DocumentosProfesorPg.module.css';

function DocumentosProfesorPg() {
    return(<div className={styles.documentosProfeWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Documentos</h3>

        </div>
    );
}
export default DocumentosProfesorPg;