using CEDigital.Models;

namespace CEDigital.Data_input_models
{
    public class Data_input_delete_grading_item
    {
        public string course_code { get; set; }
        public int group_number { get; set; }
        public List<string> name_grading_items { get; set; }
    }

}
