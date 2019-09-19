using System;
using System.Collections.Generic;
using System.Threading;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            IncThread mt1 = new IncThread("Inc thread", 5);

            // разрешить инкременирующему потоку начаться
            Thread.Sleep(100);

            DecThread mt2 = new DecThread("Dec thread", 5);

            mt1.Thrd.Join();
            mt2.Thrd.Join();

            Console.ReadLine();
        }
    }

    class Mutex
    {
        const int INITIAL_VALUE = 0;
        const int MAX_VALUE = 1;

        private static int _count = 0;
        private int threadId = 0;
        //занял

        public void Lock()
        {
            while (Interlocked.CompareExchange(ref _count, MAX_VALUE, INITIAL_VALUE) != 0)
            {
                
            }
            threadId = Thread.CurrentThread.ManagedThreadId;

        }

        //вышел
        public void Unlock()
        {
            if (Thread.CurrentThread.ManagedThreadId == threadId)
                Interlocked.Exchange(ref _count, INITIAL_VALUE);

        }
    }

    class SharedRes
    {
        public static int Count;
        public static Mutex mtx = new Mutex();
    }

    // В этом классе Count инкрементируется
    class IncThread
    {
        int num;
        public Thread Thrd;

        public IncThread(string name, int n)
        {
            Thrd = new Thread(this.Run);
            num = n;
            Thrd.Name = name;
            Thrd.Start();
        }

        // Точка входа в поток
        void Run()
        {
            Console.WriteLine(Thrd.Name + " ожидает мьютекс");


            // Получить мьютекс
            SharedRes.mtx.Lock();

            Console.WriteLine(Thrd.Name + " получает мьютекс");

            do
            {
                Thread.Sleep(500);
                SharedRes.Count++;
                Console.WriteLine("в потоке {0}, Count={1}", Thrd.Name, SharedRes.Count);
                num--;
            } while (num > 0);

            Console.WriteLine(Thrd.Name + " освобождает мьютекс");
            SharedRes.mtx.Unlock();

        }
    }

    class DecThread
    {
        int num;
        public Thread Thrd;

        public DecThread(string name, int n)
        {
            Thrd = new Thread(new ThreadStart(this.Run));
            num = n;
            Thrd.Name = name;
            Thrd.Start();
        }

        // Точка входа в поток
        void Run()
        {
            Console.WriteLine(Thrd.Name + " ожидает мьютекс");

            // Получить мьютекс
            SharedRes.mtx.Lock();

            Console.WriteLine(Thrd.Name + " получает мьютекс");

            do
            {
                Thread.Sleep(500);
                SharedRes.Count--;
                Console.WriteLine("в потоке {0}, Count={1}", Thrd.Name, SharedRes.Count);
                num--;
            } while (num > 0);

            Console.WriteLine(Thrd.Name + " освобождает мьютекс");

            SharedRes.mtx.Unlock();
        }
    }
}

