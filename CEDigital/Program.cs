using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



/*
 * Este bloque de código realiza una consulta a la base de datos para obtener la lista de estudiantes,
 * convierte cada fila del resultado en un objeto Student_model, y luego imprime la información.
 */

// Crear una instancia de conexión a la base de datos
SQL_connection sQL_Connection = new SQL_connection();

// Definir la consulta SQL que se va a ejecutar
sQL_Connection.query = "SELECT student_id, name, last_name, email FROM Student";

// Crear la lista donde se almacenarán los estudiantes obtenidos
List<Student_model> student_list = new List<Student_model>();

try
{
    // Usar un bloque 'using' para manejar automáticamente la conexión
    using (SqlConnection connection = new SqlConnection(sQL_Connection.connection_string))
    {
        connection.Open(); // Abrir la conexión

        // Crear el comando SQL con la consulta y la conexión
        using (SqlCommand command = new SqlCommand(sQL_Connection.query, connection))
        using (SqlDataReader reader = command.ExecuteReader()) // Ejecutar el comando y obtener un lector de datos
        {
            // Leer cada fila del resultado
            while (reader.Read())
            {
                // Crear un objeto Student_model con los datos de la fila
                Student_model student = new Student_model
                {
                    student_id = reader["student_id"].ToString(),
                    name = reader["name"].ToString(),
                    last_name = reader["last_name"].ToString(),
                    email = reader["email"].ToString()
                };

                // Agregar el estudiante a la lista
                student_list.Add(student);
            }
        }
    }

    // Mostrar los estudiantes en la consola
    foreach (var student in student_list)
    {
        Console.WriteLine($"ID: {student.student_id}, Nombre: {student.name} {student.last_name}, Email: {student.email}");
    }
}
catch (Exception ex)
{
    // Mostrar el mensaje de error en caso de que ocurra una excepción
    Console.WriteLine("Error: " + ex.Message);
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
