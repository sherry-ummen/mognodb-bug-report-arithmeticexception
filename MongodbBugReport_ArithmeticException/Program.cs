using MongoDB.Driver;
using System;

namespace MongodbBugReport_ArithmeticException
{
    class Program
    {
        static void Main(string[] args)
        {
            string remoteServer = "nsmongodb01";
            var client = new MongoClient("mongodb://" + remoteServer + ":27017");
            var list = client.ListDatabaseNames().ToList();
            Console.ReadKey();
        }
    }
}
