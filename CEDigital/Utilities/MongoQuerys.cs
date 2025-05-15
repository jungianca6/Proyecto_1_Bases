// Clase de servicio para consultas
using MongoDB.Driver;

public class MongoDbQueryService
{
    private readonly IMongoDatabase _database;

    public MongoDbQueryService(string connectionString = "mongodb://localhost:27017", string databaseName = "CEDigital")
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    // Consultar todos los estudiantes
    public async Task<List<Student>> GetAllStudents()
    {
        var collection = _database.GetCollection<Student>("Student");
        return await collection.Find(_ => true).ToListAsync();
    }

    // Consultar todos los profesores
    public async Task<List<Professor>> GetAllProfessors()
    {
        var collection = _database.GetCollection<Professor>("Professor");
        return await collection.Find(_ => true).ToListAsync();
    }

    // Buscar estudiante por nombre
    public async Task<List<Student>> FindStudentsByName(string name)
    {
        var collection = _database.GetCollection<Student>("Student");
        var filter = Builders<Student>.Filter.Eq(s => s.name, name);
        return await collection.Find(filter).ToListAsync();
    }

    // Buscar profesor por nombre de usuario
    public async Task<Professor> FindProfessorByUsername(string username)
    {
        var collection = _database.GetCollection<Professor>("Professor");
        var filter = Builders<Professor>.Filter.Eq(p => p.username, username);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }
}


/*   Ejemplo de uso en el main
 * var queryService = new MongoDbQueryService();

        var allStudents = await queryService.GetAllStudentsAsync();
        Console.WriteLine("Estudiantes:");
        foreach (var s in allStudents)
        {
            Console.WriteLine($"{s.name} {s.last_name} - {s.email}");
        }

        var allprofessors = await queryService.GetAllProfessorsAsync();
        Console.WriteLine("Profesores:");
        foreach (var s in allprofessors)
        {
            Console.WriteLine($"{s.name} {s.last_name} - {s.email}");
        }

        var foundProf = await queryService.FindProfessorByUsernameAsync("juan123");
        if (foundProf != null)
        {
            Console.WriteLine($"Profesor encontrado: {foundProf.name} {foundProf.last_name}");
        }
        else
        {
            Console.WriteLine("Profesor no encontrado.");
        }
*/