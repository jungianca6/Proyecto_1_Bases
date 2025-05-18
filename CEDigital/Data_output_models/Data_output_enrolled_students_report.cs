namespace CEDigital.Data_output_models
{
    // Modelo de estudiante matriculado
    public class Enrolled_student_model
    {
        public string student_id { get; set; }
        public string student_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    // Clase para encapsular la lista de estudiantes
    public class Data_output_enrolled_students_report
    {
        public List<Enrolled_student_model> students { get; set; }
    }
}
