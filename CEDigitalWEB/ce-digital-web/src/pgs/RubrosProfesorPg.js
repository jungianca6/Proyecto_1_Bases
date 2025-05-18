import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import { Button, Card, Form } from 'react-bootstrap';
import styles from './RubrosProfesorPg.module.css';

function RubrosProfesorPg() {
    return(<div className={styles.rubrosWrapper}>
            <h1 className={styles.title}>CE Digital</h1>
            <h3 className={styles.subtitle}>Rubros</h3>

        </div>
    );
}
export default RubrosProfesorPg;