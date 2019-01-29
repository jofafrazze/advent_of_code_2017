using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day13
{
    class Scanner
    {
        public int depth;
        public int range;
        public int pos;
        public int dir;
        public Scanner(int d, int r)
        {
            depth = d;
            range = r;
            pos = 0;
            dir = 1;
        }
        public Scanner(Scanner s)
        {
            depth = s.depth;
            range = s.range;
            pos = s.pos;
            dir = s.dir;
        }
    }
    class Day13
    {
        static Dictionary<int, Scanner> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            Dictionary<int, Scanner> dict = new Dictionary<int, Scanner>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                int[] d = line.Split(": ").Select(int.Parse).ToArray();
                Scanner s = new Scanner(d[0], d[1]);
                dict[s.depth] = s;
            }
            return dict;
        }

        static void StepScanners(Dictionary<int, Scanner> scanners)
        {
            foreach (Scanner s in scanners.Values)
            {
                s.pos += s.dir;
                if (s.pos < 0)
                {
                    s.dir = 1;
                    s.pos = 1;
                }
                else if (s.pos >= s.range)
                {
                    s.dir = -1;
                    s.pos = s.range - 2;
                }
            }
        }

        static int CalculateTotalSeverity(Dictionary<int, Scanner> scanners, ref int timesCaught, bool bailIfCaught)
        {
            int dn = scanners.Keys.Min();
            int dx = scanners.Keys.Max();
            int sum = 0;
            timesCaught = 0;
            for (int i = dn; (i <= dx) && (!bailIfCaught || (timesCaught == 0)); i++)
            {
                if (scanners.ContainsKey(i) && scanners[i].pos == 0)
                {
                    sum += scanners[i].depth * scanners[i].range;
                    timesCaught++;
                }
                StepScanners(scanners);
            }
            return sum;
        }

        static void PartA()
        {
            Dictionary<int, Scanner> scanners = ReadInput();
            int timesCaught = 0;
            int sum = CalculateTotalSeverity(scanners, ref timesCaught, false);
            Console.WriteLine("Part A: Result is {0}.", sum);
        }

        static Dictionary<int, Scanner> CopyScanners(Dictionary<int, Scanner> scanners)
        {
            Dictionary<int, Scanner> copy = new Dictionary<int, Scanner>();
            foreach (var kvp in scanners)
                copy[kvp.Key] = new Scanner(kvp.Value);
            return copy;
        }

        static void PartB()
        {
            Dictionary<int, Scanner> scanners = ReadInput();
            int delay = -1;
            int timesCaught = 1;
            while (timesCaught > 0)
            {
                delay++;
                Dictionary<int, Scanner> scannersCopy = CopyScanners(scanners);
                CalculateTotalSeverity(scannersCopy, ref timesCaught, true);
                StepScanners(scanners);
                if (delay % 10000 == 0)
                    Console.Write(".");
            }
            Console.WriteLine();
            Console.WriteLine("Part B: Result is {0}.", delay);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day13).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
