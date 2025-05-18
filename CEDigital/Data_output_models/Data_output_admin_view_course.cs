namespace CEDigital.Data_output_models
{
    public class Data_output_admin_view_course
    {
        public int course_id { get; set; }            // Id interno de base de datos
        public string course_code { get; set; }
        public string name { get; set; }
        public int credits { get; set; }
        public string career { get; set; }
        public List<int> group_ids { get; set; }             // Lista de grupos asociados
        public List<int> professors_ids { get; set; }   // Vacío o null
    }
}
