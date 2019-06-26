using MongoDB.Driver;
using System;

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
            Console.ReadKey();
        }

        static void ProbelmaticCall()
        {
            FloatingPointControl.TurnOnFpExceptions();
            var list = client.ListDatabaseNames().ToList();
        }
        static void NonProbelmaticCall()
        {
            var list = client.ListDatabaseNames().ToList();
        }

    }
}
