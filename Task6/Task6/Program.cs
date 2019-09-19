using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task6
{
    class Program
    {
        static void Main(string[] args)
        {
            DynamicList<int> data = new DynamicList<int>(5);
            data.Add(6);
            data[0] = 1;
            data.Remove(6);
            data[2] = 5;
            data.RemoveAt(8);
            foreach(var i in data)
                Console.Write(i.ToString() + " ");
            Console.ReadLine();
        }
    }
}
