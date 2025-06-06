﻿using CEDigital.Data_input_models;
using CEDigital.Data_output_models;
using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MongoDB.Driver;

namespace CEDigital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {

        private Api_response response;

        public ReportController()
        {
            response = new Api_response("OK", null);  // Inicializando la respuesta por defecto
        }


        [HttpPost("grades_report")]
        public IActionResult PostGradesReport([FromBody] Data_input_grades_report message)
        {
            try
            {
                SQL_connection db = new SQL_connection();
                SqlConnection connection;

                // 1. Obtener todos los estudiantes y sus notas por rubro
                string query = @"
            SELECT 
                ES.student_id,
                GI.name AS grading_item_name,
                ES.grade
            FROM Evaluation_Student ES
            INNER JOIN Evaluation E ON ES.evaluation_id = E.evaluation_id
            INNER JOIN Grading_item GI ON E.grading_item_id = GI.grading_item_id
            INNER JOIN Groups G ON GI.group_id = G.group_id
            WHERE G.course_code = @course_code AND G.group_number = @group_number";

                Dictionary<string, Student_grade_model> studentGrades = new Dictionary<string, Student_grade_model>();

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@course_code", message.course_code);
                    cmd.Parameters.AddWithValue("@group_number", message.group_number);

                    using (SqlDataReader reader = db.Execute_query(cmd, out connection))
                    {
                        while (reader.Read())
                        {
                            string studentId = reader["student_id"].ToString();
                            string rubricName = reader["grading_item_name"].ToString();
                            float grade = reader["grade"] != DBNull.Value ? Convert.ToSingle(reader["grade"]) : 0;

                            if (!studentGrades.ContainsKey(studentId))
                            {
                                studentGrades[studentId] = new Student_grade_model
                                {
                                    student_id = studentId,
                                    grades_by_rubric = new Dictionary<string, float>()
                                };
                            }

                            studentGrades[studentId].grades_by_rubric[rubricName] = grade;
                        }
                        reader.Close();
                    }
                }

                // 2. Preparar output
                Data_output_grades_report data_Output_Grades_Report = new Data_output_grades_report
                {
                    grades = studentGrades.Values.ToList()
                };

                // 3. Enviar respuesta
                response.status = "OK";
                response.message = data_Output_Grades_Report;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al generar el reporte de notas: " + ex.Message;
                return StatusCode(500, response);
            }
        }


        [HttpPost("enrolled_students_report")]
        public IActionResult PostEnrolledStudentsReport([FromBody] Data_input_enrolled_students_report message)
        {
            SQL_connection db = new SQL_connection();
            SqlConnection connection;

            Data_output_enrolled_students_report output = new Data_output_enrolled_students_report();
            output.students = new List<Student>();

            try
            {
                // 1. Obtener los Mongo IDs desde SQL
                List<string> mongoStudentIds = new List<string>();
                string getStudentsQuery = @"
                SELECT student_id
                FROM Student_Group 
                WHERE group_id = @group AND course_code = @course";

                using (SqlCommand getStudentsCmd = new SqlCommand(getStudentsQuery))
                {
                    getStudentsCmd.Parameters.AddWithValue("@group", message.group_id);
                    getStudentsCmd.Parameters.AddWithValue("@course", message.course_code);

                    using (SqlDataReader reader = db.Execute_query(getStudentsCmd, out connection))
                    {
                        while (reader.Read())
                        {
                            mongoStudentIds.Add(reader.GetInt32(0).ToString());
                        }
                    }
                }

                if (mongoStudentIds.Count == 0)
                {
                    response.status = "OK";
                    response.message = "No hay estudiantes inscritos en el grupo o curso especificado.";
                    return Ok(response);
                }

                // 2. Obtener los datos desde MongoDB
                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("CEDigital");
                var collection = database.GetCollection<Student>("Student");

                var filter = Builders<Student>.Filter.In("_id", mongoStudentIds);
                var studentsList = collection.Find(filter).ToList();

                output.students = studentsList;

                response.status = "OK";
                response.message = output;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al generar el reporte: " + ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPost("student_grades_report")]
        public IActionResult PostStudentGradesReport([FromBody] Data_input_student_grade_report message)
        {
            try
            {
                SQL_connection db = new SQL_connection();
                SqlConnection connection;

                string query = @"
            SELECT 
                GI.name AS rubric_name,
                E.evaluation_title,
                ES.grade
            FROM Evaluation_Student ES
            INNER JOIN Evaluation E ON ES.evaluation_id = E.evaluation_id
            INNER JOIN Grading_item GI ON E.grading_item_id = GI.grading_item_id
            INNER JOIN Groups G ON GI.group_id = G.group_id
            WHERE ES.student_id = @student_id
              AND G.course_code = @course_code
              AND G.group_number = @group_number";

                List<Student_grade_detail_model> evaluations = new List<Student_grade_detail_model>();

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@student_id", message.student_id);
                    cmd.Parameters.AddWithValue("@course_code", message.course_code);
                    cmd.Parameters.AddWithValue("@group_number", message.group_number);

                    using (SqlDataReader reader = db.Execute_query(cmd, out connection))
                    {
                        while (reader.Read())
                        {
                            Student_grade_detail_model eval = new Student_grade_detail_model
                            {
                                rubric_name = reader["rubric_name"].ToString(),
                                evaluation_title = reader["evaluation_title"].ToString(),
                                grade = reader["grade"] != DBNull.Value ? Convert.ToSingle(reader["grade"]) : 0
                            };

                            evaluations.Add(eval);
                        }
                        reader.Close();
                    }
                }

                Data_output_student_grade_report data_Output_Student_Grade_Report = new Data_output_student_grade_report
                {
                    evaluations = evaluations
                };

                response.status = "OK";
                response.message = data_Output_Student_Grade_Report;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = "ERROR";
                response.message = "Error al obtener las notas del estudiante: " + ex.Message;
                return StatusCode(500, response);
            }
        }



    }
}