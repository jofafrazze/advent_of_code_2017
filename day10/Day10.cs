using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day10
{
    class Day10
    {
        static string ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            return reader.ReadLine();
        }

        static void DoKnotHashRound(ref int pos, ref int skip, ref List<int> lengths, ref List<int> list)
        {
            foreach (int l in lengths)
            {
                List<int> list2 = list.Concat(list).ToList();
                List<int> copy = list2.GetRange(pos, l);
                copy.Reverse();
                List<int> list3 = list2.GetRange(0, pos).Concat(copy).Concat(list2.GetRange(pos + l, (list.Count * 2) - l - pos)).ToList();
                int newStartLength = Math.Max(0, (pos + l) - list.Count);
                List<int> newListStart = list3.GetRange(list.Count, newStartLength).ToList();
                list = newListStart.Concat(list3.GetRange(newStartLength, list.Count - newStartLength)).ToList();
                pos += l + skip++;
                pos %= list.Count;
            }
        }

        static void PartA()
        {
            //List<int> lengths = new List<int>() { 3, 4, 1, 5 };
            //List<int> list = Enumerable.Range(0, 5).ToList();
            string input = ReadInput();
            List<int> lengths = input.Split(',').Select(int.Parse).ToList();
            List<int> list = Enumerable.Range(0, 256).ToList();
            int pos = 0;
            int skip = 0;
            DoKnotHashRound(ref pos, ref skip, ref lengths, ref list);
            Console.WriteLine("Part A: Result is {0}.", list[0] * list[1]);
        }

        static void PartB()
        {
            string input = ReadInput();
            List<int> lengths = Encoding.ASCII.GetBytes(input).Select(x => (int)x).ToList();
            lengths.AddRange(new List<int>() { 17, 31, 73, 47, 23 });
            List<int> list = Enumerable.Range(0, 256).ToList();
            int pos = 0;
            int skip = 0;
            for (int i = 0; i < 64; i++)
                DoKnotHashRound(ref pos, ref skip, ref lengths, ref list);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                int sum = 0;
                for (int n = 0; n < 16; n++)
                    sum ^= list[i * 16 + n];
                sb.Append(sum.ToString("x2"));
            }
            Console.WriteLine("Part B: Result is {0}.", sb.ToString());
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day10).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
