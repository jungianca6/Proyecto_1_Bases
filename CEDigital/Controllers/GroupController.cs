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
            SqlConnection connection;

            // Validación básica
            if (message.sections.Count != message.percentages.Count)
            {
                response.status = "ERROR";
                response.message = "La cantidad de secciones no coincide con la cantidad de porcentajes.";
                return Ok(response);
            }

            string checkGroupQuery = "SELECT COUNT(*) FROM Groups WHERE group_id = @group_id";
            string insertQuery = "INSERT INTO Group_Section_Percentage (group_id, section_name, percentage) VALUES (@group_id, @section_name, @percentage)";

            try
            {
                // Verificar si el group_id existe
                using (SqlCommand checkCommand = new SqlCommand(checkGroupQuery))
                {
                    checkCommand.Parameters.AddWithValue("@group_id", message.group_id);

                    using (SqlDataReader reader = db.Execute_query(checkCommand, out connection))
                    {
                        if (reader.Read() && Convert.ToInt32(reader[0]) == 0)
                        {
                            response.status = "ERROR";
                            response.message = "El grupo especificado no existe.";
                            return Ok(response);
                        }
                        reader.Close();
                    }
                }

                // Insertar cada sección con su porcentaje
                for (int i = 0; i < message.sections.Count; i++)
                {
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery))
                    {
                        insertCommand.Parameters.AddWithValue("@group_id", message.group_id);
                        insertCommand.Parameters.AddWithValue("@section_name", message.sections[i]);
                        insertCommand.Parameters.AddWithValue("@percentage", message.percentages[i]);

                        db.Execute_non_query(insertCommand);
                    }
                }

                response.status = "OK";
                response.message = "Secciones y porcentajes añadidos correctamente al grupo.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al agregar las secciones: " + ex.Message;
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
        public IActionResult PostModifyGradingItem([FromBody] Data_input_update_grading_items message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            try
            {
                foreach (var item in message.grading_items)
                {
                    string checkExistsQuery = @"
                    SELECT COUNT(*) 
                    FROM Grading_item gi
                    INNER JOIN Groups g ON gi.group_id = g.group_id
                    WHERE gi.grading_item_id = @grading_item_id 
                    AND g.course_code = @course_code
                    AND g.group_number = @group_number";

                    using (SqlCommand checkCmd = new SqlCommand(checkExistsQuery))
                    {
                        checkCmd.Parameters.AddWithValue("@grading_item_id", item.grading_item_id);
                        checkCmd.Parameters.AddWithValue("@course_code", message.course_code);
                        checkCmd.Parameters.AddWithValue("@group_number", message.group_number);

                        using (SqlDataReader reader = db.Execute_query(checkCmd, out connection))
                        {
                            if (reader.Read() && Convert.ToInt32(reader[0]) == 0)
                            {
                                reader.Close();
                                response.status = "ERROR";
                                response.message = $"El ítem con ID {item.grading_item_id} no existe para el curso {message.course_code} y grupo {message.group_number}.";
                                return Ok(response);
                            }
                            reader.Close();
                        }
                    }

                    List<string> setClauses = new List<string>();
                    if (!string.IsNullOrWhiteSpace(item.name))
                        setClauses.Add("name = @name");
                    if (item.percentage != 0f)
                        setClauses.Add("percentage = @percentage");

                    if (setClauses.Count == 0)
                        continue;

                    string updateQuery = $"UPDATE Grading_item SET {string.Join(", ", setClauses)} WHERE grading_item_id = @grading_item_id";

                    using (SqlCommand updateCmd = new SqlCommand(updateQuery))
                    {
                        updateCmd.Parameters.AddWithValue("@grading_item_id", item.grading_item_id);
                        if (setClauses.Contains("name = @name"))
                            updateCmd.Parameters.AddWithValue("@name", item.name);
                        if (setClauses.Contains("percentage = @percentage"))
                            updateCmd.Parameters.AddWithValue("@percentage", item.percentage);

                        int affected = db.Execute_non_query(updateCmd, out connection);
                        if (affected == 0)
                        {
                            response.status = "ERROR";
                            response.message = $"No se pudo modificar el ítem con ID {item.grading_item_id}.";
                            return Ok(response);
                        }
                    }
                }

                response.status = "OK";
                response.message = "Ítems modificados correctamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error en el servidor: " + ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPost("add_grading_items")]
        public IActionResult PostAddGradingItems([FromBody] Data_input_update_grading_items message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            try
            {
                // Obtener el group_id a partir de course_code y group_number
                string getGroupIdQuery = @"
            SELECT group_id 
            FROM Groups 
            WHERE course_code = @course_code AND group_number = @group_number";

                int? groupId = null;
                using (SqlCommand getGroupCmd = new SqlCommand(getGroupIdQuery))
                {
                    getGroupCmd.Parameters.AddWithValue("@course_code", message.course_code);
                    getGroupCmd.Parameters.AddWithValue("@group_number", message.group_number);

                    using (var reader = db.Execute_query(getGroupCmd, out connection))
                    {
                        if (reader.Read())
                        {
                            groupId = reader.GetInt32(0);
                        }
                        else
                        {
                            response.status = "ERROR";
                            response.message = $"No existe el grupo {message.group_number} para el curso {message.course_code}.";
                            return Ok(response);
                        }
                    }
                }

                // Insertar cada grading item con el group_id obtenido
                string insertQuery = @"
            INSERT INTO Grading_item (name, percentage, group_id)
            VALUES (@name, @percentage, @group_id)";

                foreach (var item in message.grading_items)
                {
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery))
                    {
                        insertCmd.Parameters.AddWithValue("@name", item.name);
                        insertCmd.Parameters.AddWithValue("@percentage", item.percentage);
                        insertCmd.Parameters.AddWithValue("@group_id", groupId.Value);

                        db.Execute_non_query(insertCmd);  // método sin retorno
                    }
                }

                response.status = "OK";
                response.message = "Ítems agregados correctamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error en el servidor: " + ex.Message;
                return StatusCode(500, response);
            }
        }


        [HttpPost("delete_grading_item")]
        public IActionResult PostDeleteGradingItemsByName([FromBody] Data_input_delete_grading_item message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            try
            {
                if (message.name_grading_items == null || message.name_grading_items.Count == 0)
                {
                    response.status = "ERROR";
                    response.message = "No se proporcionaron ítems para eliminar.";
                    return Ok(response);
                }

                // Crear lista de parámetros para el IN
                List<string> parameterNames = new List<string>();
                for (int i = 0; i < message.name_grading_items.Count; i++)
                {
                    parameterNames.Add($"@name{i}");
                }

                string deleteQuery = $@"
                DELETE gi FROM Grading_item gi
                INNER JOIN Groups g ON gi.group_id = g.group_id
                WHERE gi.name IN ({string.Join(", ", parameterNames)})
                AND g.course_code = @course_code
                AND g.group_number = @group_number";

                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery))
                {
                    for (int i = 0; i < message.name_grading_items.Count; i++)
                    {
                        deleteCmd.Parameters.AddWithValue(parameterNames[i], message.name_grading_items[i]);
                    }
                    deleteCmd.Parameters.AddWithValue("@course_code", message.course_code);
                    deleteCmd.Parameters.AddWithValue("@group_number", message.group_number);

                    int affected = db.Execute_non_query(deleteCmd, out connection);
                    if (affected == 0)
                    {
                        response.status = "ERROR";
                        response.message = "No se pudo eliminar ningún ítem con los nombres proporcionados.";
                        return Ok(response);
                    }
                }

                response.status = "OK";
                response.message = "Ítems eliminados correctamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error en el servidor: " + ex.Message;
                return StatusCode(500, response);
            }
        }


        [HttpPost("assign_evaluation")]
        public IActionResult PostAssingEvalutaion([FromBody] Data_input_assign_evaluation message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            try
            {
                // Paso 1: Verificar grupo
                string findGroupQuery = @"
            SELECT group_id FROM Groups 
            WHERE group_number = @group_number AND course_code = @course_code";

                int groupId = -1;

                using (SqlCommand groupCommand = new SqlCommand(findGroupQuery))
                {
                    groupCommand.Parameters.AddWithValue("@group_number", message.group_number);
                    groupCommand.Parameters.AddWithValue("@course_code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(groupCommand, out connection))
                    {
                        if (reader.Read())
                        {
                            groupId = Convert.ToInt32(reader["group_id"]);
                        }
                        else
                        {
                            response.status = "ERROR";
                            response.message = "El grupo no existe para ese curso.";
                            return Ok(response);
                        }
                        reader.Close();
                    }
                }

                // Paso 2: Verificar grading item
                string findGradingItemQuery = @"
            SELECT grading_item_id FROM Grading_item 
            WHERE name = @name AND percentage = @percentage AND group_id = @group_id";

                int gradingItemId = -1;

                using (SqlCommand gradingCommand = new SqlCommand(findGradingItemQuery))
                {
                    gradingCommand.Parameters.AddWithValue("@name", message.grading_item_name);
                    gradingCommand.Parameters.AddWithValue("@percentage", message.grading_item_percentage);
                    gradingCommand.Parameters.AddWithValue("@group_id", groupId);

                    using (SqlDataReader reader = db.Execute_query(gradingCommand, out connection))
                    {
                        if (reader.Read())
                        {
                            gradingItemId = Convert.ToInt32(reader["grading_item_id"]);
                        }
                        else
                        {
                            response.status = "ERROR";
                            response.message = "El Grading Item no existe para ese grupo.";
                            return Ok(response);
                        }
                        reader.Close();
                    }
                }

                // Paso 3: Insertar evaluación
                string insertEvaluationQuery = @"
            INSERT INTO Evaluation (
                evaluation_title, 
                professor_filename, 
                data_base_path_professor, 
                evaluation_date, 
                is_group, 
                grading_item_id
            ) 
            VALUES (
                @title, 
                @filename, 
                @path, 
                @date, 
                @is_group, 
                @grading_item_id
            )";

                using (SqlCommand insertCommand = new SqlCommand(insertEvaluationQuery))
                {
                    insertCommand.Parameters.AddWithValue("@title", message.evaluation_name);
                    insertCommand.Parameters.AddWithValue("@filename", message.professor_filename);
                    insertCommand.Parameters.AddWithValue("@path", message.data_base_path_professor);
                    insertCommand.Parameters.AddWithValue("@date", DateTime.Parse(message.delivery_date));
                    insertCommand.Parameters.AddWithValue("@is_group", message.evaluation_type.ToLower() == "grupal" ? 1 : 0);
                    insertCommand.Parameters.AddWithValue("@grading_item_id", gradingItemId);

                    db.Execute_non_query(insertCommand);
                }

                response.status = "OK";
                response.message = "Evaluación creada exitosamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al crear la evaluación: " + ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPost("evaluate_evaluation")]
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

        [HttpPost("update_evaluation")]
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
