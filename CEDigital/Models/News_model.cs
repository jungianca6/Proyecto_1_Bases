namespace CEDigital.Models
{
    //Clase que representa una noticia
    public class News_model
    {
        public int news_id { get; set; }
        public string message { get; set; }
        public string title { get; set; }
        public string course_code { get; set; }

        public DateTime publication_date { get; set; }

        public string author { get; set; }
    }

}
