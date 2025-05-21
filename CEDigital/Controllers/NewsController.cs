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
    public class NewsController : ControllerBase
    {

        private Api_response response;

        public NewsController()
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

        [HttpPost("add_new")]
        public IActionResult PostAddNew([FromBody] Data_input_manage_news message)
        {
            SQL_connection db = new SQL_connection();

            string checkNewsQuery = "SELECT COUNT(*) FROM News WHERE title = @title AND course_code = @course_code";
            string checkCourseQuery = "SELECT COUNT(*) FROM Course WHERE course_code = @course_code";
            string insertQuery = "INSERT INTO News (title, message, publication_date, author, course_code) VALUES (@title, @message, @date, @author, @course)";

            SqlConnection connection;

            try
            {
                // 1. Validar que el curso exista
                using (SqlCommand checkCourseCommand = new SqlCommand(checkCourseQuery))
                {
                    checkCourseCommand.Parameters.AddWithValue("@course_code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(checkCourseCommand, out connection))
                    {
                        if (reader.Read() && Convert.ToInt32(reader[0]) == 0)
                        {
                            response.status = "ERROR";
                            response.message = "El código del curso especificado no existe.";
                            return Ok(response);
                        }

                        reader.Close();
                    }
                }

                // 2. Verificar si ya existe una noticia con ese título para ese curso
                using (SqlCommand checkNewsCommand = new SqlCommand(checkNewsQuery))
                {
                    checkNewsCommand.Parameters.AddWithValue("@title", message.title);
                    checkNewsCommand.Parameters.AddWithValue("@course_code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(checkNewsCommand, out connection))
                    {
                        if (reader.Read() && Convert.ToInt32(reader[0]) > 0)
                        {
                            response.status = "ERROR";
                            response.message = "Ya existe una noticia con este título para el curso especificado.";
                            return Ok(response);
                        }

                        reader.Close();
                    }
                }

                // 3. Insertar nueva noticia
                using (SqlCommand insertCommand = new SqlCommand(insertQuery))
                {
                    insertCommand.Parameters.AddWithValue("@title", message.title);
                    insertCommand.Parameters.AddWithValue("@message", message.message);
                    insertCommand.Parameters.AddWithValue("@date", message.publication_date);
                    insertCommand.Parameters.AddWithValue("@author", message.author);
                    insertCommand.Parameters.AddWithValue("@course", message.course_code);

                    db.Execute_non_query(insertCommand);
                }

                response.status = "OK";
                response.message = "Noticia creada exitosamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al crear la noticia: " + ex.Message;
                return StatusCode(500, response);
            }
        }


        [HttpPost("delete_new")]
        public IActionResult PostDeleteNew([FromBody] Data_input_delete_news message)
        {
            SQL_connection db = new SQL_connection();

            string checkQuery = "SELECT COUNT(*) FROM News WHERE title = @title AND course_code = @course_code";
            string deleteQuery = "DELETE FROM News WHERE title = @title AND course_code = @course_code";

            SqlConnection connection;

            try
            {
                // 1. Verificar si existe la noticia
                using (SqlCommand checkCommand = new SqlCommand(checkQuery))
                {
                    checkCommand.Parameters.AddWithValue("@title", message.title);
                    checkCommand.Parameters.AddWithValue("@course_code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(checkCommand, out connection))
                    {
                        if (reader.Read() && Convert.ToInt32(reader[0]) == 0)
                        {
                            response.status = "ERROR";
                            response.message = "No se encontró ninguna noticia con ese título y código de curso.";
                            return Ok(response);
                        }

                        reader.Close();
                    }
                }

                // 2. Eliminar la noticia
                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery))
                {
                    deleteCommand.Parameters.AddWithValue("@title", message.title);
                    deleteCommand.Parameters.AddWithValue("@course_code", message.course_code);

                    db.Execute_non_query(deleteCommand);
                }

                response.status = "OK";
                response.message = "Noticia eliminada exitosamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al eliminar la noticia: " + ex.Message;
                return StatusCode(500, response);
            }
        }


        [HttpPost("edit_new")]
        public IActionResult PostEditNew([FromBody] Data_input_edit_news message)
        {
            SQL_connection db = new SQL_connection();

            string checkQuery = "SELECT COUNT(*) FROM News WHERE title = @title AND course_code = @course_code";
            string updateQuery = "UPDATE News SET message = @new_message, title = @new_title WHERE title = @title AND course_code = @course_code";

            SqlConnection connection;

            try
            {
                // 1. Verificar que exista la noticia original
                using (SqlCommand checkCommand = new SqlCommand(checkQuery))
                {
                    checkCommand.Parameters.AddWithValue("@title", message.title);
                    checkCommand.Parameters.AddWithValue("@course_code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(checkCommand, out connection))
                    {
                        if (reader.Read() && Convert.ToInt32(reader[0]) == 0)
                        {
                            response.status = "ERROR";
                            response.message = "No se encontró ninguna noticia con ese título y código de curso.";
                            return Ok(response);
                        }

                        reader.Close();
                    }
                }

                // 2. Determinar el nuevo título
                string newTitle = string.IsNullOrWhiteSpace(message.change_title) ? message.title : message.change_title;

                // 3. Actualizar la noticia
                using (SqlCommand updateCommand = new SqlCommand(updateQuery))
                {
                    updateCommand.Parameters.AddWithValue("@new_message", message.change_new);
                    updateCommand.Parameters.AddWithValue("@new_title", newTitle);
                    updateCommand.Parameters.AddWithValue("@title", message.title);
                    updateCommand.Parameters.AddWithValue("@course_code", message.course_code);

                    db.Execute_non_query(updateCommand);
                }

                response.status = "OK";
                response.message = "Noticia actualizada exitosamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al editar la noticia: " + ex.Message;
                return StatusCode(500, response);
            }
        }


        [HttpPost("view_news")]
        public IActionResult PostView([FromBody] Data_input_view_news message)
        {
            /*
             * #######Logica para verificar si el codigo no existe con la informacion del SQL Y MongoDBB#######
             */


            /*
             * #######Envio de la respuesta#######
             * 
             * En caso postivo enviar Ok
             * En caso negativo enviar el error corrspondiente
             * 
             */

            Data_output_view_news data_Output_View_News = new Data_output_view_news();
             
            response.status = "OK";
            response.message = data_Output_View_News;
            return Ok(response);
        }


    }
}
