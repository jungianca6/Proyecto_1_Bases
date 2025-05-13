using MongoDB.Driver;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


class Program
{
    static async Task Main(string[] args)
    {
        var connectionString = "mongodb://localhost:27017";
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("CEDigital");

        // Insertar estudiante
        var student = new Student
        {
            _id = "20250001",
            name = "Carlos",
            last_name = "Rodriguez",
            id_number = "1122334455",
            email = "carlos.rod@example.com",
            username = "admin",
            password = Encriptador.ObtenerHashMD5("sapo32"),
            phone = "2222-3333"

        };

        await InsertarEstudiante(database, student);

        // Insertar profesor
        var professor = new Professor
        {
            _id = "99887766",
            name = "Ana Torres",
            last_name = "Rodriguez",
            email = "ana.torres@example.com",
            username = "admin",
            password = Encriptador.ObtenerHashMD5("sapo23")
        };

        await InsertarProfesor(database, professor);

        Console.WriteLine("Datos insertados correctamente.");
    }

    // Función para insertar estudiante
    public static async Task InsertarEstudiante(IMongoDatabase db, Student estudiante)
    {
        var coleccion = db.GetCollection<Student>("Student");
        await coleccion.InsertOneAsync(estudiante);
    }

    // Función para insertar profesor
    public static async Task InsertarProfesor(IMongoDatabase db, Professor profesor)
    {
        var coleccion = db.GetCollection<Professor>("Professor");
        await coleccion.InsertOneAsync(profesor);
    }
}

// Clase Estudiante
public class Student
{
    public string _id { get; set; }
    public string name { get; set; }
    public string last_name { get; set; }
    public string id_number { get; set; }
    public string email { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string phone { get; set; }
}

// Clase Profesor
public class Professor
{
    public string _id { get; set; }
    public string name { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}



public class Encriptador
{
    public static string ObtenerHashMD5(string input)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);

            // Convertir el arreglo de bytes a string hexadecimal
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
    }
}