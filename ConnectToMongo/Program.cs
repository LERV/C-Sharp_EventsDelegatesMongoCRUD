using System;


using MongoDB.Bson;
using MongoDB.Driver;

namespace ConnectToMongo
{
    public delegate void TestDelegateHandler(int num);

    class Program
    {
        static void Main(string[] args)
        {
            InsertAsync();
            getAsync();


            //Delegates example
            TestDelegateHandler delegate1 = new TestDelegateHandler(TestDelegateToFunction);
            TestDelegateHandler delegate2 = new TestDelegateHandler(TestDelegateToFunction2);

            Console.WriteLine("Call delegates");
            delegate1(5); //Call TestDelegateToFunction through delete 1
            delegate2(10);  //Call TestDelegateToFunction2 through delete 2
            Console.WriteLine("");


            Console.WriteLine("Call multiple delegates");
            //Call TestDelegateToFunction and TestDelegateToFunction2 through delete 1 (Deep is using an invocation list)
            delegate1 += delegate2; //Multiple delegates

            delegate1(15);

            //NOTE: If using return values with multiple deletes only the last delegate return value is returned
            Console.WriteLine("");


            Console.ReadKey();
        }

        static async void InsertAsync()
        {
            Console.WriteLine("Connecting and Inserting Mongo!");
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("schools");
            var collection = database.GetCollection<Reading>("students");

            await collection.InsertOneAsync(new Reading { name = "Smith", lastname="Titan" } );
        }

        static async void getAsync()
        {
            Console.WriteLine("Connecting and Querying Mongo!");
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("schools");
            var collection = database.GetCollection<Reading>("students");            

            var list = await collection.Find(_ => true).ToListAsync();

            Console.WriteLine("Printing DB results");
            foreach (var document in list)
            {
                Console.WriteLine(document.name + " " + document.lastname);
            }
            Console.WriteLine("");
        }


        //For Delegate example
        static void TestDelegateToFunction(int number)
        {
            Console.WriteLine("Called D1:"+number);
        }

        //For Delegate example
        static void TestDelegateToFunction2(int number)
        {
            Console.WriteLine("Called D2:" + number);
        }
    }
}
