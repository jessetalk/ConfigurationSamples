using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace ChangeNotification
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var config = new Config();
            var token = config.Watch();

            Func<CancellationTokenSource> changeTokenProducer = () => { return config.Watch(); };
            Action changeTokenConsumer = () => { Console.WriteLine(config.Value); };

            Action callback = null;
            callback = () => {
                var t = changeTokenProducer();
                try
                {
                    changeTokenConsumer();
                }
                finally
                {
                    t.Token.Register(callback);
                }
            };


            token.Token.Register(callback);


            Task.Run(()=>
            {

                var i = 1;
                while (i < 100)
                {
                    config.Value = i.ToString();
                    i++;

                }
            });

            Console.Read();
        }
    }
}
