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

        [HttpPost("add_default_document_sections")]
        public IActionResult PostAddDefaultDocumentSections([FromBody] Data_input_add_default_document_sections message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            try
            {
                // Insertar secciones en SQL (tabla Folder)
                string insertSectionQuery = "INSERT INTO Folder (group_id, name) VALUES (@code, @section)";

                foreach (var section in message.sections)
                {
                    using (SqlCommand insertCmd = new SqlCommand(insertSectionQuery))
                    {
                        insertCmd.Parameters.AddWithValue("@code", message.group_id);
                        insertCmd.Parameters.AddWithValue("@section", section);
                        db.Execute_non_query(insertCmd);
                    }
                }

                response.status = "OK";
                response.message = "Secciones creadas exitosamente para el curso " + message.group_id;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al crear secciones: " + ex.Message;
                return StatusCode(500, response);
            }
        }


        [HttpPost("add_default_grades")]
        public IActionResult PostAddDefaultGrades([FromBody] Data_input_add_default_grades message)
        {
            SQL_connection db = new SQL_connection();
            string checkCourseQuery = "SELECT COUNT(*) FROM Course WHERE course_code = @code";
            string insertQuery = "INSERT INTO Course_Percentages (course_code, section, percentage) VALUES (@code, @section, @percentage)";
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
                            reader.Close();
                            response.status = "ERROR";
                            response.message = "El curso no existe.";
                            return Ok(response);
                        }
                        reader.Close();
                    }
                }

                // Validar que listas tengan misma longitud
                if (message.sections.Count != message.percentages.Count)
                {
                    response.status = "ERROR";
                    response.message = "Las listas de secciones y porcentajes no tienen la misma cantidad de elementos.";
                    return Ok(response);
                }

                // Insertar cada sección con su porcentaje
                for (int i = 0; i < message.sections.Count; i++)
                {
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery))
                    {
                        insertCmd.Parameters.AddWithValue("@code", message.course_code);
                        insertCmd.Parameters.AddWithValue("@section", message.sections[i]);
                        insertCmd.Parameters.AddWithValue("@percentage", message.percentages[i]);
                        db.Execute_non_query(insertCmd);
                    }
                }

                response.status = "OK";
                response.message = "Porcentajes por defecto añadidos correctamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al insertar porcentajes: " + ex.Message;
                return StatusCode(500, response);
            }
        }



        [HttpPost("show_grading_item")]
        public IActionResult PostShowGradingItem([FromBody] Data_input_show_grading_items message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            string query = "SELECT section_name, percentage FROM Group_Section_Percentage WHERE group_id = @group_id";

            Data_output_show_grading_items data_Output_Show_Grading_Item = new Data_output_show_grading_items
            {
                sections = new List<string>(),
                percentages = new List<double>()
            };

            try
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@group_id", message.group_number);

                    using (SqlDataReader reader = db.Execute_query(cmd, out connection))
                    {
                        if (!reader.HasRows)
                        {
                            response.status = "ERROR";
                            response.message = "No se encontraron secciones para el grupo.";
                            return Ok(response);
                        }

                        while (reader.Read())
                        {
                            data_Output_Show_Grading_Item.sections.Add(reader["section_name"].ToString());
                            data_Output_Show_Grading_Item.percentages.Add(Convert.ToDouble(reader["percentage"]));
                        }

                        reader.Close();
                    }
                }

                response.status = "OK";
                response.message = data_Output_Show_Grading_Item;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al consultar los ítems de evaluación: " + ex.Message;
                return StatusCode(500, response);
            }
        }


        [HttpPost("modify_grading_item")]
        public IActionResult PostShowGradingItem([FromBody] Data_input_update_grade message)
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

        [HttpPost("add_grading_item")]
        public IActionResult PostAddGradingItem([FromBody] Data_input_update_grade message)
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

        [HttpPost("delete_grading_item")]
        public IActionResult PostDeleteGradingItem([FromBody] Data_input_show_grading_items message)
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

        [HttpPost("assign_evaluation")]
        public IActionResult PostAssingEvalutaion([FromBody] Data_input_assign_evaluation message)
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

        [HttpPost("evaluate_submission")]
        public IActionResult PostEvaluateSubmission([FromBody] Data_input_submit_grade_and_request_file message)
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

            Data_output_submission_file data_out = new Data_output_submission_file();

            response.status = "OK";
            response.message = data_out;
            return Ok(response);
        }

        [HttpPost("update_submission")]
        public IActionResult PostUpdateSubmission([FromBody] Data_input_update_grade message)
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


    }
}
