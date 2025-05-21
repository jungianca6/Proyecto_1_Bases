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
        public IActionResult PostDeleteNew([FromBody] Data_input_manage_news message)
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


            response.status = "OK";
            response.message = "Mensaje Aqui";
            return Ok(response);
        }

        [HttpPost("edit_new")]
        public IActionResult PostEditNew([FromBody] Data_input_manage_news message)
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


            response.status = "OK";
            response.message = "Mensaje Aqui";
            return Ok(response);
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
