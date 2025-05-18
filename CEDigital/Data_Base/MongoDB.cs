using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace CEDigital.Data_Base
{
    public class MongoDB_create
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoDB_create(string connectionString = "mongodb://localhost:27017", string databaseName = "CEDigital")
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public async Task CrearBaseYColeccionesAsync()
        {
            // Crear colecciones (si no existen, CreateCollectionAsync las crea)
            await _database.CreateCollectionAsync("Student");
            await _database.CreateCollectionAsync("Professor");

            Console.WriteLine("Base de datos y colecciones creadas correctamente.");
        }
    }

    /*                Ejemplo en para ponerlo en Main
     *                
        var MongoDB_create = new MongoDB_create();
            await mongoSetup.CrearBaseYColeccionesAsync();
    */
}