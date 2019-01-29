using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day25
{
    class Day25
    {
        /*
        Begin in state A.
        Perform a diagnostic checksum after 12399302 steps.

        In state A:
          If the current value is 0:
            - Write the value 1.
            - Move one slot to the right.
            - Continue with state B.
          If the current value is 1:
            - Write the value 0.
            - Move one slot to the right.
            - Continue with state C.

        In state B:
          If the current value is 0:
            - Write the value 0.
            - Move one slot to the left.
            - Continue with state A.
          If the current value is 1:
            - Write the value 0.
            - Move one slot to the right.
            - Continue with state D.

        In state C:
          If the current value is 0:
            - Write the value 1.
            - Move one slot to the right.
            - Continue with state D.
          If the current value is 1:
            - Write the value 1.
            - Move one slot to the right.
            - Continue with state A.

        In state D:
          If the current value is 0:
            - Write the value 1.
            - Move one slot to the left.
            - Continue with state E.
          If the current value is 1:
            - Write the value 0.
            - Move one slot to the left.
            - Continue with state D.

        In state E:
          If the current value is 0:
            - Write the value 1.
            - Move one slot to the right.
            - Continue with state F.
          If the current value is 1:
            - Write the value 1.
            - Move one slot to the left.
            - Continue with state B.

        In state F:
          If the current value is 0:
            - Write the value 1.
            - Move one slot to the right.
            - Continue with state A.
          If the current value is 1:
            - Write the value 1.
            - Move one slot to the right.
            - Continue with state E.
        */

        static void PartA()
        {
            Dictionary<int, int> tape = new Dictionary<int, int>();
            int GetValue(int index)
            {
                if (!tape.ContainsKey(index))
                    tape[index] = 0;
                return tape[index];
            }
            void SetValue(int index, int val)
            {
                tape[index] = val;
            }

            int state = 0;
            int cursor = 0;
            const int iterations = 12399302;
            for (int i = 0; i < iterations; i++)
            {
                int nextState = -1;
                int currentValue = GetValue(cursor);
                if (state == 0) // A
                {
                    if (currentValue == 0)
                    {
                        SetValue(cursor, 1);
                        cursor++;
                        nextState = 1;
                    }
                    else
                    {
                        SetValue(cursor, 0);
                        cursor++;
                        nextState = 2;
                    }
                }
                else if (state == 1) // B
                {
                    if (currentValue == 0)
                    {
                        SetValue(cursor, 0);
                        cursor--;
                        nextState = 0;
                    }
                    else
                    {
                        SetValue(cursor, 0);
                        cursor++;
                        nextState = 3;
                    }
                }
                else if (state == 2) // C
                {
                    if (currentValue == 0)
                    {
                        SetValue(cursor, 1);
                        cursor++;
                        nextState = 3;
                    }
                    else
                    {
                        SetValue(cursor, 1);
                        cursor++;
                        nextState = 0;
                    }
                }
                else if (state == 3) // D
                {
                    if (currentValue == 0)
                    {
                        SetValue(cursor, 1);
                        cursor--;
                        nextState = 4;
                    }
                    else
                    {
                        SetValue(cursor, 0);
                        cursor--;
                        nextState = 3;
                    }
                }
                else if (state == 4) // E
                {
                    if (currentValue == 0)
                    {
                        SetValue(cursor, 1);
                        cursor++;
                        nextState = 5;
                    }
                    else
                    {
                        SetValue(cursor, 1);
                        cursor--;
                        nextState = 1;
                    }
                }
                else if (state == 5) // F
                {
                    if (currentValue == 0)
                    {
                        SetValue(cursor, 1);
                        cursor++;
                        nextState = 0;
                    }
                    else
                    {
                        SetValue(cursor, 1);
                        cursor++;
                        nextState = 4;
                    }
                }
                state = nextState;
            }
            int ones = tape.Select(kvp => kvp.Value).Where(x => x == 1).Count();
            Console.WriteLine("Part A: Result is {0}.", ones);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("AoC 2017 - " + typeof(Day25).Namespace + ":");
            PartA();
        }
    }
}
