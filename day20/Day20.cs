using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Position = AdventOfCode.GenericPosition3D<long>;

namespace day20
{
    struct Particle
    {
        public int id;
        public Position p;
        public Position v;
        public Position a;
        public Particle(int id, Position p, Position v, Position a)
        {
            this.id = id;
            this.p = p;
            this.v = v;
            this.a = a;
        }
        public Particle(Particle p) : this(p.id, p.p, p.v, p.a) { }
    }
    class Day20
    {
        static List<Particle> ReadInput()
        {
            Position StrToPos(string s)
            {
                int[] v = s.Substring(3, s.Length - 4).Split(',').Select(int.Parse).ToArray();
                return new Position(v[0], v[1], v[2]);
            }
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<Particle> list = new List<Particle>();
            string line;
            int n = 0;
            while ((line = reader.ReadLine()) != null)
            {
                string[] s = line.Split(", ").ToArray();
                Particle p = new Particle
                {
                    id = n++,
                    p = StrToPos(s[0]),
                    v = StrToPos(s[1]),
                    a = StrToPos(s[2])
                };
                list.Add(p);
            }
            return list;
        }

        static void PartA()
        {
            List<Particle> particles = ReadInput();
            Particle mostOrigoBound = particles.
                OrderBy(x => x.a.ManhattanDistance()).
                ThenBy(x => x.v.ManhattanDistance()).
                ThenBy(x => x.p.ManhattanDistance()).
                First();
            Console.WriteLine("Part A: Result is {0}.", mostOrigoBound.id);
        }

        static List<Particle> MoveParticles(List<Particle> particles)
        {
            List<Particle> moved = new List<Particle>();
            foreach (Particle p in particles)
            {
                Particle n = p;
                n.v += n.a;
                n.p += n.v;
                moved.Add(n);
            }
            return moved;
        }

        static bool SortedInAllDimensions(List<Particle> particles)
        {
            bool sorted = true;
            List<Particle> p;
            p = particles.OrderBy(n => n.p.x).ToList();
            sorted &= p.Select(n => n.id).SequenceEqual(p.OrderBy(n => n.v.x).Select(n => n.id));
            sorted &= p.Select(n => n.id).SequenceEqual(p.OrderBy(n => n.a.x).Select(n => n.id));
            p = particles.OrderBy(n => n.p.y).ToList();
            sorted &= p.Select(n => n.id).SequenceEqual(p.OrderBy(n => n.v.y).Select(n => n.id));
            sorted &= p.Select(n => n.id).SequenceEqual(p.OrderBy(n => n.a.y).Select(n => n.id));
            p = particles.OrderBy(n => n.p.z).ToList();
            sorted &= p.Select(n => n.id).SequenceEqual(p.OrderBy(n => n.v.z).Select(n => n.id));
            sorted &= p.Select(n => n.id).SequenceEqual(p.OrderBy(n => n.a.z).Select(n => n.id));
            return sorted;
        }

        static void PartB()
        {
            List<Particle> particles = ReadInput();
            int iter = 0;
            while (!SortedInAllDimensions(particles))
            {
                HashSet<Position> visited = new HashSet<Position>();
                List<Position> collisions = new List<Position>();
                foreach (Particle p in particles)
                {
                    if (visited.Contains(p.p))
                        collisions.Add(p.p);
                    else
                        visited.Add(p.p);
                }
                particles = particles.Where(x => !collisions.Contains(x.p)).ToList();
                particles = MoveParticles(particles);
                iter++;
            }
            Console.WriteLine("Part B: Result is {0} (took {1} iterations).", particles.Count, iter);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day20).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
