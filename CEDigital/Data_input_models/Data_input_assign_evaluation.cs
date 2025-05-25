namespace CEDigital.Data_input_models
{
    public class Data_input_assign_evaluation
    {
        public string course_code { get; set; }
        public int group_number { get; set; }
        public string grading_item_name { get; set; }
        public string evaluation_name { get; set; }
        public float grading_item_percentage { get; set; }
        public float evaluation_percentage { get; set; }
        public string delivery_date { get; set; }
        public string description { get; set; }
        public string evaluation_type { get; set; } // "grupal" o "individual"
        public string professor_filename { get; set; }
        public string data_base_path_professor { get; set; }
    }
}
