using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day24
{
    public struct Component
    {
        public int port1;
        public int port2;
    }

    class Day24
    {
        static List<Component> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<Component> list = new List<Component>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                int[] d = line.Split('/').Select(int.Parse).ToArray();
                Array.Sort(d);
                list.Add(new Component() { port1 = d[0], port2 = d[1] });
            }
            return list;
        }

        static int GetStrength(List<Component> bridge)
        {
            int sum = 0;
            foreach (Component c in bridge)
                sum += (c.port1 + c.port2);
            return sum;
        }

        static void PartA()
        {
            List<List<Component>> bridges = new List<List<Component>>();
            List<Component> input = ReadInput();
            void FindBridges(List<Component> used, int port, List<Component> components)
            {
                List<Component> nextStep = components.Where(x => x.port1 == port || x.port2 == port).ToList();
                if (nextStep.Count > 0)
                {
                    foreach (Component c in nextStep)
                    {
                        List<Component> cList = new List<Component>() { c };
                        List<Component> nextUsed = used.Concat(cList).ToList();
                        int nextPort = (c.port1 == port) ? c.port2 : c.port1;
                        List<Component> nextComponents = components.Except(cList).ToList();
                        FindBridges(nextUsed, nextPort, nextComponents);
                    }
                }
                else
                {
                    if (used.Count > 0)
                        bridges.Add(used);
                }
            }
            List<Component> usedComponents = new List<Component>();
            List<Component> availableComponents = new List<Component>(input);
            FindBridges(usedComponents, 0, availableComponents);
            int maxStrength = bridges.Select(x => GetStrength(x)).Max();
            Console.WriteLine("Part A: Result is {0} (out of {1} possible bridges).", maxStrength, bridges.Count);
            int maxLength = bridges.Select(x => x.Count).Max();
            List<List<Component>> longBridges = bridges.Where(x => x.Count == maxLength).ToList();
            int maxStrength2 = longBridges.Select(x => GetStrength(x)).Max();
            Console.WriteLine("Part B: Result is {0} (out of {1} possible bridges).", maxStrength2, longBridges.Count);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day24).Namespace + ":");
            PartA();
        }
    }
}
