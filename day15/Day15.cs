using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day15
{
    class Day15
    {
        static int generateNextNumber(int n, long factor)
        {
            return (int)((n * factor) % 2147483647);
        }

        static void PartAB()
        {
            const int aStart = 634; //65;
            const int bStart = 301; //8921;
            const int aFactor = 16807;
            const int bFactor = 48271;
            int a = aStart;
            int b = bStart;
            int sum = 0;
            for (int i = 0; i < 40000000; i++)
            {
                a = generateNextNumber(a, aFactor);
                b = generateNextNumber(b, bFactor);
                if ((a & 0xffff) == (b & 0xffff))
                    sum++;
            }
            Console.WriteLine("Part A: Result is {0}.", sum);

            a = aStart;
            b = bStart;
            sum = 0;
            for (int i = 0; i < 5000000; i++)
            {
                while ((a = generateNextNumber(a, aFactor)) % 4 != 0)
                    ;
                while ((b = generateNextNumber(b, bFactor)) % 8 != 0)
                    ;
                if ((a & 0xffff) == (b & 0xffff))
                    sum++;
            }
            Console.WriteLine("Part B: Result is {0}.", sum);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day15).Namespace + ":");
            PartAB();
        }
    }
}
