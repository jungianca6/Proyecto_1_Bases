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


            Data_output_view_student_evaluations data_Output_View_Student_Evaluations = new Data_output_view_student_evaluations();

            response.status = "OK";
            response.message = data_Output_View_Student_Evaluations;
            return Ok(response);
        }

        [HttpPost("send_student_evaluation")]
        public IActionResult PostSendStudentEvaluation([FromBody] Data_input_submit_evaluation message)
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
