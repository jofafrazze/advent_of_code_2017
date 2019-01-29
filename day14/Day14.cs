using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Position = AdventOfCode.GenericPosition2D<int>;

namespace day14
{
    class Day14
    {
        static List<byte> CalculateKnotHash(string input)
        {
            List<byte> hash = new List<byte>();
            List<int> lengths = Encoding.ASCII.GetBytes(input).Select(x => (int)x).ToList();
            lengths.AddRange(new List<int>() { 17, 31, 73, 47, 23 });
            List<int> list = Enumerable.Range(0, 256).ToList();
            int pos = 0;
            int skip = 0;
            void DoKnotHashRound()
            {
                foreach (int l in lengths)
                {
                    List<int> list2 = list.Concat(list).ToList();
                    List<int> copy = list2.GetRange(pos, l);
                    copy.Reverse();
                    List<int> list3 = list2.GetRange(0, pos).Concat(copy).Concat(list2.GetRange(pos + l, (list.Count * 2) - l - pos)).ToList();
                    int newStartLength = Math.Max(0, (pos + l) - list.Count);
                    List<int> newListStart = list3.GetRange(list.Count, newStartLength).ToList();
                    list = newListStart.Concat(list3.GetRange(newStartLength, list.Count - newStartLength)).ToList();
                    pos += l + skip++;
                    pos %= list.Count;
                }
            }
            for (int i = 0; i < 64; i++)
                DoKnotHashRound();
            for (int i = 0; i < 16; i++)
            {
                int sum = 0;
                for (int n = 0; n < 16; n++)
                    sum ^= list[i * 16 + n];
                hash.Add((byte)sum);
            }
            return hash;
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
            string input = "hwlqcszp"; //"flqrgnkx";
            HashSet<Position> setBits = new HashSet<Position>();
            for (int i = 0; i < 128; i++)
            {
                string s = input + "-" + i.ToString();
                List<byte> hash = CalculateKnotHash(s);
                for (int x = 0; x < 16; x++)
                {
                    for (int n = 0; n < 8; n++)
                    {
                        if ((hash[x] & (1 << n)) > 0)
                        {
                            setBits.Add(new Position(x * 8 + (7 - n), i));
                        }
                    }
                }
            }
            void TurnOffRegion(Position p)
            {
                if (setBits.Contains(p))
                {
                    setBits.Remove(p);
                    foreach (Position n in directions)
                        TurnOffRegion(p + n);
                }
            }
            int regions = 0;
            while (setBits.Count > 0)
            {
                TurnOffRegion(setBits.First());
                regions++;
            }
            Console.WriteLine("Part B: Result is {0}.", regions);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day14).Namespace + ":");
            PartAB();
        }
    }
}
