using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day07
{
    class Node
    {
        public string name;
        public int weight;
        public Node parent;
        public List<Node> children;
        public Node(string n)
        {
            name = n;
            children = new List<Node>();
        }
    }

    class Day07
    {
        static Dictionary<string, Node> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            int[] data = { 0, 1, 2 };
            Dictionary<string, Node> dict = new Dictionary<string, Node>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] v = line.Split(' ').ToArray();
                if (!dict.ContainsKey(v[0]))
                    dict[v[0]] = new Node(v[0]);
                Node n = dict[v[0]];
                n.weight = int.Parse(v[1].TrimStart('(').TrimEnd(')'));
                for (int i = 3; i < v.Length; i++)
                {
                    string child = v[i].TrimEnd(',');
                    if (!dict.ContainsKey(child))
                        dict[child] = new Node(child);
                    Node cn = dict[child];
                    n.children.Add(cn);
                    cn.parent = n;
                }
                dict[v[0]] = n;
            }
            return dict;
        }

        static void PartA()
        {
            Dictionary<string, Node> tree = ReadInput();
            string rootName = tree.Values.Where(x => x.parent == null).ToList().First().name;
            Console.WriteLine("Part A: Result is {0}.", rootName);
        }

        static void PartB()
        {
            Dictionary<string, Node> tree = ReadInput();
            Dictionary<string, int> totalWeights = new Dictionary<string, int>();
            int CalculateTotalNodeWeight(string nodeName)
            {
                Node node = tree[nodeName];
                int sum = node.weight;
                foreach (Node child in node.children)
                    sum += CalculateTotalNodeWeight(child.name);
                totalWeights[nodeName] = sum;
                return sum;
            }
            string rootName = tree.Values.Where(x => x.parent == null).ToList().First().name;
            CalculateTotalNodeWeight(rootName);
            Node badChild = null;
            int badChildsCorrectedWeight = 0;
            void CheckTreeBalance(string nodeName)
            {
                Node node = tree[nodeName];
                foreach (Node child in node.children)
                    CheckTreeBalance(child.name);
                List<int> childWeights = new List<int>();
                foreach (Node child in node.children)
                    childWeights.Add(totalWeights[child.name]);
                bool balanced = (childWeights.Count == 0) || (childWeights.Min() == childWeights.Max());
                if (!balanced && (badChild == null))
                {
                    int median = childWeights.OrderBy(x => x).ElementAt(childWeights.Count / 2);
                    for (int i = 0; (i < node.children.Count) && (badChild == null); i++)
                    {
                        if (totalWeights[node.children[i].name] != median)
                            badChild = node.children[i];
                    }
                    badChildsCorrectedWeight = (median - totalWeights[badChild.name]) + badChild.weight;
                }
            }
            CheckTreeBalance(rootName);
            Console.WriteLine("Part B: Result is {0}.", badChildsCorrectedWeight);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day07).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
