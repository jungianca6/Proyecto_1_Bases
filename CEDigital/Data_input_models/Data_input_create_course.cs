namespace CEDigital.Data_input_models
{
    public class Data_input_admin_create_course
    {
        public string course_code { get; set; }       // Código del curso
        public string name { get; set; }              // Nombre del curso
        public int credits { get; set; }              // Cantidad de créditos
        public string career { get; set; }            // Carrera a la que pertenece
        public List<int> group_ids { get; set; }             // Lista de grupos asociados
        public List<int> professors_ids { get; set; }  // Semestres asociados (en vez de profesores, según tu modelo)
    }

}
