using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongodbBugReport_ArithmeticException
{
    class Program
    {
        private static MongoClient client;

        static void Main(string[] args)
        {
            string remoteServer = "nsmongodb01";
            client = new MongoClient("mongodb://" + remoteServer + ":27017");
            ProbelmaticCall();
            //NonProbelmaticCall();
            Console.WriteLine("Reached here all is OK");
            Console.ReadKey();
        }

        static void ProbelmaticCall()
        {
            FloatingPointControl.TurnOnFpExceptions();
            var list = client.GetDatabaseNames().ToList();
        }
        static void NonProbelmaticCall()
        {
            var list = client.GetDatabaseNames().ToList();
        }
    }

    static class Extension
    {
        public static IEnumerable<string> GetDatabaseNames(this IMongoClient client)
        {

            return client.ListDatabases()
                .ToList()
                .Select(db => db.GetValue("name").AsString);
        }
    }
}
