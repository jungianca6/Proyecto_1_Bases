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
            const response = await axios.post("https://localhost:7190/MenuInicio/Login", {
                usuario: username,
                contrasena: password
            });

            if (response.data.success) {
                const usuario = response.data.usuario_actual;
                const cuenta = response.data.cuenta_actual;

                setUser(usuario);
                localStorage.setItem("usuario_actual", JSON.stringify(usuario));

                setCuenta(cuenta);
                localStorage.setItem("cuenta_actual", JSON.stringify(cuenta));

                alert("Logeo exitoso");

                switch (usuario.rol) {
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
                alert(response.data.message);
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
                    <h1 className={styles.title}>TecBank</h1>

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
