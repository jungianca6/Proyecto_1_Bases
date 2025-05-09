namespace CEDigital.Models
{
    //Clase que representa un grupo
    public class Group_model
    {
        public int group_id { get; set; }
        public string group_number { get; set; }

        // Relación con estudiantes
        public List<string> student_ids { get; set; }

        // Relación con profesor
        public List<string> professor_ids { get; set; }

        // Relación con rubros
        public List<int> grading_item_ids { get; set; }

        // Relación con carpetas
        public List<int> folder_ids { get; set; }
    }
}
