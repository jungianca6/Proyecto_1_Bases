using CEDigital.Data_input_models;
using CEDigital.Data_output_models;
using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;

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

            Data_output_enrolled_students_report data_Output_Enrolled_Students_Report = new Data_output_enrolled_students_report();

            response.status = "OK";
            response.message = data_Output_Enrolled_Students_Report;
            return Ok(response);
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