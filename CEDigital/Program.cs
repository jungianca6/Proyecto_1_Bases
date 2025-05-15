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

// Crear la conexión
SQL_connection sql = new SQL_connection();

// Definir la consulta
sql.query = "SELECT student_id, name, last_name FROM Student";

// Variable para recibir la conexión abierta
SqlConnection connection;

try
{
    // Ejecutar la consulta
    using (SqlDataReader reader = sql.Execute_query(out connection))
    {
        // Leer cada fila del resultado
        while (reader.Read())
        {
            // Obtener los valores por nombre de columna
            string id = reader["student_id"].ToString();
            string name = reader["name"].ToString();
            string lastName = reader["last_name"].ToString();

            // Mostrar datos
            Console.WriteLine($"Estudiante: {id} - {name} {lastName}");
        }
    } // Al cerrar el reader, también se cierra la conexión
}
catch (Exception ex)
{
    // Mostrar error si ocurre
    Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
}


Console.WriteLine("// ---------------- SELECT sin WHERE ni ORDER BY ----------------");
Sql_query_builder_model select_1 = new Sql_query_builder_model("Student", "student_id", "name");
Console.WriteLine(select_1.build_select());

Console.WriteLine("\n// ---------------- SELECT con WHERE ----------------");
Sql_query_builder_model select_2 = new Sql_query_builder_model("Student", "name", "email");
select_2.add_where("email LIKE '%@gmail.com'");
Console.WriteLine(select_2.build_select());

Console.WriteLine("\n// ---------------- SELECT con ORDER BY ----------------");
Sql_query_builder_model select_3 = new Sql_query_builder_model("Student", "student_id", "name", "last_name");
select_3.set_order_by("name", "DESC");
Console.WriteLine(select_3.build_select());

Console.WriteLine("\n// ---------------- SELECT con JOIN ----------------");
Sql_query_builder_model select_4 = new Sql_query_builder_model("Student", "Student.student_id", "name", "course_name");
select_4.add_join("INNER", "Enrollment", "Student.student_id = Enrollment.student_id");
select_4.add_join("LEFT", "Course", "Enrollment.course_id = Course.course_id");
Console.WriteLine(select_4.build_select());

Console.WriteLine("\n// ---------------- SELECT con TODO: JOIN, WHERE, ORDER BY ----------------");
Sql_query_builder_model select_5 = new Sql_query_builder_model("Student", "Student.student_id", "name", "email");
select_5.add_join("INNER", "Enrollment", "Student.student_id = Enrollment.student_id");
select_5.add_where("email LIKE '%@example.com'");
select_5.add_where("name = 'Ana'");
select_5.set_order_by("student_id");
Console.WriteLine(select_5.build_select());

Console.WriteLine("\n// ---------------- INSERT ----------------");
Sql_query_builder_model insert_1 = new Sql_query_builder_model("Student");
insert_1.set_insert_value("name", "Pedro");
insert_1.set_insert_value("last_name", "Ramirez");
insert_1.set_insert_value("email", "pedro.ramirez@example.com");
Console.WriteLine(insert_1.build_insert());

Console.WriteLine("\n// ---------------- UPDATE con WHERE ----------------");
Sql_query_builder_model update_1 = new Sql_query_builder_model("Student");
update_1.set_update_value("email", "nuevo@email.com");
update_1.add_where("student_id = 4");
Console.WriteLine(update_1.build_update());

Console.WriteLine("\n// ---------------- UPDATE sin WHERE ----------------");
Sql_query_builder_model update_2 = new Sql_query_builder_model("Student");
update_2.set_update_value("email", "mass_update@email.com");
Console.WriteLine(update_2.build_update());

Console.WriteLine("\n// ---------------- DELETE con WHERE ----------------");
Sql_query_builder_model delete_1 = new Sql_query_builder_model("Student");
delete_1.add_where("student_id = 10");
Console.WriteLine(delete_1.build_delete());

Console.WriteLine("\n// ---------------- DELETE sin WHERE (PELIGROSO) ----------------");
Sql_query_builder_model delete_2 = new Sql_query_builder_model("Student");
Console.WriteLine(delete_2.build_delete());



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
