namespace CEDigital.Data_output_models
{
    public class News_model
    {
        public string title { get; set; }
        public string message { get; set; }
        public DateTime publish_date { get; set; }
        public string author { get; set; }
    }

    public class Data_output_view_news
    {
        public List<News_model> news_list { get; set; }
    }
}
