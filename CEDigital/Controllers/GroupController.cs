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
    public class GroupController : ControllerBase
    {

        private Api_response response;

        public GroupController()
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

        [HttpPost("create_group")]
        public IActionResult PostCreateGruop([FromBody] Data_input_add_group message)
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
