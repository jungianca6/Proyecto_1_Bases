namespace CEDigital.Data_input_models
{
    public class Data_input_add_student_to_group
    {
        public int student_id { get; set; }          // Cambiado de string a int
        public int group_number { get; set; }
        public string course_code { get; set; }
    }
}
