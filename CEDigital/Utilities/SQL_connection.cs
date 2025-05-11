using System;
using System.Data;
using Microsoft.Data.SqlClient;
namespace CEDigital.Utilities
{
    public class SQL_connection
    {
        // Cadena de conexión a la base de datos
        public string connection_string = "Server=localhost;Database=AcademicDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // Consulta que se desea ejecutar
        public string query { get; set; }

        // Ejecuta la consulta y devuelve un SqlDataReader con los resultados
        public SqlDataReader Execute_query(out SqlConnection connection)
        {
            // Crear la conexión
            connection = new SqlConnection(connection_string);

            // Crear el comando con la consulta
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                // Abrir la conexión
                connection.Open();

                // Ejecutar la consulta y devolver el lector (reader)
                return command.ExecuteReader(CommandBehavior.CloseConnection); // la conexión se cierra al cerrar el reader
            }
            catch
            {
                // Si hay error, liberar la conexión
                connection.Dispose();
                throw; // volver a lanzar el error
            }
        }
    }


}
