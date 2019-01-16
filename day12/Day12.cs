using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode;
using Node = AdventOfCode.Graph.Node<int>;

namespace day12
{
    class Day12
    {
        static HashSet<Node> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            int[] data = { 0, 1, 2 };
            Dictionary<int, Node> dict = new Dictionary<int, Node>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] v = line.Split(' ').ToArray();
                int id = int.Parse(v[0]);
                if (!dict.ContainsKey(id))
                    dict[id] = new Node(id);
                Node n = dict[id];
                for (int i = 2; i < v.Length; i++)
                {
                    int childId = int.Parse(v[i].TrimEnd(','));
                    if (!dict.ContainsKey(childId))
                        dict[childId] = new Node(childId);
                    Node cn = dict[childId];
                    n.edges.Add(cn);
                    cn.edges.Add(n);
                }
            }
            return dict.Values.ToHashSet();
        }

        static HashSet<Node> GetGroup(Node member)
        {
            HashSet<Node> group = new HashSet<Node>() { member };
            List<Node> toCheck = new List<Node>() { member };
            while (toCheck.Count > 0)
            {
                List<Node> nextToCheck = new List<Node>();
                foreach (Node n in toCheck)
                {
                    foreach (Node nc in n.edges)
                    {
                        if (!group.Contains(nc))
                        {
                            group.Add(nc);
                            nextToCheck.Add(nc);
                        }
                    }
                }
                toCheck = nextToCheck;
            }
            return group;
        }

        static void PartA()
        {
            HashSet<Node> input = ReadInput();
            HashSet<Node> group = GetGroup(input.First());
            Console.WriteLine("Part A: Result is {0} members in group 0.", group.Count);
        }

        static void PartB()
        {
            HashSet<Node> input = ReadInput();
            int groups = 0;
            while (input.Count > 0)
            {
                HashSet<Node> group = GetGroup(input.First());
                groups++;
                foreach (Node n in group)
                    input.Remove(n);
            }
            Console.WriteLine("Part B: Result is {0} groups.", groups);
        }

        static void PartAB2()
        {
            HashSet<Node> input = ReadInput();
            HashSet<Node> R = new HashSet<Node>();
            HashSet<Node> P = new HashSet<Node>(input);
            HashSet<Node> X = new HashSet<Node>();
            List<HashSet<Node>> cliques = new List<HashSet<Node>>();
            Graph.BronKerbosch2(R, P, X, ref cliques);
            int members = cliques.Where(x => x.Contains(input.First())).First().Count;
            int groups = cliques.Count;
            Console.WriteLine("Part A2: Result is {0} members in group 0.", members);
            Console.WriteLine("Part B2: Result is {0} groups.", groups);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day12).Namespace + ":");
            PartA();
            PartB();
            //PartAB2();
        }
    }
}
