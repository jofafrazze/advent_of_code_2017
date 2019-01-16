using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Position = AdventOfCode.GenericPosition2D<int>;


namespace day11
{
    class Day11
    {
        static string ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            return reader.ReadLine();
        }

        // Numbering system for the hexagonal tiles:
        //
        //  0,0     2,0     4,0     6,0
        //      1,1     3,1     5,1
        //  0,2     2,2     4,2     6,2
        //      1,3     3,3     5,3
        //  0,4     2,4     4,4     6,4
        //      1,5     3,5     5,5
        //  0,6     2,6     4,6     6,6
        //
        // - Vertical neighbors have same x and y +- 2
        // - Diagonal neighbors have x +- 1 and y +- 1
        // 
        static readonly Dictionary<string, Position> directions = new Dictionary<string, Position>()
        {
            { "n", new Position(0, -2) },
            { "ne", new Position(1, -1) },
            { "se", new Position(1, 1) },
            { "s", new Position(0, 2) },
            { "sw", new Position(-1, 1) },
            { "nw", new Position(-1, -1) },
        };

        static void PartAB()
        {
            string input = ReadInput();
            string[] d = input.Split(',').ToArray();
            Position start = new Position(0, 0);
            Position targetPos = start;
            List<Position> allPositions = new List<Position>() { start };
            foreach (string s in d)
            {
                targetPos += directions[s];
                allPositions.Add(targetPos);
            }
            int xMin = allPositions.Select(a => a.x).Min();
            int xMax = allPositions.Select(a => a.x).Max();
            int yMin = allPositions.Select(a => a.y).Min();
            int yMax = allPositions.Select(a => a.y).Max();
            bool InRange(Position p)
            {
                return p.x >= xMin && p.x <= xMax && p.y >= yMin && p.y <= yMax;
            }
            Dictionary<Position, int> steps = new Dictionary<Position, int>() { { start, 0 } };
            List<Position> toCheck = new List<Position>() { start };
            while (toCheck.Count > 0)
            {
                //Console.WriteLine("Checking {0} positions ({1} checked before)", toCheck.Count, steps.Count);
                Console.Write(".");
                List<Position> nextToCheck = new List<Position>();
                foreach (Position p in toCheck)
                {
                    int k = steps[p];
                    foreach (var kvp in directions)
                    {
                        Position n = p + kvp.Value;
                        if (InRange(n) && !steps.ContainsKey(n) && !toCheck.Contains(n))
                        {
                            steps[n] = steps[p] + 1;
                            nextToCheck.Add(n);
                        }
                    }
                }
                toCheck = nextToCheck;
            }
            Console.WriteLine();
            Console.WriteLine("Part A: Result is {0}.", steps[targetPos]);
            int maxSteps = int.MinValue;
            foreach (Position p in allPositions)
                maxSteps = Math.Max(maxSteps, steps[p]);
            Console.WriteLine("Part B: Result is {0}. (Calculated steps for {1} cells)", maxSteps, steps.Count);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day11).Namespace + ":");
            PartAB();
        }
    }
}
