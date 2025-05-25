namespace CEDigital.Data_input_models
{
    public class Data_input_submit_evaluation2
    {


        public string student_id { get; set; }           // Carnet del estudiante
        public string course_code { get; set; }          // Código del curso
        public int group_number { get; set; }            // Número de grupo
        public string grading_item_name { get; set; }    // Nombre del grading item
        public string evaluation_name { get; set; }      // Nombre exacto de la evaluación

        // Ruta donde se almacenó el archivo
    }
}
