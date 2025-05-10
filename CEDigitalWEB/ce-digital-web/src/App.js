import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { useState } from "react";
import Login from "./pgs/Login";
import AdminPg from "./pgs/AdminPg";
import EstudiantePg from "./pgs/EstudiantePg";
import ProfesorPg from "./pgs/ProfesorPg";

function App() {
    const [user, setUser] = useState({ rol: "Estudiante" }); // <- Mock para desarrollo

    return (
        <Router>
            <Routes>
                <Route path="/login" element={<Login setUser={setUser} />} />
                <Route path="/admin" element={user?.rol === "Admin" ? <AdminPg /> : <Navigate to="/login" />} />
                <Route path="/estudiante" element={user?.rol === "Estudiante" ? <EstudiantePg /> : <Navigate to="/login" />} />
                <Route path="/profesor" element={user?.rol === "Profesor" ? <ProfesorPg /> : <Navigate to="/login" />} />
                <Route path="*" element={<Navigate to="/login" />} />
            </Routes>
        </Router>
    );
}
export default App;
