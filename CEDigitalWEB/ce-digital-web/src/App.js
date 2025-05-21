import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { useState } from "react";
import Login from "./pgs/Login";
import AdminPg from "./pgs/AdminPg";
import EstudiantePg from "./pgs/EstudiantePg";
import ProfesorPg from "./pgs/ProfesorPg";
import DocumentosEstudiantePg from "./pgs/DocumentosEstudiantePg";
import EvaluacionesEstudiantesPg from "./pgs/EvaluacionesEstudiantePg";
import NoticiasProfesorPg from "./pgs/NoticiasProfesorPg";
import DocumentosProfesorPg from "./pgs/DocumentosProfesorPg";
import EvaluacionesProfesorPg from "./pgs/EvaluacionesProfesorPg";
import ReporteNotasProfesorPg from "./pgs/ReporteNotasProfesorPg";
import ReporteEstudianteProfesorPg from "./pgs/ReporteEstudianteProfesorPg";
import RubrosProfesorPg from "./pgs/RubrosProfesorPg";

import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
    const [user, setUser] = useState(null);

    return (
        <Router>
            <Routes>
                <Route path="/login" element={<Login setUser={setUser} />} />
                <Route path="/admin" element={user?.rol === "Admin" ? <AdminPg /> : <Navigate to="/login" />} />
                {/* Rutas del estudiante */}
                <Route path="/estudiante" element={user?.rol === "Student" ? <EstudiantePg /> : <Navigate to="/login" />} />
                <Route path="/estudiante/documentos" element={user?.rol === "Student" ? <DocumentosEstudiantePg /> :
                    <Navigate to="/login" />} />
                <Route path="/estudiante/evaluaciones" element={user?.rol === "Student" ? <EvaluacionesEstudiantesPg /> :
                    <Navigate to="/login" />} />
                {/* Rutas del profesor */}
                <Route path="/profesor" element={user?.rol === "Professor" ? <ProfesorPg /> : <Navigate to="/login" />} />
                <Route path="/profesor/noticias" element={user?.rol === "Professor" ? <NoticiasProfesorPg /> :
                    <Navigate to="/login" />} />
                <Route path="/profesor/documentos" element={user?.rol === "Professor" ? <DocumentosProfesorPg /> :
                    <Navigate to="/login" />} />
                <Route path="/profesor/evaluaciones" element={user?.rol === "Professor" ? <EvaluacionesProfesorPg /> :
                    <Navigate to="/login" />} />
                <Route path="/profesor/reportenotas" element={user?.rol === "Professor" ? <ReporteNotasProfesorPg /> :
                    <Navigate to="/login" />} />
                <Route path="/profesor/reporteEstudiantes" element={user?.rol === "Professor" ? <ReporteEstudianteProfesorPg /> :
                    <Navigate to="/login" />} />
                <Route path="/profesor/rubros" element={user?.rol === "Professor" ? <RubrosProfesorPg /> :
                    <Navigate to="/login" />} />

                <Route path="*" element={<Navigate to="/login" />} />
            </Routes>
        </Router>
    );
}
export default App;