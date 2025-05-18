namespace CEDigital.Data_output_models
{
    public class Student_document_model
    {
        public string title { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
        public DateTime upload_date { get; set; }
    }

    public class Data_output_view_student_documents
    {
        public List<Student_document_model> documents { get; set; }
    }
}
