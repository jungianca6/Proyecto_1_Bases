namespace CEDigital.Data_input_models
{
    public class Data_input_submit_evaluation
    {
        public string student_id { get; set; }            // Carnet del estudiante
        public string course_code { get; set; }           // Código del curso
        public int group_number { get; set; }             // Número de grupo
        public string grading_item_name { get; set; }     // Nombre del rubro
        public string evaluation_title { get; set; }      // Título de la evaluación
        public string filename_evaluation { get; set; }             // Nombre del archivo
        public string evaluation_filename { get; set; }             // Ruta donde se almacenó el archivo
    }
}
