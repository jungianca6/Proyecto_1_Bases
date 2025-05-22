using Azure;
using CEDigital.Data_input_models;
using CEDigital.Data_output_models;
using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CEDigital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private Api_response response;
        public StudentController()
        {
            response = new Api_response("OK", null);  // Inicializando la respuesta por defecto
        }

        [HttpPost("add_student_to_group")]
        public IActionResult PostAddStudentToGroup([FromBody] Data_input_add_student_to_group message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            try
            {
                // 0. Verificar que el estudiante existe
                string checkStudentExistQuery = "SELECT COUNT(*) FROM Student WHERE student_id = @student_id";

                using (SqlCommand checkStudentExistCmd = new SqlCommand(checkStudentExistQuery))
                {
                    checkStudentExistCmd.Parameters.AddWithValue("@student_id", message.student_id);

                    using (SqlDataReader reader = db.Execute_query(checkStudentExistCmd, out connection))
                    {
                        if (reader.Read() && Convert.ToInt32(reader[0]) == 0)
                        {
                            response.status = "ERROR";
                            response.message = "El estudiante no existe.";
                            return Ok(response);
                        }
                        reader.Close();
                    }
                }

                string checkGroupQuery = "SELECT group_id FROM Groups WHERE group_number = @group_number AND course_code = @course_code";
                int groupId = -1;

                using (SqlCommand checkGroupCmd = new SqlCommand(checkGroupQuery))
                {
                    checkGroupCmd.Parameters.AddWithValue("@group_number", message.group_number);
                    checkGroupCmd.Parameters.AddWithValue("@course_code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(checkGroupCmd, out connection))
                    {
                        if (reader.Read())
                        {
                            groupId = Convert.ToInt32(reader["group_id"]);
                        }
                        else
                        {
                            response.status = "ERROR";
                            response.message = "El grupo no existe.";
                            return Ok(response);
                        }

                        reader.Close();
                    }

                    // 2. Verificar si el estudiante ya est� en el grupo
                    string checkStudentQuery = "SELECT COUNT(*) FROM Student_Group WHERE student_id = @student_id AND group_id = @group_id";

                    using (SqlCommand checkStudentCmd = new SqlCommand(checkStudentQuery))
                    {
                        checkStudentCmd.Parameters.AddWithValue("@student_id", message.student_id);
                        checkStudentCmd.Parameters.AddWithValue("@group_id", groupId);

                        using (SqlDataReader reader = db.Execute_query(checkStudentCmd, out connection))
                        {
                            if (reader.Read() && Convert.ToInt32(reader[0]) > 0)
                            {
                                response.status = "ERROR";
                                response.message = "El estudiante ya est� en el grupo.";
                                return Ok(response);
                            }

                            reader.Close();
                        }
                    }

                    // 3. Insertar al estudiante en el grupo
                    string insertQuery = "INSERT INTO Student_Group (student_id, group_id) VALUES (@student_id, @group_id)";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery))
                    {
                        insertCmd.Parameters.AddWithValue("@student_id", message.student_id);
                        insertCmd.Parameters.AddWithValue("@group_id", groupId);

                        db.Execute_non_query(insertCmd);
                    }

                    response.status = "OK";
                    response.message = "Estudiante agregado correctamente al grupo.";
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error en el servidor: " + ex.Message;
                return Ok(response);
            }
        }

        [HttpPost("view_student_courses")]
        public IActionResult PostViewStudentCourses([FromBody] Data_input_view_student_courses message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection = null;

            string query = @"
            SELECT c.course_code, c.name AS course_name, g.group_number
            FROM Student_Group sg
            JOIN Groups g ON sg.group_id = g.group_id
            JOIN Course c ON g.course_code = c.course_code
            WHERE sg.student_id = @student_id
            ";

            try
            {
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Parameters.AddWithValue("@student_id", message.student_id);

                    using (SqlDataReader reader = db.Execute_query(command, out connection))
                    {
                        List<View_student_course> courses = new List<View_student_course>();

                        while (reader.Read())
                        {
                            View_student_course course = new View_student_course
                            {
                                course_code = reader["course_code"].ToString(),
                                course_name = reader["course_name"].ToString(),
                                group_number = Convert.ToInt32(reader["group_number"])
                            };

                            courses.Add(course);
                        }

                        reader.Close();
                        connection.Close();

                        var output = new Data_output_view_student_courses
                        {
                            courses = courses
                        };

                        response.status = "OK";
                        response.message = output;
                        return Ok(response);
                    }
                }
            }
            catch (Exception ex)
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();

                response.status = "ERROR";
                response.message = "Ocurri� un error al obtener los cursos del estudiante: " + ex.Message;
                return StatusCode(500, response);

            }

        }

    }
}
