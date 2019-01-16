using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day06
{
    class Day06
    {
        static List<int> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            string line = reader.ReadLine();
            return line.Split('\t').Select(int.Parse).ToList();
        }

        static void PartAB()
        {
            //List<int> input = new List<int>() { 0, 2, 7, 0 };
            List<int> input = ReadInput();
            HashSet<string> visited = new HashSet<string>();
            List<int> banks = input;
            string h = String.Join(",", banks);
            string hB = "";
            int n = 0;
            int partA = 0;
            int partB = 0;
            do
            {
                visited.Add(h);
                int max = banks.Max();
                int i = banks.IndexOf(max);
                banks[i] = 0;
                for (int a = 0; a < max; a++)
                    banks[(i + a + 1) % banks.Count]++;
                n++;
                h = String.Join(",", banks);
                if (visited.Contains(h) && (partA == 0))
                {
                    partA = n;
                    hB = h;
                    h = "";
                }
            }
            while (h != hB);
            Console.WriteLine("Part A: Result is {0}.", partA);
            Console.WriteLine("Part B: Result is {0}.", n - partA);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day06).Namespace + ":");
            PartAB();
        }
    }
}
