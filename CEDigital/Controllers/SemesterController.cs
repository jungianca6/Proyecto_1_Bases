using Azure;
using CEDigital.Data_input_models;
using CEDigital.Data_output_models;
using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CEDigital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SemesterController : ControllerBase
    {
        private Api_response response;
        public SemesterController()
        {
            response = new Api_response("OK", null);  // Inicializando la respuesta por defecto
        }

        [HttpPost("initialize_semester")]
        public IActionResult PostInitializeSemester([FromBody] Data_input_initialize_semester message)
        {
            SQL_connection db = new SQL_connection();

            string checkQuery = "SELECT COUNT(*) FROM Semester WHERE year = @year AND period = @period";
            string insertQuery = "INSERT INTO Semester (year, period) VALUES (@year, @period)";

            SqlConnection connection;

            try
            {
                // 1. Verificar si el semestre ya existe
                using (SqlCommand checkCommand = new SqlCommand(checkQuery))
                {
                    checkCommand.Parameters.AddWithValue("@year", message.year);
                    checkCommand.Parameters.AddWithValue("@period", message.period);

                    using (SqlDataReader reader = db.Execute_query(checkCommand, out connection))
                    {
                        if (reader.Read() && Convert.ToInt32(reader[0]) > 0)
                        {
                            response.status = "ERROR";
                            response.message = "El semestre ya existe.";
                            return Ok(response);
                        }

                        reader.Close();
                    }
                }

                // 2. Insertar nuevo semestre
                using (SqlCommand insertCommand = new SqlCommand(insertQuery))
                {
                    insertCommand.Parameters.AddWithValue("@year", message.year);
                    insertCommand.Parameters.AddWithValue("@period", message.period);

                    db.Execute_non_query(insertCommand);
                }

                response.status = "OK";
                response.message = "Semestre inicializado exitosamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al inicializar el semestre: " + ex.Message;
                return StatusCode(500, response);
            }
        }
        [HttpPost("add_course_to_semester")]
        public IActionResult PostAddCourseToSemester([FromBody] Data_input_add_course_to_semester message)
        {
            SQL_connection db = new SQL_connection();

            string checkCourseQuery = "SELECT COUNT(*) FROM Course WHERE course_code = @code";
            string getSemesterIdQuery = "SELECT semester_id FROM Semester WHERE year = @year AND period = @period";
            string checkExistenceQuery = "SELECT COUNT(*) FROM Course_Semester WHERE course_code = @code AND semester_id = @semester_id";
            string insertQuery = "INSERT INTO Course_Semester (course_code, semester_id) VALUES (@code, @semester_id)";

            SqlConnection connection;

            try
            {
                // 1. Verificar si el curso existe
                using (SqlCommand checkCourseCmd = new SqlCommand(checkCourseQuery))
                {
                    checkCourseCmd.Parameters.AddWithValue("@code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(checkCourseCmd, out connection))
                    {
                        if (reader.Read() && Convert.ToInt32(reader[0]) == 0)
                        {
                            response.status = "ERROR";
                            response.message = "El curso no existe.";
                            return Ok(response);
                        }

                        reader.Close();
                    }
                }

                int semesterId = -1;

                // 2. Obtener el semester_id correspondiente
                using (SqlCommand getSemesterCmd = new SqlCommand(getSemesterIdQuery))
                {
                    getSemesterCmd.Parameters.AddWithValue("@year", message.year);
                    getSemesterCmd.Parameters.AddWithValue("@period", message.period);

                    using (SqlDataReader reader = db.Execute_query(getSemesterCmd, out connection))
                    {
                        if (reader.Read())
                        {
                            semesterId = Convert.ToInt32(reader["semester_id"]);
                        }
                        else
                        {
                            response.status = "ERROR";
                            response.message = "El semestre no existe.";
                            return Ok(response);
                        }

                        reader.Close();
                    }
                }

                // 3. Verificar si ya existe esa relaci�n
                using (SqlCommand checkExistCmd = new SqlCommand(checkExistenceQuery))
                {
                    checkExistCmd.Parameters.AddWithValue("@code", message.course_code);
                    checkExistCmd.Parameters.AddWithValue("@semester_id", semesterId);

                    using (SqlDataReader reader = db.Execute_query(checkExistCmd, out connection))
                    {
                        if (reader.Read() && Convert.ToInt32(reader[0]) > 0)
                        {
                            response.status = "ERROR";
                            response.message = "El curso ya est� asignado a ese semestre.";
                            return Ok(response);
                        }

                        reader.Close();
                    }
                }

                // 4. Insertar en Course_Semester
                using (SqlCommand insertCmd = new SqlCommand(insertQuery))
                {
                    insertCmd.Parameters.AddWithValue("@code", message.course_code);
                    insertCmd.Parameters.AddWithValue("@semester_id", semesterId);

                    db.Execute_non_query(insertCmd);
                }

                response.status = "OK";
                response.message = "Curso asignado al semestre exitosamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al asignar curso al semestre: " + ex.Message;
                return StatusCode(500, response);
            }
        }



    }

}
