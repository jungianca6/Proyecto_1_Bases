namespace CEDigital.Models
{
    //Clase que representa una carpeta
    public class Folder_model
    {
        public int folder_id { get; set; }
        public string name { get; set; }

        //FK de grupo
        public int group_id { get; set; }



    }
}
