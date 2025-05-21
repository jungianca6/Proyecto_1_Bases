using System;
using System.Data;
using Microsoft.Data.SqlClient;
using MongoDB.Driver.Core.Configuration;
namespace CEDigital.Utilities
{
    public class SQL_connection
    {
        // Cadena de conexión a la base de datos
        public string connection_string = "Server=PcKoki;Database=CEDigital;Trusted_Connection=True;TrustServerCertificate=True;";

        // Ejecuta la consulta y devuelve un SqlDataReader con los resultados
        public SqlDataReader Execute_query(SqlCommand command, out SqlConnection connection)
        {
            connection = new SqlConnection(connection_string);
            command.Connection = connection;
            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public void Execute_non_query(SqlCommand command)
        {
            using (var connection = new SqlConnection(connection_string))
            {
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }


}
