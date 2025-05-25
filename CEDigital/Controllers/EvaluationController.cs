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
    public class EvaluationController : ControllerBase
    {

        private Api_response response;

        public EvaluationController()
        {
            response = new Api_response("OK", null);  // Inicializando la respuesta por defecto
        }


        [HttpPost("view_student_evaluations")]
        public IActionResult PostViewStudentEvaluation([FromBody] Data_input_view_student_evaluations message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection = null;

            try
            {
                // 0. Verificar que el estudiante existe
                string checkStudentExistQuery = "SELECT COUNT(*) FROM Student WHERE student_id = @student_id";
                using (SqlCommand checkStudentExistCmd = new SqlCommand(checkStudentExistQuery))
                {
                    checkStudentExistCmd.Parameters.AddWithValue("@student_id", message.student_id);

                    using (SqlDataReader reader = db.Execute_query(checkStudentExistCmd, out connection))
                    {
                        if (reader.Read())
                        {
                            int count = Convert.ToInt32(reader[0]);
                            if (count == 0)
                            {
                                response.status = "ERROR";
                                response.message = "El estudiante no existe.";
                                return Ok(response);
                            }
                        }
                        reader.Close();
                    }
                }

                // 1. Obtener las evaluaciones del estudiante con el query corregido
                string evaluationsQuery = @"
                SELECT 
                gi.name AS rubric_name,
                e.evaluation_title,
                es.grade,
                es.feedback,
                es.is_public,
                e.evaluation_date,
                es.data_base_path_evalution,
                e.data_base_path_professor,
                es.evaluation_filename,
                e.professor_filename
                FROM Evaluation e
                INNER JOIN Grading_item gi ON e.grading_item_id = gi.grading_item_id
                INNER JOIN Evaluation_Student es ON e.evaluation_id = es.evaluation_id
                INNER JOIN Groups g ON gi.group_id = g.group_id
                WHERE es.student_id = @student_id
                AND g.course_code = @course_code
                AND g.group_number = @group_number
                ORDER BY e.evaluation_date DESC;";

                var evaluations = new List<Student_evaluation_model>();

                using (SqlCommand evalCmd = new SqlCommand(evaluationsQuery))
                {
                    evalCmd.Parameters.AddWithValue("@student_id", message.student_id);
                    evalCmd.Parameters.AddWithValue("@course_code", message.course_code);
                    evalCmd.Parameters.AddWithValue("@group_number", message.group_number);

                    using (SqlDataReader reader = db.Execute_query(evalCmd, out connection))
                    {
                        while (reader.Read())
                        {
                            evaluations.Add(new Student_evaluation_model
                            {
                                rubric_name = reader["rubric_name"].ToString(),
                                evaluation_title = reader["evaluation_title"].ToString(),
                                grade = reader["grade"] != DBNull.Value ? Convert.ToSingle(reader["grade"]) : 0f,
                                feedback = reader["feedback"]?.ToString(),
                                is_public = Convert.ToBoolean(reader["is_public"]),
                                evaluation_date = Convert.ToDateTime(reader["evaluation_date"]),
                                data_base_path_evalution = reader["data_base_path_evalution"]?.ToString(),
                                data_base_path_professor = reader["data_base_path_professor"]?.ToString(),
                                evaluation_filename = reader["evaluation_filename"]?.ToString(),
                                professor_filename = reader["professor_filename"]?.ToString()
                            });
                        }
                        reader.Close();
                    }
                }

                if (evaluations.Count == 0)
                {
                    response.status = "ERROR";
                    response.message = "No se encontraron evaluaciones para el estudiante en ese curso y grupo.";
                    return Ok(response);
                }

                // 2. Preparar la salida
                var dataOutput = new Data_output_view_student_evaluations
                {
                    evaluations = evaluations
                };

                response.status = "OK";
                response.message = dataOutput;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error en el servidor: " + ex.Message;
                return Ok(response);
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        [HttpPost("send_student_evaluation")]
        public IActionResult PostSendStudentEvaluation([FromBody] Data_input_submit_evaluation message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection = null;

            try
            {

                // 1. Obtener group_id desde Student_Group con student_id y course_code
                string getGroupIdQuery = @"
            SELECT sg.group_id
            FROM Student_Group sg
            WHERE sg.student_id = @student_id
              AND sg.course_code = @course_code";

                int groupId;

                using (SqlCommand cmd = new SqlCommand(getGroupIdQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@student_id", message.student_id);
                    cmd.Parameters.AddWithValue("@course_code", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(cmd, out connection))
                    {
                        if (reader.Read())
                        {
                            groupId = Convert.ToInt32(reader["group_id"]);
                        }
                        else
                        {
                            response.status = "ERROR";
                            response.message = "No se encontró el grupo para el estudiante con ese código de curso.";
                            return Ok(response);
                        }
                    }
                }

                // 2. Validar que el group_number coincida en Groups con el group_id obtenido
                string checkGroupNumberQuery = @"
            SELECT group_number
            FROM Groups
            WHERE group_id = @group_id";

                int actualGroupNumber;

                using (SqlCommand cmd = new SqlCommand(checkGroupNumberQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@group_id", groupId);

                    using (SqlDataReader reader = db.Execute_query(cmd, out connection))
                    {
                        if (reader.Read())
                        {
                            actualGroupNumber = Convert.ToInt32(reader["group_number"]);
                            if (actualGroupNumber != message.group_number)
                            {
                                response.status = "ERROR";
                                response.message = "El número de grupo no coincide con el grupo asignado al estudiante.";
                                return Ok(response);
                            }
                        }
                        else
                        {
                            response.status = "ERROR";
                            response.message = "No se encontró el grupo en la tabla Groups.";
                            return Ok(response);
                        }
                    }
                }

                // 3. Obtener grading_item_id usando grading_item_name y group_id
                string getGradingItemIdQuery = @"
            SELECT grading_item_id
            FROM Grading_item
            WHERE name = @grading_item_name
              AND group_id = @group_id";

                int gradingItemId;

                using (SqlCommand cmd = new SqlCommand(getGradingItemIdQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@grading_item_name", message.grading_item_name);
                    cmd.Parameters.AddWithValue("@group_id", groupId);

                    using (SqlDataReader reader = db.Execute_query(cmd, out connection))
                    {
                        if (reader.Read())
                        {
                            gradingItemId = Convert.ToInt32(reader["grading_item_id"]);
                        }
                        else
                        {
                            response.status = "ERROR";
                            response.message = "No se encontró el rubro para el grupo especificado.";
                            return Ok(response);
                        }
                    }
                }

                // 4. Insertar evaluación en Evaluation
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
                @evaluation_title,
                @professor_filename,
                @data_base_path_professor,
                GETDATE(),
                0,
                @grading_item_id
            )";

                using (SqlCommand insertCmd = new SqlCommand(insertEvaluationQuery, connection))
                {
                    insertCmd.Parameters.AddWithValue("@evaluation_title", message.evaluation_title);
                    insertCmd.Parameters.AddWithValue("@professor_filename", message.filename_evaluation);
                    insertCmd.Parameters.AddWithValue("@data_base_path_professor", message.path);
                    insertCmd.Parameters.AddWithValue("@grading_item_id", gradingItemId);

                    db.Execute_non_query(insertCmd, out connection);
                }

                response.status = "OK";
                response.message = "Evaluación registrada correctamente.";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al insertar la evaluación: " + ex.Message;
                return Ok(response);
            }
            finally
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}