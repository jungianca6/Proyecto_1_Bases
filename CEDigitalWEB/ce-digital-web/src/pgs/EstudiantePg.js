import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import { Button } from 'react-bootstrap';
import styles from './EstudiantePg.module.css';

function EstudiantePg() {
    const handleButton1Click = () => {
        alert("Botón en (255, 100) clickeado");
    };

    const handleButton2Click = () => {
        alert("Botón púrpura clickeado!");
    };

    return (
        <div className={styles.container}>
            <Button
                variant="primary"
                className={styles.customButton1}
                onClick={handleButton1Click}>
                Botón 1
            </Button>

            <Button
                className={styles.customButton2}
                onClick={handleButton2Click}>
                Botón 2
            </Button>
        </div>
    );
}

export default EstudiantePg;