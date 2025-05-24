using CEDigital.Data_input_models;
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

            Data_output_grades_report data_Output_Grades_Report = new Data_output_grades_report();

            response.status = "OK";
            response.message = data_Output_Grades_Report;
            return Ok(response);
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
                    getStudentsCmd.Parameters.AddWithValue("@group", message.group_number);
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

            Data_output_student_grade_report data_Output_Student_Grade_Report = new Data_output_student_grade_report();

            response.status = "OK";
            response.message = data_Output_Student_Grade_Report;
            return Ok(response);
        }


    }
}