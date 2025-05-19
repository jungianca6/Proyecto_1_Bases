using System;
using System.Data;
using Microsoft.Data.SqlClient;
namespace CEDigital.Utilities
{
    public class SQL_connection
    {
        // Cadena de conexión a la base de datos
        public string connection_string = "Server=localhost;Database=CEDigital;Trusted_Connection=True;TrustServerCertificate=True;";

        // Ejecuta la consulta y devuelve un SqlDataReader con los resultados
        public SqlDataReader Execute_query(string query, out SqlConnection connection)
        {
            // Validar que la consulta no esté vacía
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentException("La consulta no puede ser nula o vacía.");
            }

            // Crear la conexión
            connection = new SqlConnection(connection_string);

            // Crear el comando con la consulta
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                // Abrir la conexión
                connection.Open();
                Console.WriteLine("Base conectada: " + connection.Database);

                // Ejecutar la consulta y devolver el lector (reader)
                return command.ExecuteReader(CommandBehavior.CloseConnection); // la conexión se cierra al cerrar el reader
            }
            catch (SqlException sqlEx)
            {
                // Capturar errores específicos de SQL
                Console.WriteLine($"Error de SQL: {sqlEx.Message}");
                connection.Dispose();
                throw; // volver a lanzar el error
            }
            catch (Exception ex)
            {
                // Capturar otros errores generales
                Console.WriteLine($"Error: {ex.Message}");
                connection.Dispose();
                throw; // volver a lanzar el error
            }
        }
    }


}
