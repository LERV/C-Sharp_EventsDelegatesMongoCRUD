using System;


using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading;

namespace ConnectToMongo
{
    public delegate void TestDelegateHandler(int num);
    

    class Program
    {
        private static bool keepRunning = true;
        static void Main(string[] args)
        {            


            Thread t = new Thread(new ThreadStart(getAsync));
            t.Start();
            //Task
            //getAsync();


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
            Console.WriteLine("-----------------------------------------");

            for (int i = 0; i < 10; i++)
            {
                Console.Write("Write name: ");
                String name = Console.ReadLine();
                InsertAsync(name);
                
            }       


            Console.ReadKey();
        }

        static async void InsertAsync(String name)
        {
            Console.WriteLine("Connecting and Inserting Mongo!");
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("schools");
            var collection = database.GetCollection<Reading>("students");

            await collection.InsertOneAsync(new Reading { name = name, lastname="Janzing" } );
        }

        static void getAsync()
        {
            Console.WriteLine("Connecting and Querying Mongo!");
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("schools");
            var collection = database.GetCollection<Reading>("students");

            while(keepRunning)
            {
                Console.WriteLine("*************************Readings********************");
                //var list = await collection.Find(_ => true).ToListAsync();
                var list = collection.Find(_ => true).ToList();

                Console.WriteLine("Printing DB results");
                foreach (var document in list)
                {
                    Console.WriteLine(document.name + " " + document.lastname);
                }
                Console.WriteLine("**********************************************");
                Thread.Sleep(5000);
            }
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
