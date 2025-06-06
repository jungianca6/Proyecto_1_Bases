using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

public class MongoDbQueryService
{
    private readonly IMongoDatabase _database;

    public MongoDbQueryService(string connectionString = "mongodb://localhost:27017", string databaseName = "CEDigital")
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public async Task<(BsonDocument?, string)> FindUser(string username, string password)
    {
        // Buscar en Student
        var studentCollection = _database.GetCollection<BsonDocument>("Student");
        var studentFilter = Builders<BsonDocument>.Filter.Eq("username", username) &
                            Builders<BsonDocument>.Filter.Eq("password", password);

        var studentDoc = await studentCollection.Find(studentFilter).FirstOrDefaultAsync();
        if (studentDoc != null) return (studentDoc, "Student");

        // Buscar en Professor
        var professorCollection = _database.GetCollection<BsonDocument>("Professor");
        var professorFilter = Builders<BsonDocument>.Filter.Eq("username", username) &
                              Builders<BsonDocument>.Filter.Eq("password", password);

        var professorDoc = await professorCollection.Find(professorFilter).FirstOrDefaultAsync();
        if (professorDoc != null) return (professorDoc, "Professor");

        // Buscar en Admins
        var adminCollection = _database.GetCollection<BsonDocument>("Admins");
        var adminFilter = Builders<BsonDocument>.Filter.Eq("username", username) &
                          Builders<BsonDocument>.Filter.Eq("password", password);

        var adminDoc = await adminCollection.Find(adminFilter).FirstOrDefaultAsync();
        if (adminDoc != null) return (adminDoc, "Admin");

        // No encontrado
        return (null, "");
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