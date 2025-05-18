using CEDigital.Data_input_models;
using CEDigital.Data_output_models;
using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CEDigital.Controllers
{
    [ApiController]
    [Route("course/[controller]")]
    public class CourseController : ControllerBase
    {

        private Api_response response;

        public CourseController()
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

        [HttpPost("create_course")]
        public IActionResult PostCreateCourse([FromBody] Data_input_admin_create_course message)
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

        [HttpPost("disable_course")]
        public IActionResult PostDisableCourse([FromBody] Data_input_admin_disable_course message)
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

        [HttpPost("view_course")]
        public IActionResult PostViewCourse([FromBody] Data_input_admin_view_course message)
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

            Data_output_admin_view_course data_Output_Admin_View_Course = new Data_output_admin_view_course();

            response.status = "OK";
            response.message = data_Output_Admin_View_Course;
            return Ok(response);
        }

        [HttpPost("add_default_document_sections")]
        public IActionResult PostAddDefaultDocumentSections([FromBody] Data_input_add_default_document_sections message)
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

            Data_output_admin_view_course data_Output_Admin_View_Course = new Data_output_admin_view_course();

            response.status = "OK";
            response.message = data_Output_Admin_View_Course;
            return Ok(response);
        }


        [HttpPost("add_default_grades")]
        public IActionResult PostAddDefaultGrades([FromBody] Data_input_add_default_grades message)
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

            Data_output_admin_view_course data_Output_Admin_View_Course = new Data_output_admin_view_course();

            response.status = "OK";
            response.message = data_Output_Admin_View_Course;
            return Ok(response);
        }

    }
}
