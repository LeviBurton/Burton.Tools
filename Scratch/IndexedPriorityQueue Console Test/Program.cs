using Burton.Lib.Alg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexedPriorityQueue_Console_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<float> Keys = new List<float>();
            List<int> Nodes = new List<int>();

        
            Keys.Add(50.0f);
            Keys.Add(5.0f);
            Keys.Add(35.0f);
            Keys.Add(25.0f);

            IndexedPriorityQueueLow<float> Queue = new IndexedPriorityQueueLow<float>(Keys, Keys.Count());

            Queue.Insert(0);
            Queue.Insert(1);
            Queue.Insert(2);
            Queue.Insert(3);

            var Foo = Queue.Pop();

        }
    }
}
