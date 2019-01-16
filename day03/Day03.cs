using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day03
{
    public struct Position : IComparable<Position>
    {
        public int x;
        public int y;

        public Position(Position p)
        {
            x = p.x;
            y = p.y;
        }
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int CompareTo(Position p)    // Reading order
        {
            if (x == p.x && y == p.y)
                return 0;
            else if (y == p.y)
                return (x < p.x) ? -1 : 1;
            else
                return (y < p.y) ? -1 : 1;
        }
        public override bool Equals(Object obj)
        {
            return obj is Position && Equals((Position)obj);
        }
        public bool Equals(Position p)
        {
            return (x == p.x) && (y == p.y);
        }
        public override int GetHashCode()
        {
            var hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }
        public static bool operator ==(Position p1, Position p2)
        {
            return p1.Equals(p2);
        }
        public static bool operator !=(Position p1, Position p2)
        {
            return !p1.Equals(p2);
        }
        public static Position operator +(Position p1, int k)
        {
            return p1 + new Position(k, k);
        }
        public static Position operator +(Position p1, Position p2)
        {
            Position p = new Position(p1);
            p.x += p2.x;
            p.y += p2.y;
            return p;
        }
        public static Position operator -(Position p1, int k)
        {
            return p1 + (-k);
        }
        public static Position operator -(Position p1, Position p2)
        {
            Position p = new Position(p1);
            p.x -= p2.x;
            p.y -= p2.y;
            return p;
        }
        public static Position operator *(Position p1, int k)
        {
            Position p = new Position(p1);
            p.x *= k;
            p.y *= k;
            return p;
        }
        public static Position operator /(Position p1, int k)
        {
            Position p = new Position(p1);
            p.x /= k;
            p.y /= k;
            return p;
        }
        public int ManhattanDistance(Position p = new Position())
        {
            return Math.Abs(x - p.x) + Math.Abs(y - p.y);
        }
    }

    class Day03
    {
        static readonly Position goUp = new Position(0, -1);
        static readonly Position goLeft = new Position(-1, 0);
        static readonly Position goRight = new Position(1, 0);
        static readonly Position goDown = new Position(0, 1);
        static readonly List<Position> directions = new List<Position>()
        {
            goRight, goUp, goLeft, goDown,
        };

        static void PartA()
        {
            const int input = 347991;
            Dictionary<Position, int> visited = new Dictionary<Position, int>();
            Position pos = new Position(0, 0);
            int value = 1;
            visited[pos] = value;
            int curDirection = directions.Count - 1;
            do
            {
                Position nextPos = pos + directions[(curDirection + 1) % directions.Count];
                if (!visited.ContainsKey(nextPos))
                    curDirection = (curDirection + 1) % directions.Count;
                else
                    nextPos = pos + directions[curDirection];
                pos = nextPos;
                visited[pos] = ++value;
            }
            while (value < input);
            Console.WriteLine("Part A: Result is {0}.", pos.ManhattanDistance());
        }

        static readonly List<Position> neighbors = new List<Position>()
        {
            goRight, goRight + goUp, goUp, goUp + goLeft, goLeft, goLeft + goDown, goDown, goDown + goRight,
        };

        static int GetAdjacentSquaresSum(Position pos, ref Dictionary<Position, int> visited)
        {
            int sum = 0;
            foreach (Position n in neighbors)
            {
                if (visited.ContainsKey(pos + n))
                    sum += visited[pos + n];
            }
            return sum;
        }

        static void PartB()
        {
            const int input = 347991;
            Dictionary<Position, int> visited = new Dictionary<Position, int>();
            Position pos = new Position(0, 0);
            int value = 1;
            visited[pos] = value;
            int curDirection = directions.Count - 1;
            do
            {
                Position nextPos = pos + directions[(curDirection + 1) % directions.Count];
                if (!visited.ContainsKey(nextPos))
                    curDirection = (curDirection + 1) % directions.Count;
                else
                    nextPos = pos + directions[curDirection];
                pos = nextPos;
                value = GetAdjacentSquaresSum(pos, ref visited);
                visited[pos] = value;
            }
            while (value <= input);
            Console.WriteLine("Part B: Result is {0}.", value);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day03).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
