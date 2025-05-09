namespace CEDigital.Models
{
    //Clase que representa a un profesor
    public class Professor_model
    {
        public string id_number { get; set; }
        public string name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        // Relación con grupos
        public List<int> group_ids { get; set; }

        // Relación con noticias
        public List<int> news_ids { get; set; }
    }
}
