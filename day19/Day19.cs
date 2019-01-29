using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Position = AdventOfCode.GenericPosition2D<int>;

namespace day19
{
    class Day19
    {
        static List<string> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<string> list = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
                list.Add(line);
            return list;
        }

        static readonly Position goUp = new Position(0, -1);
        static readonly Position goLeft = new Position(-1, 0);
        static readonly Position goRight = new Position(1, 0);
        static readonly Position goDown = new Position(0, 1);
        static readonly List<Position> directions = new List<Position>()
        {
            goRight, goUp, goLeft, goDown,
        };

        static void PartAB()
        {
            List<string> input = ReadInput();
            Position pos = new Position(input[0].IndexOf('|'), 0);
            int dirIndex = 3;
            StringBuilder sb = new StringBuilder();
            int steps = -1;
            bool done = false;
            while (!done)
            {
                char c = input[pos.y][pos.x];
                if (c == '+')
                {
                    int nextDir = (dirIndex + 1) % 4;
                    Position nextPos = pos + directions[nextDir];
                    if (input[nextPos.y][nextPos.x] == ' ')
                        nextDir = (dirIndex + 4 - 1) % 4;
                    dirIndex = nextDir;
                }
                else if (c == ' ')
                {
                    done = true;
                }
                else
                {
                    if ((c != '|') && (c != '-'))
                        sb.Append(c);
                }
                pos += directions[dirIndex];
                steps++;
            }
            Console.WriteLine("Part A: Result is {0}.", sb.ToString());
            Console.WriteLine("Part B: Result is {0}.", steps);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day19).Namespace + ":");
            PartAB();
        }
    }
}
