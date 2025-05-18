namespace CEDigital.Data_output_models
{
    // Modelo para notas por estudiante
    public class Student_grade_model
    {
        public string student_id { get; set; }
        public string student_name { get; set; }
        public Dictionary<string, float> grades_by_rubric { get; set; }  // Rubro => nota
    }

    // Clase para encapsular la lista de notas
    public class Data_output_grades_report
    {
        public List<Student_grade_model> grades { get; set; }
    }
}
