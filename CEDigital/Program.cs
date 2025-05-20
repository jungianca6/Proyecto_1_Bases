using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.Data.SqlClient;
using CEDigital.Data_Base;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOriginPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configuración de CORS (debe ir antes de Authorization)
app.UseCors("AllowAnyOriginPolicy");

app.UseHttpsRedirection();
app.UseAuthorization(); // Aquí se autoriza el acceso
app.MapControllers(); // Mapea los controladores



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





SQL_connection db = new SQL_connection();

string query1 = "SELECT name FROM Course";
SqlConnection connection1;

try
{
    using (SqlCommand command = new SqlCommand(query1))
    {
        using (SqlDataReader reader = db.Execute_query(command, out connection1))
        {
            Console.WriteLine("Lista de cursos:");

            while (reader.Read())
            {
                // Leer el campo 'name'
                string courseName = reader["name"].ToString();
                Console.WriteLine("- " + courseName);
            }

            reader.Close(); // Esto cerrará también la conexión automáticamente
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("Ocurrió un error al ejecutar la consulta: " + ex.Message);
}



/*
 * CODIGO DE UN SOLO USO
var MongoDB_create = new MongoDB_create();
await MongoDB_create.CrearBaseYColeccionesAsync();
*/


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
