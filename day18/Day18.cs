using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day18
{
    class Day18
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
            public Executable()
            {
                program = new List<Instruction>();
                pc = 0;
                registers = new Dictionary<char, long>();
            }
            public void Init(Func<char, string, bool> sndFunc, Func<char, string, bool> rcvFunc)
            {
                opCodes = new List<OpCode>()
                {
                    new OpCode("snd", sndFunc ),
                    new OpCode("set", delegate(char x, string y) { InitReg(x); registers[x] = GetValue(y); return true; }),
                    new OpCode("add", delegate(char x, string y) { InitReg(x); registers[x] += GetValue(y); return true; }),
                    new OpCode("mul", delegate(char x, string y) { InitReg(x); registers[x] *= GetValue(y); return true; }),
                    new OpCode("mod", delegate(char x, string y) { InitReg(x); registers[x] %= GetValue(y); return true; }),
                    new OpCode("rcv", rcvFunc ),
                    new OpCode("jgz", delegate(char x, string y) { InitReg(x); if (GetValue(x) > 0) pc += (int)(GetValue(y) - 1); return true; }),
                };
                instructionSet = opCodes.Select((x, i) => new { x, i }).ToDictionary(a => a.x.name, a => a.i);
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
            Stack<long> soundPlaying = new Stack<long>();
            Executable exe = new Executable();
            bool sndAction(char x, string _) { exe.InitReg(x); soundPlaying.Push(exe.GetValue(x)); return true; }
            bool rcvAction(char x, string _) { exe.InitReg(x); return exe.GetValue(x) == 0; }
            exe.Init(sndAction, rcvAction);
            exe.program = ReadInput(exe);
            exe.Execute(out int n);
            Console.WriteLine("Exe executed {0} instructions", n);
            Console.WriteLine("Part A: Result is {0}.", soundPlaying.Peek());
        }

        class Thread
        {
            public int id;
            public Queue<long> sendQueue;
            public int sendAmount;
            public Executable exe;
            public bool canContinue;
            public int instructionsExecuted;
            public Thread(int id)
            {
                this.id = id;
                sendQueue = new Queue<long>();
                sendAmount = 0;
                exe = new Executable();
                canContinue = true;
                instructionsExecuted = 0;
            }
        }

        static Func<T1, T2, TRes> Curry<T1, T2, T3, TRes>(Func<T1, T2, T3, TRes> func, T3 t3)
        {
            return (t1, t2) => func(t1, t2, t3);
        }
        static Func<T1, T2, TRes> Curry<T1, T2, T3, T4, TRes>(Func<T1, T2, T3, T4, TRes> func, T3 t3, T4 t4)
        {
            return (t1, t2) => func(t1, t2, t3, t4);
        }

        static void PartB()
        {
            bool sndAction(char x, string _, Thread t)
            {
                t.exe.InitReg(x);
                t.sendQueue.Enqueue(t.exe.GetValue(x));
                t.sendAmount++;
                return true;
            }
            bool rcvAction(char x, string _, Thread t, Thread other)
            {
                t.exe.InitReg(x);
                bool starved = other.sendQueue.Count == 0;
                if (!starved)
                    t.exe.registers[x] = other.sendQueue.Dequeue();
                return !starved;
            }
            Thread[] d = new Thread[2];
            for (int i = 0; i < 2; i++)
                d[i] = new Thread(i);
            for (int i = 0; i < 2; i++)
            {
                d[i].exe.registers['p'] = i;
                d[i].exe.Init(
                    Curry<char, string, Thread, bool>(sndAction, d[i]),
                    Curry<char, string, Thread, Thread, bool>(rcvAction, d[i], d[i == 0 ? 1 : 0])
                    );
                d[i].exe.program = ReadInput(d[i].exe);
            }
            bool anyThreadAlive = true;
            while (anyThreadAlive)
            {
                anyThreadAlive = false;
                for (int i = 0; i < 2; i++)
                {
                    d[i].canContinue &= d[i].exe.Execute(out d[i].instructionsExecuted);
                    bool aliveAndNotStarved = d[i].canContinue && (d[i].instructionsExecuted != 1);
                    anyThreadAlive |= aliveAndNotStarved;
                    //Console.WriteLine("Exe {0} executed {1} instructions, crashed? {2}", 
                    //    i, d[i].instructionsExecuted, d[i].canContinue ? "no" : "yes");
                }
            };
            Console.WriteLine("Part B: Result is {0}.", d[1].sendAmount);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day18).Namespace + ":");
            PartA();
            PartB();
        }
    }
}
