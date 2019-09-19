using System;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskQueue tp = new TaskQueue(AmountOdThreads());

            for (int i = 0; i < 20; i++)
            {
                tp.EnqueueTask(i, new TaskDelegate(PerformWork));
            }


            Console.ReadLine();


            tp.WaitAll();
        }

        static private void PerformWork(object o)
        {
            int i = (int)o;

            Console.WriteLine("Work Performed: " + i.ToString());
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("End Work Performed: " + i.ToString());
        }


        private static int AmountOdThreads()
        {
            int amount = 0;
            bool isCorrect = false;

            while (!isCorrect)
            {
                Console.Write("Enter max number of threads: ");
                amount = Convert.ToInt32(Console.ReadLine());
                if (amount <= 0)
                    isCorrect = false;
                else
                    isCorrect = true;
            }
            return amount;
        }
    }
}
