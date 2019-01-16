using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day04
{
    class Day04
    {
        static List<string> ReadInput()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<string> list = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                list.Add(line);
            }
            return list;
        }

        static List<string> StripLinesWithSameWordMoreThanOnce(List<string> input)
        {
            List<string> list = new List<string>();
            foreach (string line in input)
            {
                string[] v = line.Split(' ').ToArray();
                if (v.ToHashSet().Count == v.Count())
                    list.Add(line);
            }
            return list;
        }

        static void PartA()
        {
            List<string> input = ReadInput();
            int sum = StripLinesWithSameWordMoreThanOnce(input).Count;
            Console.WriteLine("Part A: Result is {0} out of {1}.", sum, input.Count);
        }

        static List<List<T>> HeapPermutation<T>(List<T> a)
        {
            List<List<T>> result = new List<List<T>>();
            void Swap(ref List<T> b, int i1, int i2)
            {
                T temp = b[i1];
                b[i1] = b[i2];
                b[i2] = temp;
            }
            void Permute(ref List<T> b, int size)
            {
                if (size == 1)
                {
                    result.Add(new List<T>(b));
                }
                else
                {
                    for (int i = 0; i < size - 1; i++)
                    {
                        Permute(ref b, size - 1);
                        Swap(ref b, (size % 2 == 0) ? i : 0, size - 1);
                    }
                    Permute(ref b, size - 1);
                }
            }
            List<T> copy = new List<T>(a);
            Permute(ref copy, copy.Count);
            return result;
        }

        static void PartB()
        {
            List<string> input = ReadInput();
            List<string> lines = StripLinesWithSameWordMoreThanOnce(input);
            int sum = 0;
            foreach (string line in lines)
            {
                bool pwOk = true;
                HashSet<string> set = new HashSet<string>();
                string[] v = line.Split(' ').ToArray();
                foreach (string a in v)
                {
                    List<char> chars = new List<char>();
                    chars.AddRange(a);
                    chars = chars.ToHashSet().ToList(); // No duplicate chars
                    var permutations = HeapPermutation(chars);
                    foreach (List<char> p in permutations)
                    {
                        string ps = new string(p.ToArray());
                        if (set.Contains(ps))
                            pwOk = false;
                        set.Add(ps);
                    }
                }
                if (pwOk)
                    sum++;
            }
            Console.WriteLine("Part B: Result is {0} out of {1}.", sum, input.Count);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day04).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
