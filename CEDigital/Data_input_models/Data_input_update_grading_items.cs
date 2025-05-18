using CEDigital.Models;

namespace CEDigital.Data_input_models
{
    public class Data_input_update_grading_items
    {
        public string course_code { get; set; }
        public int group_number { get; set; }
        public List<Grading_item_model> grading_items { get; set; }
    }

}
