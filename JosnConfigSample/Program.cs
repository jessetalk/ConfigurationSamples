using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.CommandLine;

namespace JosnConfigSample
{
    class Program
    {
        static void Main(string[] args)
        {
            


            JsonConfigurationSource source = new JsonConfigurationSource()
            {
                Path = "settings.json",
            };

            var builder = new ConfigurationBuilder();

            builder.Add(source);


            var configuration = builder.Build();
  
            Console.WriteLine($" ClassNo = { configuration["ClassNo"] }");
            Console.WriteLine($" ClassDesc = { configuration["ClassDesc"] }");

            Console.WriteLine("Students :");
            Console.Write(configuration["Students:0:name"]);
            Console.WriteLine($" age: { configuration["Students:0:age"]}");

            Console.Write(configuration["Students:1:name"]);
            Console.WriteLine($" age: { configuration["Students:1:age"]}");

            Console.Write(configuration["Students:2:name"]);
            Console.WriteLine($" age: { configuration["Students:2:age"]}");

            Console.ReadLine();


        }


        public void ChangeValue()
        {



        }


        
 
    }
}
