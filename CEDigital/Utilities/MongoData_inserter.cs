using MongoDB.Driver;
using System.Threading.Tasks;

public class MongoDataInserter
{
    private readonly IMongoDatabase _database;

    public MongoDataInserter(string connectionString = "mongodb://localhost:27017", string dbName = "CEDigital")
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(dbName);
    }

    public async Task InsertStudent(Student student)
    {
        var collection = _database.GetCollection<Student>("Student");
        await collection.InsertOneAsync(student);
    }

    public async Task InsertProfessor(Professor professor)
    {
        var collection = _database.GetCollection<Professor>("Professor");
        await collection.InsertOneAsync(professor);
    }

    public async Task InsertAdmin(Admin admin)
    {
        var collection = _database.GetCollection<Admin>("Admins");
        await collection.InsertOneAsync(admin);
    }
}

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

public class Professor
{
    public string _id { get; set; }
    public string name { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}

public class Admin
{
    public string _id { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}

/*
 * var inserter = new MongoDataInserter();

        var student = new Student
        {
            _id = "20250001",
            name = "Carlos",
            last_name = "Rodriguez",
            id_number = "1122334455",
            email = "carlos.rod@example.com",
            username = "admin",
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
            username = "superadmin",
            password = "adminpass"
        };

        await inserter.InsertAdmin(admin);
        await inserter.InsertStudent(student);
        await inserter.InsertProfessor(professor);
        
        Console.WriteLine("Datos insertados correctamente.");
*/