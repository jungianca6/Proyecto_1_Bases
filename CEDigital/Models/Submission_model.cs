namespace CEDigital.Models
{
    //Clase que representa una entrega de tarea
    public class Submission_model
    {
        public int submission_id { get; set; }
        public string file { get; set; }
        public string delivery_date { get; set; } // Formato: DD/MM/YYYY
        public string delivery_time { get; set; } // Formato: HH:MM
        public bool is_group { get; set; }

        //FK de rubro
        public int grading_item_id { get; set; }
    }
}
