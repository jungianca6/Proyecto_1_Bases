namespace CEDigital.Data_output_models
{
    public class Student_grade_detail_model
    {
        public string rubric_name { get; set; }
        public string evaluation_title { get; set; }
        public float grade { get; set; }
    }

    public class Data_output_student_grade_report
    {
        public List<Student_grade_detail_model> evaluations { get; set; }
    }

}
