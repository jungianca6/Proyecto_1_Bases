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
    public class GroupController : ControllerBase
    {

        private Api_response response;

        public GroupController()
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

        [HttpPost("create_group")]
        public IActionResult PostCreateGruop([FromBody] Data_input_add_group message)
        {
            SQL_connection db = new SQL_connection();

            string checkCourseQuery = "SELECT COUNT(*) FROM Course WHERE course_code = @code";
            string getMaxGroupQuery = "SELECT ISNULL(MAX(group_number), 0) FROM Groups WHERE course_code = @code";
            string insertGroupQuery = "INSERT INTO Groups (course_code, group_number) VALUES (@code, @group_number)";

            SqlConnection connection;

            try
            {
                // Verificar que el curso exista
                using (SqlCommand checkCmd = new SqlCommand(checkCourseQuery))
                {
                    checkCmd.Parameters.AddWithValue("@code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(checkCmd, out connection))
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

                int nextGroupNumber = 1;

                // Obtener el número de grupo más alto para ese curso
                using (SqlCommand getMaxCmd = new SqlCommand(getMaxGroupQuery))
                {
                    getMaxCmd.Parameters.AddWithValue("@code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(getMaxCmd, out connection))
                    {
                        if (reader.Read())
                        {
                            nextGroupNumber = Convert.ToInt32(reader[0]) + 1;
                        }

                        reader.Close();
                    }
                }

                // Insertar nuevo grupo con el número siguiente
                using (SqlCommand insertCmd = new SqlCommand(insertGroupQuery))
                {
                    insertCmd.Parameters.AddWithValue("@code", message.course_code);
                    insertCmd.Parameters.AddWithValue("@group_number", nextGroupNumber);

                    db.Execute_non_query(insertCmd);
                }

                response.status = "OK";
                response.message = $"Grupo {nextGroupNumber} creado exitosamente para el curso {message.course_code}.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al crear el grupo: " + ex.Message;
                return StatusCode(500, response);
            }
        }




    }
}
