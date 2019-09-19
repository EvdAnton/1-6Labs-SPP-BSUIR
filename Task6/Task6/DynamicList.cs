using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task6
{
    public class DynamicList<T> : IEnumerable<T>
    {
        T[] data;
        private readonly int _numberOfElements; 
        public DynamicList(int numberOdElements)
        {
            _numberOfElements = numberOdElements;
            data = new T[numberOdElements];
        }

        public T this[int index]
        {
            get
            {
                return data[index];
            }
            set
            {
                data[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return data.Length;
            }
        }
        
        public void Add(T element)
        {
            var b = data.ToList();
            b.Add(element);
            data = b.ToArray();
        }

        public void Remove(T element)
        {
            var b = data.ToList();
            try
            {
                b.Remove(element);
            }
            catch
            {
                Console.WriteLine("Данного элемента в массиве нет ---> Remove");
            }
            data = b.ToArray();
            
        }

        public void RemoveAt(int index)
        {
            var b = data.ToList();
            try
            {
                b.RemoveAt(index);
            }
            catch
            {
                Console.WriteLine("Данного индекса в массиве нет ---> RemoveAt");
            }
            data = b.ToArray();
        }

        public void Clear()
        {
            var d = data.ToList();
            d.Clear();
            data = d.ToArray();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DynamicListEnumerator<T>(data);
        }

        private IEnumerator GetEnumerator1()
        {
            return this.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator1();
        }
    }

    public class DynamicListEnumerator<T> : IEnumerator<T>
    {
        private int _index = 0;
        private readonly int _count = 0;

        private T[] _data;
        public DynamicListEnumerator(T[] data)
        {
            _data = data;
            _count = data.Length;
        }

        private T _current;

        public T Current
        {
            get
            {
                if (_data == null || _current == null )
                    throw new InvalidOperationException();
                else
                    return _current;
            }
        }

        private object Current1
        {

            get { return this.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current1; }
        }

        public void Dispose()
        {
            _current = default(T);
            _data = null;
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            if (_index >= _count)
                return false;
            _current = _data[_index++];
            return true;
        }

        public void Reset()
        {
            _index = 0;
            _current = default(T);
        }
    }
}