using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;



namespace CommandLineConfig
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new Dictionary<string, string>
            {
                { "name","jesse" },
                { "age", "18"}
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .AddCommandLine(args);

            

            var configuration = builder.Build();

            Console.WriteLine($"Name: {configuration["name"]}");
            Console.WriteLine($"Age: {configuration["age"]}");


            Console.ReadLine();


        }
    }
}
