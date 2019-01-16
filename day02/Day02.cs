using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day02
{
    class Day02
    {
        static List<List<int>> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<List<int>> list = new List<List<int>>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                List<int> d = line.Split('\t').Select(int.Parse).ToList();
                list.Add(d);
            }
            return list;
        }

        static void PartA()
        {
            List<List<int>> lines = ReadInput();
            int sum = 0;
            foreach (List<int> line in lines)
            {
                sum += line.Max() - line.Min();
            }
            Console.WriteLine("Part A: Result is {0}.", sum);
        }

        static void PartB()
        {
            List<List<int>> lines = ReadInput();
            int FindLineValue(List<int> line)
            {
                for (int i = 0; i < line.Count; i++)
                {
                    for (int j = 0; j < line.Count; j++)
                    {
                        if ((i != j) && (line[i] % line[j] == 0))
                        {
                            return line[i] / line[j];
                        }
                    }
                }
                throw new ArgumentOutOfRangeException();
            }
            int sum = 0;
            foreach (List<int> line in lines)
            {
                sum += FindLineValue(line);
            }
            Console.WriteLine("Part B: Result is {0}.", sum);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day02).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
