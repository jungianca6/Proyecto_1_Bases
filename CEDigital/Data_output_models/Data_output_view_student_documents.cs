using CEDigital.Models;

namespace CEDigital.Data_output_models
{

    public class Data_output_view_student_documents
    {
        public List<Student_document_model> documents { get; set; }
    }

    // Modelo para notas por estudiante
    public class Student_document_model
    {
        public int document_id { get; set; }
        public string filename { get; set; }
        public string file_data_base_path { get; set; }
        public string upload_date { get; set; } // Formato: DD/MM/YYYY HH:MM
        public bool uploaded_by_professor { get; set; }

        //FK de carpeta
        public int folder_id { get; set; }

        public int folder_name { get; set; }


    }
}
