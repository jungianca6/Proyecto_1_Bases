namespace CEDigital.Data_input_models
{
    public class Data_input_add_default_grades
    {
        public string course_code { get; set; }
        public List<string> sections { get; set; }
        public List<double> percentages { get; set; }
    }
}
