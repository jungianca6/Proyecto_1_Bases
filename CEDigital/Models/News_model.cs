namespace CEDigital.Models
{
    //Clase que representa una noticia
    public class News_model
    {
        public int news_id { get; set; }
        public string message { get; set; }
        public string title { get; set; }

        //FK de profesor
        public string professor_id { get; set; }
    }

}
