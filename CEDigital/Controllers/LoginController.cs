using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;
using CEDigital.Data_input_models;
using CEDigital.Data_output_models;

namespace CEDigital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private Api_response response;

        public LoginController()
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

        [HttpPost("user")]
        public IActionResult PostUser([FromBody] Data_input_login message)
        {

            if (string.IsNullOrEmpty(message.username))
            {
                response.status = "Ok";
                response.message = "El usuario es invalido";
                return Ok(response);
            }

            /*
             * #######Logica para completar la clase message con la informacion del SQL Y MongoDBB#######
             */

            Data_output_login data_Output_Login = new Data_output_login();


            /*
             * #######Envio de la respuesta#######
             */

            response.status = "OK";
            response.message = data_Output_Login;
            return Ok(response);
        }



    }
}
