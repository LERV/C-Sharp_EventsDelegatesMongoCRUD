using System;


using MongoDB.Bson;
using MongoDB.Driver;

namespace ConnectToMongo
{
    class Program
    {
        static void Main(string[] args)
        {
            InsertAsync();
            getAsync();

            Console.ReadKey();
        }

        static async void InsertAsync()
        {
            Console.WriteLine("Connecting and Inserting!");
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("schools");
            var collection = database.GetCollection<Reading>("students");

            await collection.InsertOneAsync(new Reading { name = "Smith", lastname="Titan" } );
        }

        static async void getAsync()
        {
            Console.WriteLine("Connecting and Querying!");
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("schools");
            var collection = database.GetCollection<Reading>("students");            

            var list = await collection.Find(_ => true).ToListAsync();

            foreach (var document in list)
            {
                Console.WriteLine(document.name + " " + document.lastname);
            }
        }
    }
}
