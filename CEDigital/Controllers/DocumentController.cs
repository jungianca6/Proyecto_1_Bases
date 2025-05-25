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
               
                int folderId = -1;
                string getFolderQuery = "SELECT folder_id FROM Folder WHERE group_id = @code AND name = @section";

                using (SqlCommand folderCmd = new SqlCommand(getFolderQuery))
                {
                    folderCmd.Parameters.AddWithValue("@code", message.group_id);
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

                string insertDocumentQuery = @"
            INSERT INTO Document (filename, path, upload_date, uploaded_by_professor, folder_id)
            VALUES (@filename, @path, @upload_date, @uploaded_by_professor, @folder_id)";

                using (SqlCommand insertCmd = new SqlCommand(insertDocumentQuery))
                {
                    insertCmd.Parameters.AddWithValue("@filename", message.file_name);
                    insertCmd.Parameters.AddWithValue("@path", "Data_base_files/Documents"); // Ruta lógica
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
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            try
            {

                //Verificar si la sección existe
                int folderId = -1;
                string getFolderQuery = "SELECT folder_id FROM Folder WHERE group_id = @group AND name = @section";
                using (SqlCommand folderCmd = new SqlCommand(getFolderQuery))
                {
                    folderCmd.Parameters.AddWithValue("@group", message.group_id);
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
                            response.message = "No se encontró la sección del documento para ese grupo.";
                            return Ok(response);
                        }
                        reader.Close();
                    }
                }

                // Eliminar el documento
                string deleteDocumentQuery = "DELETE FROM Document WHERE filename = @filename AND folder_id = @folder_id";
                using (SqlCommand deleteCmd = new SqlCommand(deleteDocumentQuery))
                {
                    deleteCmd.Parameters.AddWithValue("@filename", message.file_name);
                    deleteCmd.Parameters.AddWithValue("@folder_id", folderId);

                    db.Execute_non_query(deleteCmd);
                }
                
                // Eliminar archivo físico si existe
                string filePath = Path.Combine("Data_base_files/Documents", message.file_name);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                response.status = "OK";
                response.message = "Documento eliminado correctamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al eliminar el documento: " + ex.Message;
                return StatusCode(500, response);
            }
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
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            try
            {

                // Insertar nueva sección en la tabla Folder
                string insertSectionQuery = "INSERT INTO Folder (group_id, name) VALUES (@group, @name)";
                using (SqlCommand insertCmd = new SqlCommand(insertSectionQuery))
                {
                    insertCmd.Parameters.AddWithValue("@group", message.group_id);
                    insertCmd.Parameters.AddWithValue("@name", message.section_name);
                    db.Execute_non_query(insertCmd);
                }

                // Enviar respuesta positiva
                response.status = "OK";
                response.message = "Sección agregada correctamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al agregar la sección: " + ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPost("delete_document_section")]
        public IActionResult PostDeleteDocumentSection([FromBody] Data_input_delete_document_section message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            try
            {

                // Verificar si la sección existe
                int folderId = -1;
                string getFolderQuery = "SELECT folder_id FROM Folder WHERE group_id = @group AND name = @section";
                using (SqlCommand folderCmd = new SqlCommand(getFolderQuery))
                {
                    folderCmd.Parameters.AddWithValue("@group", message.group_id);
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
                            response.message = "No se encontró la sección del documento para ese grupo.";
                            return Ok(response);
                        }
                        reader.Close();
                    }
                }

                // Eliminar documentos vinculados
                string deleteDocsQuery = "DELETE FROM Document WHERE folder_id = @folder";
                using (SqlCommand delDocsCmd = new SqlCommand(deleteDocsQuery))
                {
                    delDocsCmd.Parameters.AddWithValue("@folder", folderId);
                    db.Execute_non_query(delDocsCmd);
                }

                // Eliminar la carpeta (sección)
                string deleteFolderQuery = "DELETE FROM Folder WHERE folder_id = @folder";
                using (SqlCommand delFolderCmd = new SqlCommand(deleteFolderQuery))
                {
                    delFolderCmd.Parameters.AddWithValue("@folder", folderId);
                    db.Execute_non_query(delFolderCmd);
                }

                response.status = "OK";
                response.message = "Sección eliminada correctamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al eliminar la sección: " + ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPost("view_student_documents")]
        public IActionResult PostViewStudentDocuments([FromBody] Data_input_view_student_documents message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            try
            {

                // Verificar que el estudiante está inscrito en el grupo
                string checkEnrollmentQuery = "SELECT 1 FROM Student_Group WHERE student_id = @student AND group_id = @group";
                using (SqlCommand checkCmd = new SqlCommand(checkEnrollmentQuery))
                {
                    checkCmd.Parameters.AddWithValue("@student", message.student_id);
                    checkCmd.Parameters.AddWithValue("@group", message.group_id);

                    using (SqlDataReader reader = db.Execute_query(checkCmd, out connection))
                    {
                        if (!reader.Read())
                        {
                            response.status = "ERROR";
                            response.message = "El estudiante no está inscrito en el grupo.";
                            return Ok(response);
                        }
                        reader.Close();
                    }
                }

                // Obtener los documentos del grupo
                string getDocumentsQuery = @"
                    SELECT 
                        d.document_id,
                        d.filename,
                        d.path,
                        FORMAT(d.upload_date, 'dd/MM/yyyy HH:mm') AS formatted_date,
                        d.uploaded_by_professor,
                        d.folder_id,
                        f.name AS folder_name
                    FROM Document d
                    JOIN Folder f ON d.folder_id = f.folder_id
                    WHERE f.group_id = @groupId";

                List<Student_document_model> documentList = new List<Student_document_model>();
                using (SqlCommand docCmd = new SqlCommand(getDocumentsQuery))
                {
                    docCmd.Parameters.AddWithValue("@groupId", message.group_id);

                    using (SqlDataReader reader = db.Execute_query(docCmd, out connection))
                    {
                        while (reader.Read())
                        {
                            Student_document_model doc = new Student_document_model
                            {
                                document_id = Convert.ToInt32(reader["document_id"]),
                                filename = reader["filename"].ToString(),
                                path = reader["path"].ToString(),
                                upload_date = reader["formatted_date"].ToString(),
                                uploaded_by_professor = Convert.ToBoolean(reader["uploaded_by_professor"]),
                                folder_id = Convert.ToInt32(reader["folder_id"]),
                                folder_name = reader["folder_name"]?.ToString() 
                            };
                            documentList.Add(doc);
                        }
                        reader.Close();
                    }
                }

                Data_output_view_student_documents output = new Data_output_view_student_documents
                {
                    documents = documentList
                };

                response.status = "OK";
                response.message = output;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al obtener los documentos: " + ex.Message;
                return StatusCode(500, response);
            }
        }




    }
}
