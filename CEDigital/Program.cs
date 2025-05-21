using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.Data.SqlClient;
using CEDigital.Data_Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CEDigital.Data_input_models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Agregar servicios al contenedor
builder.Services.AddControllers();
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

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuración de CORS (debe ir antes de Authorization)
app.UseCors("AllowAnyOriginPolicy");

app.UseHttpsRedirection();
app.UseAuthorization(); // Aquí se autoriza el acceso
app.MapControllers(); // Mapea los controladores



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




 // CODIGO DE UN SOLO USO
var MongoDB_create = new MongoDB_create();
await MongoDB_create.CrearBaseYColeccionesAsync();

/*
var inserter = new MongoDataInserter();

var student = new Student
{
    _id = "20250001",
    name = "Carlos",
    last_name = "Rodriguez",
    id_number = "1122334455",
    email = "carlos.rod@example.com",
    username = "Juan",
    password = "123456", // puedes usar hashing aquí si lo deseas
    phone = "2222-3333"
};

var professor = new Professor
{
    _id = "99887766",
    name = "Ana",
    last_name = "Torres",
    email = "ana.torres@example.com",
    username = "ana123",
    password = "123456"
};

var admin = new Admin
{
    _id = "54352346",
    username = "superadmin",
    password = "adminpass"
};

await inserter.InsertAdmin(admin);
await inserter.InsertStudent(student);
await inserter.InsertProfessor(professor);

Console.WriteLine("Datos insertados correctamente.");

*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
