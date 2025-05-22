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
    public class CourseController : ControllerBase
    {

        private Api_response response;

        public CourseController()
        {
            response = new Api_response("OK", null);  // Inicializando la respuesta por defecto
        }

        // Petición GET
        [HttpGet("GET")]
        public ActionResult<Api_response> Get_Example()
        {
            response.status = "OK";
            response.message = "This is a GET request.";
            return Ok(response);
        }

        [HttpPost("create_course")]
        public IActionResult PostCreateCourse([FromBody] Data_input_admin_create_course message)
        {
            SQL_connection db = new SQL_connection();
            string checkQuery = "SELECT COUNT(*) FROM Course WHERE course_code = @code";
            string insertQuery = "INSERT INTO Course (course_code, name, credits, career) VALUES (@code, @name, @credits, @career)";

            SqlConnection connection;

            try
            {
                // Validar si el código ya existe
                using (SqlCommand checkCommand = new SqlCommand(checkQuery))
                {
                    checkCommand.Parameters.AddWithValue("@code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(checkCommand, out connection))
                    {
                        if (reader.Read() && Convert.ToInt32(reader[0]) > 0)
                        {
                            response.status = "ERROR";
                            response.message = "El código del curso ya existe.";
                            return Ok(response);
                        }

                        reader.Close();
                    }
                }

                // Insertar nuevo curso
                using (SqlCommand insertCommand = new SqlCommand(insertQuery))
                {
                    insertCommand.Parameters.AddWithValue("@code", message.course_code);
                    insertCommand.Parameters.AddWithValue("@name", message.name);
                    insertCommand.Parameters.AddWithValue("@credits", message.credits);
                    insertCommand.Parameters.AddWithValue("@career", message.career);

                    db.Execute_non_query(insertCommand);
                }

                response.status = "OK";
                response.message = "Curso creado exitosamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al crear el curso: " + ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPost("disable_course")]
        public IActionResult PostDeleteCourse([FromBody] Data_input_admin_disable_course message)
        {
            SQL_connection db = new SQL_connection();

            string checkQuery = "SELECT COUNT(*) FROM Course WHERE course_code = @code";
            string deleteQuery = "DELETE FROM Course WHERE course_code = @code";

            SqlConnection connection;

            try
            {
                // Validar si el código existe
                using (SqlCommand checkCommand = new SqlCommand(checkQuery))
                {
                    checkCommand.Parameters.AddWithValue("@code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(checkCommand, out connection))
                    {
                        if (reader.Read() && Convert.ToInt32(reader[0]) == 0)
                        {
                            response.status = "ERROR";
                            response.message = "El código del curso no existe.";
                            return Ok(response);
                        }
                        reader.Close();
                    }
                }

                // Eliminar el curso
                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery))
                {
                    deleteCommand.Parameters.AddWithValue("@code", message.course_code);
                    db.Execute_non_query(deleteCommand);
                }

                response.status = "OK";
                response.message = "Curso eliminado exitosamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al eliminar el curso: " + ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPost("view_course")]
        public IActionResult PostViewCourse([FromBody] Data_input_admin_view_course message)
        {
            SQL_connection db = new SQL_connection();

            // Consulta sin filtrar por IsActive porque no existe esa columna
            string query = "SELECT course_code, name, credits, career FROM Course WHERE course_code = @code";

            SqlConnection connection;

            try
            {
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Parameters.AddWithValue("@code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(command, out connection))
                    {
                        if (reader.Read())
                        {
                            var course = new Data_output_admin_view_course()
                            {
                                course_code = reader["course_code"].ToString(),
                                name = reader["name"].ToString(),
                                credits = Convert.ToInt32(reader["credits"]),
                                career = reader["career"].ToString(),
                                group_ids = new List<int>(),      // Aquí puedes cargar info si tienes
                                professors_ids = new List<int>()  // Igual si tienes info
                            };

                            response.status = "OK";
                            response.message = course;
                            return Ok(response);
                        }
                        else
                        {
                            response.status = "ERROR";
                            response.message = "No se encontró el curso con el código especificado.";
                            return NotFound(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al obtener el curso: " + ex.Message;
                return StatusCode(500, response);
            }
        }

    }
}
