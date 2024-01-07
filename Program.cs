using EscapeFromTheWoods.MongoDB.Repo;
using EscapeFromTheWoods.Objects;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EscapeFromTheWoods
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Hello World!");

            string connectionStringMongoDB = "mongodb://localhost:27017";

            MongoDBRepository mongoDBRepository = new MongoDBRepository(connectionStringMongoDB);

            string path = @"D:\Hogent\Semester_3\PS\Opdrachten\Opdracht 3 - Refactoring\EscapeFromTheWoodsToRefactor\monkeys";
            Map m1 = new Map(0, 500, 0, 500);
            Wood w1 = WoodBuilder.GetWood(500, m1, path, mongoDBRepository);
            w1.PlaceMonkey("Alice", IDgenerator.GetMonkeyID());
            w1.PlaceMonkey("Janice", IDgenerator.GetMonkeyID());
            w1.PlaceMonkey("Toby", IDgenerator.GetMonkeyID());
            w1.PlaceMonkey("Mindy", IDgenerator.GetMonkeyID());
            w1.PlaceMonkey("Jos", IDgenerator.GetMonkeyID());

            Map m2 = new Map(0, 200, 0, 400);
            Wood w2 = WoodBuilder.GetWood(2500, m2, path, mongoDBRepository);
            w2.PlaceMonkey("Tom", IDgenerator.GetMonkeyID());
            w2.PlaceMonkey("Jerry", IDgenerator.GetMonkeyID());
            w2.PlaceMonkey("Tiffany", IDgenerator.GetMonkeyID());
            w2.PlaceMonkey("Mozes", IDgenerator.GetMonkeyID());
            w2.PlaceMonkey("Jebus", IDgenerator.GetMonkeyID());

            Map m3 = new Map(0, 400, 0, 400);
            Wood w3 = WoodBuilder.GetWood(2000, m3, path, mongoDBRepository);
            w3.PlaceMonkey("Kelly", IDgenerator.GetMonkeyID());
            w3.PlaceMonkey("Kenji", IDgenerator.GetMonkeyID());
            w3.PlaceMonkey("Kobe", IDgenerator.GetMonkeyID());
            w3.PlaceMonkey("Kendra", IDgenerator.GetMonkeyID());

            await Task.WhenAll(
                Task.Run(async () =>
                {
                    await w1.WriteWoodToDBAsync();
                    await w1.EscapeAsync(m1);
                }),
                Task.Run(async () =>
                {
                    await w2.WriteWoodToDBAsync();
                    await w2.EscapeAsync(m2);
                }),
                Task.Run(async () =>
                {
                    await w3.WriteWoodToDBAsync();
                    await w3.EscapeAsync(m3);
                })
            );

            //await w1.WriteWoodToDBAsync();
            //await w1.EscapeAsync(m1);

            //await w2.WriteWoodToDBAsync();
            //await w2.EscapeAsync(m2);

            //await w3.WriteWoodToDBAsync();
            //await w3.EscapeAsync(m3);

            stopwatch.Stop();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
            Console.WriteLine("end");
        }
    }
}