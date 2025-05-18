import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { FaRegUser } from "react-icons/fa";
import { MdLockOutline } from "react-icons/md";
import React from "react";
import axios from "axios";
import styles from './Login.module.css';

function Login({ setUser }) {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [cuenta, setCuenta] = useState(null);
    const [showPassword, setShowPassword] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        const cuentaGuardada = JSON.parse(localStorage.getItem("cuenta_actual"));
        if (cuentaGuardada) {
            setCuenta(cuentaGuardada);
        }
    }, []);

    const handleLogin = async (e) => {
        e.preventDefault();

        // Logins locales de prueba
        if (username === "admin" && password === "admin123") {
            const adminUser = {
                nombre: "Admin",
                usuario: "admin",
                contrasena: "admin123",
                rol: "Admin"
            };
            setUser(adminUser);
            localStorage.setItem("usuario_actual", JSON.stringify(adminUser));
            navigate("/admin");
            return;
        }

        if (username === "profesor" && password === "profesor123") {
            const profesorUser = {
                nombre: "Profesor Prueba",
                usuario: "profesor",
                contrasena: "profesor123",
                rol: "Profesor"
            };
            setUser(profesorUser);
            localStorage.setItem("usuario_actual", JSON.stringify(profesorUser));
            navigate("/profesor");
            return;
        }

        if (username === "estudiante" && password === "estudiante123") {
            const estudianteUser = {
                nombre: "Estudiante Prueba",
                usuario: "estudiante",
                contrasena: "estudiante123",
                rol: "Estudiante"
            };
            setUser(estudianteUser);
            localStorage.setItem("usuario_actual", JSON.stringify(estudianteUser));
            navigate("/estudiante");
            return;
        }

        // Login real con backend
        try {
        const requestData = {
            data_input_login: {
                username: username,
                password: password,
                user_type: null,
                primary_key: null
            }
        };

        const response = await axios.post("https://localhost:7190/MenuInicio/Login", requestData);

        if (response.data.status === "OK") {
            const data = response.data.message.data_output_login;

            const usuario = {
                nombre: data.username,
                usuario: data.username,
                contrasena: data.password,
                rol: data.user_type
            };

            setUser(usuario);
            localStorage.setItem("usuario_actual", JSON.stringify(usuario));

            setCuenta(data);
            localStorage.setItem("cuenta_actual", JSON.stringify(data));

            alert("Logeo exitoso");

            switch (data.user_type) {
                case "Admin":
                    navigate("/admin");
                    break;
                case "Estudiante":
                    navigate("/estudiante");
                    break;
                case "Profesor":
                    navigate("/profesor");
                    break;
                default:
                    alert("Rol desconocido");
                    break;
            }
        } else {
            alert("Login fallido: " + (response.data.message || "Respuesta no válida del servidor"));
        }
    } catch (error) {
        console.error(error);
        alert("Error al conectar con el servidor");
    }
};

    return (
        <div className={styles.loginWrapper}>
            <div className={styles.wrapper}>
                <form onSubmit={handleLogin}>
                    <h1 className={styles.title}>CE Digital</h1>

                    <div className={styles.inputBox}>
                        <input type="text" placeholder="Usuario" value={username} onChange={(e) => setUsername(e.target.value)} required />
                        <FaRegUser className={styles.icon} />
                    </div>

                    <div className={styles.inputBox}>
                    <input
                        type={showPassword ? "text" : "password"}
                        placeholder="Contraseña"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                        <MdLockOutline className={styles.icon} />
                    </div>
                    
                    <div className={styles.checkboxContainer}>
                        <input
                            type="checkbox"
                            id="showPassword"
                            checked={showPassword}
                            onChange={(e) => setShowPassword(e.target.checked)}
                            className={styles.checkbox}
                        />
                        <label htmlFor="showPassword" className={styles.checkboxLabel}>Mostrar contraseña</label>
                    </div>

                    <button type="submit" className={styles.button}>Ingresar</button>
                </form>
            </div>
        </div>
    );
}

export default Login;
