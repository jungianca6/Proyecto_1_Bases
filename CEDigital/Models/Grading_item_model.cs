namespace CEDigital.Models
{
    //Clase que representa un rubro de evaluación
    public class Grading_item_model
    {
        public int grading_item_id { get; set; }
        public string name { get; set; }
        public float percentage { get; set; }

        //FK de grupo
        public int group_id { get; set; }

        // Relación con entregas
        public List<int> submission_ids { get; set; }
    }
}
