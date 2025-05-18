namespace CEDigital.Data_output_models
{
    public class Data_output_login
    {
        public string username { get; set; }      // Usuario enviado
        public string password { get; set; }      // Contraseña enviada
        public string user_type { get; set; }     // Tipo de usuario vacío (estudiante, profesor, admin)
        public string primary_key { get; set; }   // Carnet o PK vacío
    }

}
