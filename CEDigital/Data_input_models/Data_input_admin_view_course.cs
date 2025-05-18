namespace CEDigital.Data_input_models
{
    // Visualizar curso (entrada)
    public class Data_input_admin_view_course
    {
        public string course_code { get; set; }
        public string name { get; set; }              // Vacío en petición
        public int? credits { get; set; }             // Vacío (nullable)
        public string career { get; set; }            // Vacío
        public List<int> group_ids { get; set; }       // Lista de grupos asociados
        public List<int> professors_ids { get; set; }   // Vacío o null
    }

}
