using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    class Program
    {
        // https://www.techdays.ru/get.aspx?MID=5e2797e0-7f91-4ecb-95e8-5a4b5491ee57&type=3

        static void Main(string[] args)
        {
            #region 1 Simple tasks

            var task1 = new Task(printSomething); // preferred way to run
            var task2 = new Task(delegate
            {
                printSomething();
            });
            var task3 = new Task(() => printSomething());

            task1.Start();
            task2.Start();
            task3.Start(); 
            
            #endregion

            #region 2 Tasks with parameters
            /*
            var task1 = new Task(new Action<object>(printSomething), 1);
            var task2 = new Task(printSomething, 2); // main!
            var task3 = new Task(x => printSomething(x), 3);

            task1.Start();
            task2.Start();
            task3.Start(); 
            */
            #endregion

            #region 3 Return result from task
            /*
            Stopwatch timer = new Stopwatch();
            //var task = Task.Factory.StartNew()
            Task<int> task1 = new Task<int>(() =>
            {
                int sum = 0;
                for (int i = 1; i <= 1000000; i++)
                {
                    sum += i;
                }
                return sum;
            });

            timer.Restart();
            task1.Start();
            Console.WriteLine("Result 1: " + task1.Result);
            timer.Stop();
            Console.WriteLine("Time, ms: " + timer.ElapsedMilliseconds + ", ticks: " + timer.ElapsedTicks);
            Console.WriteLine();


            Task<int> task2 = new Task<int>((c) =>
            {
                int sum = 0;
                for (int i = 1; i <= (int)c; i++)
                {
                    sum += i;
                }
                return sum;
            }, 1000000);

            timer.Restart();
            task2.Start();
            Console.WriteLine("Result 2: " + task2.Result);
            timer.Stop();
            Console.WriteLine("Time, ms: " + timer.ElapsedMilliseconds + ", ticks: " + timer.ElapsedTicks);
            Console.WriteLine();
            */
            #endregion

            #region 4 Cancelling
            /*
            var tokenSrc = new CancellationTokenSource();
            var token = tokenSrc.Token;

            Task task = new Task(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    //token.ThrowIfCancellationRequested();
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("From task: Operation is canseled");
                        throw new OperationCanceledException(token);
                    }
                    else
                    {
                        Thread.Sleep(10);
                        Console.Write(".");
                    }
                }
            }, token);
            
            // Необовязково. Буде викликаний після скасування.
            token.Register(() => {
                Console.WriteLine("Cancel delegate called");
            });

            // task2 запуститься після скасування першої
            Task task2 = new Task(() => {
                token.WaitHandle.WaitOne();
                Console.WriteLine("task2 started");
            });
            task2.Start();

            Console.WriteLine("Press enter to start task, enter again to stop");
            Console.ReadLine();
            task.Start();
            Console.ReadLine();
            Console.WriteLine("Cancelling");
            tokenSrc.Cancel();
            */

            #endregion

            #region 5
            /*
            var cts1 = new CancellationTokenSource();
            var cts2 = new CancellationTokenSource();
            var cts3 = new CancellationTokenSource();

            var cts = CancellationTokenSource.CreateLinkedTokenSource(cts1.Token, cts2.Token, cts3.Token);
            var task = new Task(() => {
                Console.WriteLine("task waiting");
                //cts.Token.WaitHandle.WaitOne();
                cts.Token.WaitHandle.WaitOne(5000); // wait for 5 seconds
                Console.WriteLine("task cancelled");
                throw new OperationCanceledException(cts.Token);
            });
            task.Start();
            //task.Wait(2000); //task.Wait(cts.Token);
            Console.WriteLine("press enter to cancel");
            Console.ReadLine();
            cts3.Cancel();
            */

            #endregion
                        
            Console.WriteLine("ALL DONE"); ;
            Console.ReadKey();
        }

        private static void printSomething()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Task comleted");
        }

        private static void printSomething(object x)
        {
            Console.WriteLine("Task comleted, " + x);
        }
    }
}
