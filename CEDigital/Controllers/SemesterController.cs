using Azure;
using CEDigital.Data_input_models;
using CEDigital.Data_output_models;
using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CEDigital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SemesterController : ControllerBase
    {
        private Api_response response;
        public SemesterController()
        {
            response = new Api_response("OK", null);  // Inicializando la respuesta por defecto
        }

        [HttpPost("initialize_semester")]
        public IActionResult PostInitializeSemester([FromBody] Data_input_initialize_semester message)
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

        [HttpPost("add_course_to_semester")]
        public IActionResult PostaAddCourseToSemester([FromBody] Data_input_add_course_to_semester message)
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
