using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Primitives;

namespace FileProviderSample
{
    class Program
    {
       private static IFileProvider fileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

        static void Main(string[] args)
        {
            

            
            ReadConfig(fileProvider);

            ChangeToken.OnChange(
                    () =>  fileProvider.Watch("settings.json"),
                    () => {
                        Console.WriteLine("File Changed============================");
                        ReadConfig(fileProvider);
                    }
                );

            //foreach (var file in fileProvider.GetDirectoryContents(""))
            //{
            //    if (file.IsDirectory)
            //    {
            //        Console.WriteLine($" - { file.Name}");
            //    }
            //    else
            //    {
            //        Console.WriteLine($" { file.Name} - {file.Length} bytes");
            //    }
            //}

            //Console.ReadLine();

            Console.ReadLine();
        }

        private static void ReadConfig(IFileProvider fileProvider)
        {
            var file = fileProvider.GetFileInfo("settings.json");
            if (file != null)
            {
                var stream = file.CreateReadStream();

                var reader = new JsonTextReader(new StreamReader(stream));
                var jsonConfig = JObject.Load(reader);


                foreach (var x in jsonConfig)
                {
                    Console.WriteLine($"name : {x.Key}, value: {x.Value.ToString()}");
                }

                
            }

        }
    }
}
