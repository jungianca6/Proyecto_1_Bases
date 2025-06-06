﻿namespace CEDigital.Models
{
    //Clase que representa a un estudiante
    public class Student_model
    {
        public string name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string phone { get; set; }

        // Relación con grupos
        public List<int> group_ids { get; set; }
    }
}
