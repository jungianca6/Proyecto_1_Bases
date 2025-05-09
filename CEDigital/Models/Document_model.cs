namespace CEDigital.Models
{
    //Clase que representa un documento
    public class Document_model
    {
        public int document_id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public float size { get; set; }
        public string upload_date { get; set; } // Formato: DD/MM/YYYY HH:MM
        public bool uploaded_by_professor { get; set; }

        //FK de carpeta
        public int folder_id { get; set; }
    }
}
