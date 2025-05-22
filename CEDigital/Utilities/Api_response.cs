namespace CEDigital.Utilities
{
    public class Api_response
    {
        public string status { get; set; }  // Estado de la respuesta
        public object message { get; set; } // Mensaje de la respuesta, que puede ser un string o un objeto

        // Constructor para facilitar la creación de respuestas
        public Api_response(string status, object message)
        {
            this.status = status;
            this.message = message;
        }

    }
}
