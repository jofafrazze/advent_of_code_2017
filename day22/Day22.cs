using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode;

using Position = AdventOfCode.GenericPosition2D<int>;

namespace day22
{
    class Day22
    {
        static Map ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<string> list = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
                list.Add(line);
            int w = list[0].Length;
            int h = list.Count;
            Map map = new Map(w, h, new Position(w / 2, h / 2));
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    map.data[x, y] = list[y][x];
            return map;
        }

        static readonly Position goUp = new Position(0, -1);
        static readonly Position goRight = new Position(1, 0);
        static readonly Position goDown = new Position(0, 1);
        static readonly Position goLeft = new Position(-1, 0);
        static readonly List<Position> directions = new List<Position>()
        {
            goUp, goRight, goDown, goLeft,
        };

        static void PartA()
        {
            Map map = ReadInput();
            int dir = 0;
            map.Expand(1000, '.');
            //map.Print();
            int infectionBursts = 0;
            for (int iter = 0; iter < 10000; iter++)
            {
                bool infected = map[map.pos] == '#';
                if (!infected)
                    infectionBursts++;
                int dirAdd = infected ? 1 : -1;
                dir = (dir + 4 + dirAdd) % 4;
                map[map.pos] = infected ? '.' : '#';
                map.pos += directions[dir];
                if (map.pos.x <= 0 || map.pos.y <= 0 || map.pos.x >= map.width - 1 || map.pos.y >= map.height - 1)
                {
                    Console.WriteLine("Error! Reached edge after {0} iterations, add more margin [{1}, {2}]", iter, map.pos.x, map.pos.y);
                    break;
                }
            }
            Console.WriteLine("Part A: Result is {0}.", infectionBursts);
        }

        static readonly List<char> progress = new List<char>() { '.', 'W', '#', 'F' };
        static readonly Dictionary<char, int> progressIndex = progress.Select((x, i) => new { x, i }).ToDictionary(a => a.x, a => a.i);

        static void PartB()
        {
            Map map = ReadInput();
            int dir = 0;
            map.Expand(1000, '.');
            //map.Print();
            int infectionBursts = 0;
            for (int iter = 0; iter < 10000000; iter++)
            {
                char state = map[map.pos];
                if (state == 'W')
                    infectionBursts++;
                int dirAdd = (state == '.') ? -1 : ((state == '#') ? 1 : ((state == 'F') ? 2 : 0));
                dir = (dir + 4 + dirAdd) % 4;
                map[map.pos] = progress[(progressIndex[state] + 1) % 4];
                map.pos += directions[dir];
                if (map.pos.x <= 0 || map.pos.y <= 0 || map.pos.x >= map.width - 1 || map.pos.y >= map.height - 1)
                {
                    Console.WriteLine("Error! Reached edge after {0} iterations, add more margin [{1}, {2}]", iter, map.pos.x, map.pos.y);
                    break;
                }
            }
            Console.WriteLine("Part B: Result is {0}.", infectionBursts);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day22).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
