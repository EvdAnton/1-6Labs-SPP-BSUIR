using System;
using System.Collections.Generic;
using System.Text;

namespace Task1
{
    public delegate void TaskDelegate(object workObject);

    class WorkItem
    {
        public object workObject;
        public TaskDelegate Delegate;
    }
}
