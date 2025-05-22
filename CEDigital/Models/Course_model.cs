namespace CEDigital.Models
{
    //Clase que representa un curso
    public class Course_model
    {
        public string name { get; set; }
        public string course_code { get; set; }
        public string career { get; set; }
        public int credits { get; set; }

        // Relación con semestres
        public List<int> semester_ids { get; set; }
    }
}
