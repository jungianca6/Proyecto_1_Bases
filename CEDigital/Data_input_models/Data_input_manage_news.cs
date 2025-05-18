namespace CEDigital.Data_input_models
{
    // Input para agregar, editar o eliminar noticias
    public class Data_input_manage_news
    {
        public string title { get; set; }
        public string message { get; set; }
        public DateTime publication_date { get; set; }
        public string author { get; set; }
        public int course_code { get; set; }
    }
}
