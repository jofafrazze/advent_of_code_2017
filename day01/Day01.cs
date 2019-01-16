using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day01
{
    class Day01
    {
        static string ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            return reader.ReadLine();
        }

        static int GetSum(string s, int offs)
        {
            int sum = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char c1 = s[i];
                char c2 = s[(i + offs) % s.Length];
                if (c1 == c2)
                {
                    int a = c1 - '0';
                    sum += a;
                }
            }
            return sum;
        }

        static void PartA()
        {
            string s = ReadInput();
            int sum = GetSum(s, 1);
            Console.WriteLine("Part A: Result is {0}.", sum);
        }

        static void PartB()
        {
            string s = ReadInput();
            int sum = GetSum(s, s.Length / 2);
            Console.WriteLine("Part B: Result is {0}.", sum);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day01).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
