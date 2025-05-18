using CEDigital.Data_input_models;
using CEDigital.Data_output_models;
using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CEDigital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {

        private Api_response response;

        public NewsController()
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


        [HttpPost("add_new")]
        public IActionResult PostAddNew([FromBody] Data_input_manage_news message)
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


        [HttpPost("delete_new")]
        public IActionResult PostDeleteNew([FromBody] Data_input_manage_news message)
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

        [HttpPost("edit_new")]
        public IActionResult PostEditNew([FromBody] Data_input_manage_news message)
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
