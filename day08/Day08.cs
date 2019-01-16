using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day08
{
    class Day08
    {
        static List<string[]> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<string[]> list = new List<string[]>();
            string line;
            while ((line = reader.ReadLine()) != null)
                list.Add(line.Split(' ').ToArray());
            return list;
        }

        static readonly Dictionary<string, Func<int, int, bool>> comparisons = new Dictionary<string, Func<int, int, bool>>()
        {
            { ">", delegate(int a, int b) { return a > b; } },
            { "<", delegate(int a, int b) { return a < b; } },
            { ">=", delegate(int a, int b) { return a >= b; } },
            { "<=", delegate(int a, int b) { return a <= b; } },
            { "==", delegate(int a, int b) { return a == b; } },
            { "!=", delegate(int a, int b) { return a != b; } },
        };

        static void PartAB()
        {
            List<string[]> input = ReadInput();
            Dictionary<string, int> registers = new Dictionary<string, int>();
            int regMax = int.MinValue;
            foreach (string[] s in input)
            {
                if (!registers.ContainsKey(s[0])) registers[s[0]] = 0;
                if (!registers.ContainsKey(s[4])) registers[s[4]] = 0;
                if (comparisons[s[5]](registers[s[4]], int.Parse(s[6])))
                {
                    registers[s[0]] += int.Parse(s[2]) * (s[1] == "inc" ? 1 : -1);
                    regMax = Math.Max(regMax, registers[s[0]]);
                }
            }
            Console.WriteLine("Part A: Result is {0}.", registers.Values.Max());
            Console.WriteLine("Part B: Result is {0}.", regMax);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day08).Namespace + ":");
            PartAB();
        }
    }
}
