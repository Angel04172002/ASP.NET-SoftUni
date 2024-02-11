using System.Runtime.InteropServices;

namespace Multithreading
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //Tasks

            /*

            Console.WriteLine("Start");

            Task.Run(() => PrintNumbersInRange(0, 100));
            Task.Run(() => PrintNumbersInRange(100, 200));

            PrintNumbersInRange(200, 300);

            Task.WaitAll();


            Console.WriteLine("Done");

            */







            //Asynchronous code


            /*

            List<long> numbers = new List<long>();
            Thread t = new Thread(() =>
            SumOddNumbers(numbers, 10, 100000000L));
            t.Start();
            Console.WriteLine("What should I do?");
            while (true)
            {
                string command = Console.ReadLine();
                if (command == "exit") break;
            }


            Console.WriteLine(String.Join(", ", numbers));

            */




            //Race condition

            List<int> numbers = Enumerable.Range(0, 10000).ToList();


            Thread t1 = new Thread(() =>
            {
                lock (numbers)
                {
                    for (int i = 0; i <= 10; i++)
                    {
                        int removed = numbers[i];
                        numbers.RemoveAt(i);
                        Console.WriteLine(removed);
                    }
                }
            });


            Thread t2 = new Thread(() =>
            {
                lock (numbers)
                {

                    for (int i = 5; i <= 15; i++)
                    {
                        int removed = numbers[i];
                        numbers.RemoveAt(i);
                        Console.WriteLine(removed);
                    }
                }
            });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("This is a total mess");

        }

        static void SumOddNumbers(List<long> numbers, long start, long end)
        {
            for (long i = start;  i <= end; i++)
            {
                if(i % 2 != 0)
                {
                    numbers.Add(i);
                }   
            }
        }



        static void PrintNumbersInRange(int min, int max)
        {
            for (int i = min; i <= max; i++)
            {
                Console.WriteLine(i);
            }
        }
    }
}
