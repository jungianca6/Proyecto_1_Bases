import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { useState } from "react";
import Login from "./pgs/Login";
import AdminPg from "./pgs/AdminPg";
import EstudiantePg from "./pgs/EstudiantePg";
import ProfesorPg from "./pgs/ProfesorPg";
import DocumentosEstudiantePg from "./pgs/DocumentosEstudiantePg";
import EvaluacionesEstudiantesPg from "./pgs/EvaluacionesEstudiantePg";
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
    const [user, setUser] = useState(null);

    return (
        <Router>
            <Routes>
                <Route path="/login" element={<Login setUser={setUser} />} />
                <Route path="/admin" element={user?.rol === "Admin" ? <AdminPg /> : <Navigate to="/login" />} />
                {/* Rutas del estudiante */}
                <Route path="/estudiante" element={user?.rol === "Estudiante" ? <EstudiantePg /> : <Navigate to="/login" />} />
                <Route path="/estudiante/documentos" element={user?.rol === "Estudiante" ? <DocumentosEstudiantePg /> :
                    <Navigate to="/login" />} />
                <Route path="/estudiante/evaluaciones" element={user?.rol === "Estudiante" ? <EvaluacionesEstudiantesPg /> :
                    <Navigate to="/login" />} />

                <Route path="/profesor" element={user?.rol === "Profesor" ? <ProfesorPg /> : <Navigate to="/login" />} />
                <Route path="*" element={<Navigate to="/login" />} />
            </Routes>
        </Router>
    );
}
export default App;
