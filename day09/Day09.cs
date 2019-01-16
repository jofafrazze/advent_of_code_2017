using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day09
{
    class Day09
    {
        static string ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            return reader.ReadLine();
        }

        static string FilterGarbage(string input, ref int filtered)
        {
            StringBuilder sb = new StringBuilder();
            int gBegin = 0;
            int gEndLast = gBegin;
            while ((gBegin = input.IndexOf('<', gEndLast)) >= 0)
            {
                int gEnd = gBegin + 1;
                bool done = false;
                while ((gEnd < input.Length) && !done)
                {
                    char c = input[gEnd];
                    if (c == '>')
                        done = true;
                    else if (c == '!')
                        gEnd++;
                    else
                        filtered++;
                    gEnd++;
                }
                sb.Append(input.Substring(gEndLast, gBegin - gEndLast));
                gEndLast = gEnd;
            }
            sb.Append(input.Substring(gEndLast));
            return sb.ToString();
        }

        static int CalculateScore(string input)
        {
            int sum = 0;
            int depth = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '{')
                    depth++;
                else if (input[i] == '}')
                    sum += depth--;
            }
            return sum;
        }

        static void PartAB()
        {
            string input = ReadInput();
            int filtered = 0;
            string i2 = FilterGarbage(input, ref filtered);
            int score = CalculateScore(i2);
            Console.WriteLine("Part A: Result is {0}.", score);
            Console.WriteLine("Part B: Result is {0}.", filtered);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day09).Namespace + ":");
            PartAB();
        }
    }
}
