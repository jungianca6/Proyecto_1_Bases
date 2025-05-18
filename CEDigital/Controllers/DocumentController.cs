using CEDigital.Data_input_models;
using CEDigital.Data_output_models;
using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CEDigital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {

        private Api_response response;

        public DocumentController()
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

        [HttpPost("add_document")]
        public IActionResult PostAddDocument([FromBody] Data_input_add_document message)
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

        [HttpPost("delete_document")]
        public IActionResult PostDeleteDocument([FromBody] Data_input_delete_document message)
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

        [HttpPost("edit_document")]
        public IActionResult PostEditDocument([FromBody] Data_input_edit_document message)
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

        [HttpPost("add_document_section")]
        public IActionResult PostAddDocumentSection([FromBody] Data_input_create_document_section message)
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

        [HttpPost("delete_document_section")]
        public IActionResult PostDeleteDocumentSection([FromBody] Data_input_delete_document_section message)
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
