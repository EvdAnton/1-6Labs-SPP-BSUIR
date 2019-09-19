using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Task1
{
    public class TaskQueue
    {
        public int MaxThreads { get; set; }

        public int IdleTimeThreshold { get; set; } = 5;

        //основная очередь для обработки WorkItem в порядке FIFO
        //- содержит очередь задач в виде делегатов без параметров:

        //delegate void TaskDelegate(); ---> 3 требование (с параметром для интереса)
        private Queue<WorkItem> _workQueue;

        public int QueueLength
        {
            get
            {
                return _workQueue.Count();
            }
        }

        //список текущих потоков используемых для обрабоки WorkItem
        private List<WorkThread> _threadList;

        //выполняет функция управления в пуле потоков(отключение свободных потоков)
        private Thread _managementThread;
        private bool _keepManagementThreadRunning = true;


        public TaskQueue(int count)
        {
            //- создает указанное количество потоков пула в конструкторе; ---> 2 требование
            MaxThreads = count;
            _managementThread = new Thread(new ThreadStart(ManagementWorker));
            _managementThread.Start();

            _workQueue = new Queue<WorkItem>();
            _threadList = new List<WorkThread>();
        }

        ~TaskQueue()
        {
            this.WaitAll();
        }


        //- обеспечивает постановку в очередь и последующее выполнение делегатов с помощью метода

        //void EnqueueTask(TaskDelegate task); ---> 4 тебование
        public void EnqueueTask(object workObject, TaskDelegate Delegate)
        {
            WorkItem wi = new WorkItem();

            wi.workObject = workObject;
            wi.Delegate = Delegate;
            lock (_workQueue)
            {
                _workQueue.Enqueue(wi);
            }

            //проверяет есть ли свободные потоки
            bool FoundIdleThread = false;
            foreach (WorkThread wt in _threadList)
            {
                if (!wt.Busy)
                {
                    wt.WakeUp();
                    FoundIdleThread = true;
                    break;
                }
            }

            if (!FoundIdleThread)
            {
                //проверяет можно ли создать новый поток для обработки дополнительной нагрузки
                if (_threadList.Count < MaxThreads)
                {
                    WorkThread wt = new WorkThread(ref _workQueue);
                    lock (_threadList)
                    {
                        _threadList.Add(wt);
                    }
                }
            }
        }

        //5 task WaitAll
        public void WaitAll()
        {
            //останавливает управляющий поток
            _keepManagementThreadRunning = false;
            if (_managementThread != null)
            {
                if (_managementThread.ThreadState == ThreadState.WaitSleepJoin)
                {
                    _managementThread.Interrupt();
                }
                _managementThread.Join();
            }
            _managementThread = null;

            foreach (WorkThread t in _threadList)
            {
                t.ShutDown();
            }
            _threadList.Clear();

            _workQueue.Clear();
        }

        private void ManagementWorker()
        {
            while (_keepManagementThreadRunning)
            {
                try
                {
                    //проверяет можем ли мы освободить поток
                    if (_threadList.Count > 0)
                    {
                        foreach (WorkThread wt in _threadList)
                        {
                            if (DateTime.Now.Subtract(wt.LastOperation).Seconds > IdleTimeThreshold)
                            {
                                wt.ShutDown();
                                lock (_threadList)
                                {
                                   // _threadList.Remove(wt);
                                    break;
                                }
                            }
                        }
                    }
                }
                catch { }

                try
                {
                    Thread.Sleep(1000);
                }
                catch { }
            }
        }
    }
}
