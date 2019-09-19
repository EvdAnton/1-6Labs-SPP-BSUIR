using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Task1
{
    class WorkThread
    {
        private Thread w_workProcess = null;

        private bool w_keepRunning = true;
        public bool Busy { get; private set; } = false;
        public DateTime LastOperation { get; private set; } = DateTime.Now;

        private Queue<WorkItem> w_workQueue;

        public WorkThread(ref Queue<WorkItem> WorkQueue)
        {
            w_workQueue = WorkQueue;
            w_workProcess = new Thread(new ThreadStart(Worker));
            w_workProcess.Start();
        }

        ~WorkThread()
        {
            if (w_workProcess != null)
            {
                Busy = false;
                w_keepRunning = false;
                if (w_workProcess.ThreadState == ThreadState.WaitSleepJoin)
                {
                    w_workProcess.Interrupt();
                }
                w_workProcess.Join();
            }
        }

        public void WakeUp()
        {
            if (w_workProcess.ThreadState == ThreadState.WaitSleepJoin)
            {
                w_workProcess.Interrupt();
            }
            Busy = true;
        }

        //5 task WaitAll
        public void ShutDown()
        {
            w_keepRunning = false;
            if (w_workProcess.ThreadState == ThreadState.WaitSleepJoin)
            {
                //прерывает работу потока находящегося в состоянии ожидания
                w_workProcess.Interrupt();
            }
            w_workProcess.Join();
            w_workProcess = null;
        }

        private void Worker()
        {
            WorkItem wi;

            while (w_keepRunning)
            {
                try
                {
                    while (w_workQueue.Count > 0)
                    {
                        wi = null;

                        lock (w_workQueue)
                        {
                            wi = w_workQueue.Dequeue();
                        }

                        if (wi != null)
                        {
                            LastOperation = DateTime.Now;
                            Busy = true;
                            wi.Delegate.Invoke(wi.workObject);
                        }
                    }

                }
                catch { }

                try
                {
                    Busy = false;
                    Thread.Sleep(1000);
                }
                catch { }
            }
        }
    }
}
