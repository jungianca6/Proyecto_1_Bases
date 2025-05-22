namespace CEDigital.Models
{
    //Clase que representa una entrega de tarea
    public class Evaluation_model
    {
        public string evaluation_id { get; set; }
        public string filename { get; set; }

        public string path { get; set; }
        public string delivery_date { get; set; } // Formato: DD/MM/YYYY
        public string delivery_time { get; set; } // Formato: HH:MM
        public bool is_group { get; set; }

        //FK de rubro
        public int grading_item_id { get; set; }
    }
}
