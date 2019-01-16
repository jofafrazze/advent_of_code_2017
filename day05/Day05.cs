using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day05
{
    class Day05
    {
        static List<int> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<int> list = new List<int>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                list.Add(int.Parse(line));
            }
            return list;
        }

        static void PartA()
        {
            List<int> input = ReadInput();
            int i = 0;
            int n = 0;
            while (i >= 0 && i < input.Count)
            {
                int add = input[i]++;
                i += add;
                n++;
            }
            Console.WriteLine("Part A: Result is {0}.", n);
        }

        static void PartB()
        {
            List<int> input = ReadInput();
            int i = 0;
            int n = 0;
            while (i >= 0 && i < input.Count)
            {
                int add = input[i];
                input[i] += (add >= 3) ? -1 : 1;
                i += add;
                n++;
            }
            Console.WriteLine("Part B: Result is {0}.", n);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day05).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
