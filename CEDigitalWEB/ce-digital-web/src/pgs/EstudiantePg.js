import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import Button from 'react-bootstrap/Button';


function EstudiantePg() {
    return (
        <div
            className="d-flex justify-content-center align-items-center"
            style={{ height: "100vh" }}
        >
            <Button variant="primary">Prueba</Button>
            <Button style={{ backgroundColor: "purple", borderColor: "purple" }}>
                Prueba
            </Button>

        </div>
    );
}

export default EstudiantePg;
