namespace CEDigital.Data_input_models
{
    // Input para el 2do botón (actualizar nota y observaciones)
    public class Data_input_update_grade
    {
        public int group_number { get; set; }
        public string grading_item_name { get; set; }
        public string evaluation_name { get; set; }
        public float grade { get; set; }
        public string observations { get; set; } // texto o nombre archivo
        public bool grades_public { get; set; }
    }
}
