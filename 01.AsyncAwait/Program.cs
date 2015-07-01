using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest_AwaitInCycle
{
    class Program
    {
        private static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Starting sync caclulation:");
            Test_1();
            Console.WriteLine("Calculation started. Press Enter to continue");
            Console.WriteLine();
            Console.ReadLine();

            Console.WriteLine("Starting async calculations via task awaiter:");
            Test_2();
            Console.WriteLine("Calculation started. Press Enter to continue");
            Console.WriteLine();
            Console.ReadLine();

            Console.WriteLine("Starting async calculations with async/await:");
            Test_3();
            Console.WriteLine("Calculation started. Press Enter");
            Console.WriteLine();
            Console.ReadLine();

            Console.WriteLine("Starting 10 async calculations with async/await in cycle:");
            Test();
            Console.WriteLine("Calculation cycle started. Press Enter to abort and exit");
            Console.WriteLine();
            Console.ReadLine();
        }

        // Sync operation
        static void Test_1()
        {
            int result = ComplexCalculation();
            Console.WriteLine("Test_1(): " + result);
        }

        // Async operation with Task
        static void Test_2()
        {
            Task<int> task = ComplexCalculationAsync();
            var awaiter = task.GetAwaiter();

            awaiter.OnCompleted(() =>
            {
                int result = awaiter.GetResult();
                Console.WriteLine("Test_2(): " + result);
            });
        }

        // Async / await operations
        async static void Test_3()
        {
            int result = await ComplexCalculationAsync();
            Console.WriteLine("Test_3(): " + result);
        }


        async static void Test()
        {
            for (int i = 0; i < 10; i++)
            {
                int result = await ComplexCalculationAsync();
                Console.WriteLine("Test(): " + result + ", i = " + i + ", thread id = " + Thread.CurrentThread.ManagedThreadId);
            }
        }


        private static Task<int> ComplexCalculationAsync()
        {
            return Task.Run(() => ComplexCalculation());
        }

        private static int ComplexCalculation()
        {
            //double x = 2;
            //for (int i = 1; i < 100000000; i++)
            //{
            //    x += Math.Sqrt(x) / i;
            //}
            //return (int)x;
            Thread.Sleep(rnd.Next(2000, 3000));
            return 42;
        }
    }
}
