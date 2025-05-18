using Azure;
using CEDigital.Data_input_models;
using CEDigital.Data_output_models;
using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CEDigital.Controllers
{
    [ApiController]
    [Route("student/[controller]")]
    public class StudentController : ControllerBase
    {
        private Api_response response;
        public StudentController()
        {
            response = new Api_response("OK", null);  // Inicializando la respuesta por defecto
        }

        [HttpPost("add_student_to_group")]
        public IActionResult PostAddStudentToGroup([FromBody] Data_input_add_student_to_group message)
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
