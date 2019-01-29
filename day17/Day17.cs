using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day17
{
    static class CircularLinkedList
    {
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
        {
            return current.Next ?? current.List.First;
        }

        public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
        {
            return current.Previous ?? current.List.Last;
        }
    }

    class Day17
    {
        static void PartA()
        {
            const int steps = 312;
            LinkedList<int> circularBuffer = new LinkedList<int>(new int[] { 0 });
            LinkedListNode<int> pos = circularBuffer.First;
            for (int i = 1; i <= 2017; i++)
            {
                for (int n = 0; n < steps; n++)
                    pos = pos.NextOrFirst();
                circularBuffer.AddAfter(pos, i);
                pos = pos.NextOrFirst();
            }
            Console.WriteLine("Part A: Result is {0}.", pos.NextOrFirst().Value);
        }

        static void PartB()
        {
            const int steps = 312;
            LinkedList<int> circularBuffer = new LinkedList<int>(new int[] { 0 });
            LinkedListNode<int> pos = circularBuffer.First;
            int iMax = 50000000;
            var watch = Stopwatch.StartNew();
            for (int i = 1; i <= iMax; i++)
            {
                for (int n = 0; n < steps; n++)
                    pos = pos.NextOrFirst();
                circularBuffer.AddAfter(pos, i);
                pos = pos.NextOrFirst();
                if (i % (iMax / 100) == 0)
                    Console.Write(".");
            }
            watch.Stop();
            Console.WriteLine();
            Console.WriteLine("Calculation took {0} s.", watch.Elapsed.TotalSeconds);
            LinkedListNode<int> finalPos = circularBuffer.Find(0);
            Console.WriteLine("Part B: Result is {0}.", finalPos.NextOrFirst().Value);
        }

        static void PartB2()
        {
            const int steps = 312;
            int iMax = 50000000;
            int index = 0;
            int after0 = -1;
            for (int i = 1; i <= iMax; i++)
            {
                index = (index + steps) % i;
                if (index == 0)
                    after0 = i;
                index++;
            }
            Console.WriteLine("Part B: Result is {0}.", after0);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day17).Namespace + ":");
            PartA();
            PartB2();
        }
    }
}
