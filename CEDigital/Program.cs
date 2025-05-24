using CEDigital.Models;
using CEDigital.Utilities;
using Microsoft.Data.SqlClient;
using CEDigital.Data_Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CEDigital.Data_input_models;
using Microsoft.AspNetCore.Hosting.Server;

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

var inserter = new MongoDataInserter();
var students = new List<Student>
{
    new Student
    {
        _id = "2023065165",
        name = "Carlos",
        last_name = "Rodriguez",
        id_number = "1122334455",
        email = "carlos.rod@example.com",
        username = "CarloselGrande",
        password = Encriptador.ObtenerHashMD5("123456"),
        phone = "2222-3333"
    },
    new Student
    {
        _id = "2023493175",
        name = "María",
        last_name = "Gómez",
        id_number = "2233445566",
        email = "maria.gomez@example.com",
        username = "MariaMagdalena",
        password = Encriptador.ObtenerHashMD5("abcdef"),
        phone = "3333-4444"
    },
    new Student
    {
        _id = "2024983216",
        name = "Luis",
        last_name = "Pérez",
        id_number = "3344556677",
        email = "luis.perez@example.com",
        username = "LuisVI",
        password = Encriptador.ObtenerHashMD5("654321"),
        phone = "4444-5555"
    },
    new Student
    {
        _id = "2020697822",
        name = "Sofía",
        last_name = "Ramírez",
        id_number = "4455667788",
        email = "sofia.ramirez@example.com",
        username = "ReinaSofia",
        password = Encriptador.ObtenerHashMD5("pass123"),
        phone = "5555-6666"
    },
    new Student
    {
        _id = "2016698746",
        name = "Juan",
        last_name = "Martínez",
        id_number = "5566778899",
        email = "juan.martinez@example.com",
        username = "Juan",
        password = Encriptador.ObtenerHashMD5("juan123"),
        phone = "6666-7777"
    }
};

var professors = new List<Professor>
{
    new Professor
    {
        _id = "301849792",
        name = "Ana",
        last_name = "Torres",
        email = "ana.torres@example.com",
        username = "Anabolico",
        password = Encriptador.ObtenerHashMD5("123456")
    },
    new Professor
    {
        _id = "194875095",
        name = "Pedro",
        last_name = "López",
        email = "pedro.lopez@example.com",
        username = "PedroPicapiedra",
        password = Encriptador.ObtenerHashMD5("lopezpass")
    },
    new Professor
    {
        _id = "406284097",
        name = "Lucía",
        last_name = "Fernández",
        email = "lucia.fernandez@example.com",
        username = "Lucifer",
        password = Encriptador.ObtenerHashMD5("lucia123")
    },
    new Professor
    {
        _id = "256894765",
        name = "Jorge",
        last_name = "Gutierrez",
        email = "jorge.mendez@example.com",
        username = "koki",
        password = Encriptador.ObtenerHashMD5("jorgito")
    },
    new Professor
    {
        _id = "100000001",
        name = "Paula",
        last_name = "Núñez",
        email = "paula.nunez@example.com",
        username = "Paula",
        password = Encriptador.ObtenerHashMD5("paula456")
    }
};
foreach (var student in students)
{
    await inserter.InsertStudent(student);
}
foreach (var professor in professors)
{
    await inserter.InsertProfessor(professor);
}
var admin = new Admin
{
    username = "superadmin",
    password = Encriptador.ObtenerHashMD5("adminpass")
};

await inserter.InsertAdmin(admin);

Console.WriteLine("Todos los datos fueron insertados correctamente.");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


/*
  
-- Insertar estudiantes
INSERT INTO Student (student_id) VALUES (2023065165);
INSERT INTO Student (student_id) VALUES (2023493175);
INSERT INTO Student (student_id) VALUES (2024983216);
INSERT INTO Student (student_id) VALUES (2020697822);
INSERT INTO Student (student_id) VALUES (2016698746);

-- Insertar profesores
INSERT INTO Professor (id_number) VALUES (301849792);
INSERT INTO Professor (id_number) VALUES (194875095);
INSERT INTO Professor (id_number) VALUES (406284097);
INSERT INTO Professor (id_number) VALUES (256894765);
INSERT INTO Professor (id_number) VALUES (100000001);

-- Student_Group
INSERT INTO Student_Group (student_id, group_id, course_code) VALUES 
(2023065165, 1, 'MA1101'),
(2023493175, 1, 'MA1101'),
(2024983216, 2, 'FI1402'),
(2016698746, 3, 'CE3102'),
(2020697822, 3, 'CE3102');

-- Professor_Group
INSERT INTO Professor_Group (id_number, group_id) VALUES 
(301849792, 1),
(406284097, 2),
(256894765, 3);

*/