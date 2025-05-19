using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;
using CEDigital.Data_input_models;
using CEDigital.Data_output_models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CEDigital.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly MongoDbQueryService _mongoService;
        private Api_response response;

        public LoginController()
        {
            _mongoService = new MongoDbQueryService(); // Si no usas inyección de dependencias aún
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
        public async Task<IActionResult> PostUser([FromBody] Data_input_login message)
        {

            if (string.IsNullOrEmpty(message.username))
            {
                response.status = "Ok";
                response.message = "El usuario es invalido";
                return Ok(response);
            }

            var (userDoc, userType) = await _mongoService.FindUser(message.username, message.password);

            if (userDoc == null)
            {
                response.status = "ERROR";
                response.message = "Usuario no encontrado o contraseña incorrecta.";
                return Ok(response);
            }

            Data_output_login data_Output_Login = new Data_output_login()
            {
                username = userDoc.GetValue("username", "").AsString,
                password = userDoc.GetValue("password","").AsString,
                user_type = userDoc.GetValue("user_type", "").AsString,
                primary_key = userDoc.GetValue("_id", "").AsString
            };

            response.status = "OK";
            response.message = data_Output_Login;
            return Ok(response);
        }
    }
}