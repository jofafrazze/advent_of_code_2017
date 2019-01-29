using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day23
{
    class Day23
    {
        struct OpCode
        {
            public string name;
            public Func<char, string, bool> func;
            public OpCode(string n, Func<char, string, bool> f) { name = n; func = f; }
        };

        struct Instruction
        {
            public OpCode opCode;
            public char register;
            public string parameter;
            public bool Execute()
            {
                return opCode.func(register, parameter);
            }
        };

        class Executable
        {
            public List<Instruction> program;
            public int pc;
            public Dictionary<char, long> registers;
            public List<OpCode> opCodes;
            public Dictionary<string, int> instructionSet;
            public int nMul;
            public Executable()
            {
                program = new List<Instruction>();
                pc = 0;
                registers = new Dictionary<char, long>();
                opCodes = new List<OpCode>()
                {
                    new OpCode("set", delegate(char x, string y) { InitReg(x); registers[x] = GetValue(y); return true; }),
                    new OpCode("sub", delegate(char x, string y) { InitReg(x); registers[x] -= GetValue(y); return true; }),
                    new OpCode("mul", delegate(char x, string y) { InitReg(x); registers[x] *= GetValue(y); nMul++; return true; }),
                    new OpCode("jnz", delegate(char x, string y) { InitReg(x); if (GetValue(x) != 0) pc += (int)(GetValue(y) - 1); return true; }),
                };
                instructionSet = opCodes.Select((x, i) => new { x, i }).ToDictionary(a => a.x.name, a => a.i);
                nMul = 0;
            }
            public void InitReg(char c)
            {
                if (!registers.ContainsKey(c))
                    registers[c] = 0;
            }
            public long GetValue(char c) { return GetValue(c.ToString()); }
            public long GetValue(string s)
            {
                if (s.Length == 1 && Char.IsLetter(s[0]))
                {
                    InitReg(s[0]);
                    return registers[s[0]];
                }
                else
                    return int.Parse(s);
            }
            public bool Execute(out int instructionsExecuted)
            {
                instructionsExecuted = 0;
                bool keepOn = true;
                while ((pc >= 0) && (pc < program.Count) && keepOn)
                {
                    instructionsExecuted++;
                    keepOn &= program[pc].Execute();
                    if (keepOn)
                        pc++;
                }
                return !keepOn;
            }

        };

        static List<Instruction> ReadInput(Executable exe)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\input.txt");
            StreamReader reader = File.OpenText(path);
            List<Instruction> list = new List<Instruction>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Instruction i = new Instruction();
                string[] s = line.Split(' ').ToArray();
                i.opCode = exe.opCodes[exe.instructionSet[s[0]]];
                i.register = s[1][0];
                i.parameter = (s.Count() > 2) ? s[2] : "";
                list.Add(i);
            }
            return list;
        }

        static void PartA()
        {
            Executable exe = new Executable();
            exe.program = ReadInput(exe);
            exe.Execute(out int n);
            Console.WriteLine("Exe executed {0} instructions", n);
            Console.WriteLine("Part A: Result is {0}.", exe.nMul);
        }

        static void PartB1()
        {
            int b = 0;
            int c = 0;
            int d = 0;
            int e = 0;
            int f = 0;
            int g = 0;
            int h = 0;

            b = 84;
            c = b;
            b *= 100;
            b += 100000;
            c = b + 17000;

            // Original here: patched version below
            //while (true)
            //{
            //    f = 1;
            //    d = 2;
            //    do
            //    {
            //        e = 2;
            //        do
            //        {
            //            g = d;
            //            g *= e;
            //            g -= b;
            //            if (g == 0)
            //                f = 0;
            //            e++;
            //            g = e;
            //            g -= b;
            //        }
            //        while (g != 0);
            //        d++;
            //        g = d;
            //        g -= b;
            //    }
            //    while (g != 0);
            //    if (f == 0)
            //        h++;
            //    g = b;
            //    g -= c;
            //    if (g == 0)
            //        break;
            //    b += 17;
            //}

            while (true)
            {
                f = 1;
                d = 2;
                do
                {
                    e = 2;
                    do
                    {
                        g = d;
                        g *= e;
                        g -= b;
                        if (g == 0)
                            f = 0;
                        e++;
                    }
                    while (g <= 0);
                    d++;
                }
                while (d <= b / 2);
                if (f == 0)
                    h++;
                g = b;
                g -= c;
                if (g == 0)
                    break;
                b += 17;
            }
            Console.WriteLine("Part B1: Result is {0}.", h);
        }

        static void PartB2()
        {
            int h = 0;
            int b = (84 * 100) + 100000;
            int c = b + 17000;
            while (b <= c)
            {
                bool inc = false;
                for (int d = 2; (d <= b / 2) && !inc; d++)
                {
                    for (int e = 2; (e <= b / d) && !inc; e++)
                    {
                        if (d * e == b)
                            inc = true;
                    }
                }
                if (inc)
                    h++;
                b += 17;
            }
            Console.WriteLine("Part B2: Result is {0}.", h);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day23).Namespace + ":");
            PartA();
            PartB1();
            PartB2();
        }
    }
}
