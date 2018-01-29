using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellationTokenSourceSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var cancellationToken = new CancellationTokenSource();
            cancellationToken.Token.Register(() => { Console.Write("end...."); });

            Task.Run(()=> Run(cancellationToken));

            Thread.Sleep(3000);
            cancellationToken.Cancel();


            Console.ReadLine();
        }

        private static void Run(CancellationTokenSource cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("print");
            }
        }

    }
}
