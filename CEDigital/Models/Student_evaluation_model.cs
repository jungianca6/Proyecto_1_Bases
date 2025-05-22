namespace CEDigital.Models
{

    // Entrega
    public class Student_evaluation_model
    {
        public string rubric_name { get; set; }
        public string evaluation_title { get; set; }
        public float grade { get; set; }
        public string feedback { get; set; }
        public bool is_public { get; set; }
        public DateTime evaluation_date { get; set; }

        public string data_base_path_evalution { get; set; }

        public string data_base_path_professor { get; set; }

        public string evaluation_filename { get; set; }

        public string professor_filename { get; set; }

    }
}
