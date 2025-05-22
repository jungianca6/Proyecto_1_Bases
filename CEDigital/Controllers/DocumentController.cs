using CEDigital.Data_input_models;
using CEDigital.Data_output_models;
using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CEDigital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {

        private Api_response response;

        public DocumentController()
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

        [HttpPost("add_document")]
        public IActionResult PostAddDocument([FromBody] Data_input_add_document message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            try
            {
                // 1. Verificación en SQL: curso existe
                string checkCourseQuery = "SELECT COUNT(*) FROM Course WHERE course_code = @code";
                using (SqlCommand checkCmd = new SqlCommand(checkCourseQuery))
                {
                    checkCmd.Parameters.AddWithValue("@code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(checkCmd, out connection))
                    {
                        if (reader.Read() && Convert.ToInt32(reader[0]) == 0)
                        {
                            response.status = "ERROR";
                            response.message = "El curso no existe en SQL.";
                            return Ok(response);
                        }
                        reader.Close();
                    }
                }

                // 3. Obtener folder_id
                int folderId = -1;
                string getFolderQuery = "SELECT folder_id FROM Folder WHERE course_code = @code AND section_name = @section";
                using (SqlCommand folderCmd = new SqlCommand(getFolderQuery))
                {
                    folderCmd.Parameters.AddWithValue("@code", message.course_code);
                    folderCmd.Parameters.AddWithValue("@section", message.document_section);

                    using (SqlDataReader reader = db.Execute_query(folderCmd, out connection))
                    {
                        if (reader.Read())
                        {
                            folderId = Convert.ToInt32(reader["folder_id"]);
                        }
                        else
                        {
                            response.status = "ERROR";
                            response.message = "No se encontró la sección del documento para ese curso.";
                            return Ok(response);
                        }
                        reader.Close();
                    }
                }

                // 4. Insertar el documento
                string insertDocumentQuery = @"
            INSERT INTO Document (filename, path, upload_date, uploaded_by_professor, folder_id)
            VALUES (@filename, @path, @upload_date, @uploaded_by_professor, @folder_id)";

                using (SqlCommand insertCmd = new SqlCommand(insertDocumentQuery))
                {
                    insertCmd.Parameters.AddWithValue("@filename", message.file_name);
                    insertCmd.Parameters.AddWithValue("@path", "Data_base_files/Documents"); // Ruta por defecto o lógica según tu diseño
                    insertCmd.Parameters.AddWithValue("@upload_date", DateTime.Now.Date);
                    insertCmd.Parameters.AddWithValue("@uploaded_by_professor", message.uploaded_by_professor);
                    insertCmd.Parameters.AddWithValue("@folder_id", folderId);

                    db.Execute_non_query(insertCmd);
                }

                response.status = "OK";
                response.message = "Documento agregado correctamente.";
                return Ok(response);    
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al agregar el documento: " + ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPost("delete_document")]
        public IActionResult PostDeleteDocument([FromBody] Data_input_delete_document message)
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

        [HttpPost("edit_document")]
        public IActionResult PostEditDocument([FromBody] Data_input_edit_document message)
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

        [HttpPost("add_document_section")]
        public IActionResult PostAddDocumentSection([FromBody] Data_input_create_document_section message)
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

        [HttpPost("delete_document_section")]
        public IActionResult PostDeleteDocumentSection([FromBody] Data_input_delete_document_section message)
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


        [HttpPost("view_student_documents")]
        public IActionResult PostViewStudentDocuments([FromBody] Data_input_view_student_documents message)
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

            Data_output_view_student_documents Data_output_view_student_documents = new Data_output_view_student_documents();

            response.status = "OK";
            response.message = Data_output_view_student_documents;
            return Ok(response);
        }




    }
}
