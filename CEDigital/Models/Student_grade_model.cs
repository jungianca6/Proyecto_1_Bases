namespace CEDigital.Models
{
    // Modelo para notas por estudiante
    public class Student_grade_model
    {
        public string student_id { get; set; }
        public string student_name { get; set; }
        public Dictionary<string, float> grades_by_rubric { get; set; }  // Rubro => nota
    }
}
