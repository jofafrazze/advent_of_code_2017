using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day16
{
    class Day16
    {
        struct DanceMove
        {
            public char move;
            public char c1;
            public char c2;
            public int i1;
            public int i2;
        }
        static List<DanceMove> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<DanceMove> list = new List<DanceMove>();
            string line = reader.ReadLine();
            string[] v = line.Split(',').ToArray();
            foreach(string s in v)
            {
                DanceMove dm = new DanceMove();
                dm.move = s[0];
                if (dm.move == 'p' || dm.move == 'x')
                {
                    int i = s.IndexOf('/', 2);
                    string s1 = s.Substring(1, i - 1);
                    string s2 = s.Substring(i + 1);
                    if (dm.move == 'p')
                    {
                        dm.c1 = s1[0];
                        dm.c2 = s2[0];
                    }
                    else
                    {
                        dm.i1 = int.Parse(s1);
                        dm.i2 = int.Parse(s2);
                    }
                }
                else
                {
                    dm.i1 = int.Parse(s.Substring(1));
                }
                list.Add(dm);
            }
            return list;
        }

        static void Swap<T>(ref List<T> b, int i1, int i2)
        {
            T temp = b[i1];
            b[i1] = b[i2];
            b[i2] = temp;
        }

        static List<char> PerformDance(List<char> p, List<DanceMove> danceMoves)
        {
            foreach (DanceMove dm in danceMoves)
            {
                if (dm.move == 's')
                    p = p.Skip(16 - dm.i1).Take(dm.i1).Concat(p.Take(16 - dm.i1)).ToList();
                else if (dm.move == 'x')
                    Swap(ref p, dm.i1, dm.i2);
                else
                    Swap(ref p, p.IndexOf(dm.c1), p.IndexOf(dm.c2));
            }
            return p;
        }

        static void PartA()
        {
            List<DanceMove> danceMoves = ReadInput();
            List<char> p = Enumerable.Range(0, 16).Select(x => (char)('a' + x)).ToList();
            p = PerformDance(p, danceMoves);
            Console.WriteLine("Part A: Result is {0}.", String.Join("", p));
        }

        static void PartB()
        {
            List<DanceMove> danceMoves = ReadInput();
            List<char> program = Enumerable.Range(0, 16).Select(x => (char)('a' + x)).ToList();
            List<char> p = new List<char>(program);
            string s = String.Join("", p);
            HashSet<string> visited = new HashSet<string>() { s };
            int uniqueDances = 1;
            while (!visited.Contains((s = String.Join("", (p = PerformDance(p, danceMoves))))))
            {
                //Console.WriteLine("Dance '{0}' seen before? {1}", s, visited.Contains(s) ? "YES!" : "nope");
                visited.Add(s);
                uniqueDances++;
            }
            int n = 1000000000 % uniqueDances;
            for (int i = 0; i < n; i++)
                program = PerformDance(program, danceMoves);
            Console.WriteLine("Part B: Result is {0}.", String.Join("", program));
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day16).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
