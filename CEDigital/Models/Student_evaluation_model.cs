namespace CEDigital.Models
{
    public class Student_evaluation_model
    {
        public string rubric_name { get; set; }
        public string evaluation_title { get; set; }
        public float grade { get; set; }
        public string feedback { get; set; }
        public bool is_public { get; set; }
        public DateTime evaluation_date { get; set; }
    }
}
