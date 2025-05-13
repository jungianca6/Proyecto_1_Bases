using CEDigital.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CEDigital.Controllers
{
    [ApiController]
    [Route("login/[controller]")]
    public class UserController : ControllerBase
    {

        private Api_response response;
        public UserController()
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

        // Petición POST
        [HttpPost("POST")]
        public ActionResult<Api_response> Login([FromBody] string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                response.status = "BadRequest";
                response.message = "Message cannot be empty.";
                return BadRequest(response);
            }

            response.status = "OK";
            response.message = "Received message: " + message;
            return Ok(response);
        }

        // Petición PUT
        [HttpPut("Update")]
        public ActionResult<Api_response> Put_Example([FromBody] string updatedMessage)
        {
            if (string.IsNullOrEmpty(updatedMessage))
            {
                response.status = "BadRequest";
                response.message = "Updated message cannot be empty.";
                return BadRequest(response);
            }

            response.status = "OK";
            response.message = "Message updated: " + updatedMessage;
            return Ok(response);
        }

        // Petición DELETE
        [HttpDelete("Delete/{id}")]
        public ActionResult<Api_response> Delete_Example(int id)
        {
            if (id <= 0)
            {
                response.status = "BadRequest";
                response.message = "Invalid ID provided.";
                return BadRequest(response);
            }

            response.status = "OK";
            response.message = "Item with ID " + id + " has been deleted.";
            return Ok(response);
        }

        // Petición PATCH
        [HttpPatch("Modify")]
        public ActionResult<Api_response> Patch_Example([FromBody] string patchMessage)
        {
            if (string.IsNullOrEmpty(patchMessage))
            {
                response.status = "BadRequest";
                response.message = "Patch message cannot be empty.";
                return BadRequest(response);
            }

            response.status = "OK";
            response.message = "Message patched: " + patchMessage;
            return Ok(response);
        }

    }
}
