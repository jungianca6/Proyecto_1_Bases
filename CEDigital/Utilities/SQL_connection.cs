using System;
using System.Data.SqlClient;
namespace CEDigital.Utilities
{
    public class SQL_connection
    {
        public string connection_string = "Server=localhost;Database=AcademicDB;Trusted_Connection=True;TrustServerCertificate=True;";
        public string query { get; set; }

    }
}
