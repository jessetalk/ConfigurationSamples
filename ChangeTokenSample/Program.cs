using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace ChangeTokenSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new Config();

            ChangeToken.OnChange(

                () => config.GetReloadToken(),
                () => {
                    Console.WriteLine(config.Value);
                });



            Task.Run(() =>
            {

                var i = 1;
                while (i < 5)
                {
                    config.Value = i.ToString();
                    i++;

                }
            });

            Console.Read();

        }
    }
}
