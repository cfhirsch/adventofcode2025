using System.Text.RegularExpressions;
using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec10PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            //[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}

            var machines = new List<FactoryMachine>();
            foreach (string line in PuzzleReader.GetPuzzleInput(10, test))
            {
                int pos = 0;
                var machine = new FactoryMachine();
                machine.Buttons = new List<List<int>>();

                while (pos < line.Length)
                {
                    if (line[pos] == '[')
                    {
                        machine.Lights = line.Substring(pos + 1, line.IndexOf(']') - 1);
                        pos = line.IndexOf(']') + 1;
                    }
                    else if (line[pos] == '(')
                    {
                        string buttonStr = line.Substring(pos + 1, line.IndexOf(')', pos) - pos - 1);
                        machine.Buttons.Add(new List<int>(buttonStr.Split(',').Select(s => Int32.Parse(s))));
                        pos = line.IndexOf(')', pos) + 1;
                    }
                    else
                    {
                        pos++;
                    }
                }

                machines.Add(machine);
            }

            long sum = 0;
            foreach (FactoryMachine machine in machines)
            {
                var queue = new Queue<(string, int)>();

                string startState = new string('.', machine.Lights.Length);
                queue.Enqueue((startState, 0));
                var visited = new HashSet<string>();
                long presses = 0;

                while (queue.Count > 0)
                {
                    (string state, int numPresses) = queue.Dequeue();

                    if (state == machine.Lights)
                    {
                        presses = numPresses;
                        break;
                    }

                    visited.Add(state);

                    foreach (List<int> button in machine.Buttons)
                    {
                        string nextState = GetNextState(state, button);
                        if (!visited.Contains(nextState))
                        {
                            queue.Enqueue((nextState, numPresses + 1));
                        }
                    }
                }

                sum += presses;
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private static string GetNextState(string state, List<int> button)
        {
            bool[] states = state.Select(c => c == '#').ToArray();

            foreach (int i in button)
            {
                states[i] = !states[i];
            }

            return string.Join("", states.Select(b => b ? '#' : '.').ToArray());
        }

        private class FactoryMachine
        {
            public string Lights { get; set; }

            public List<List<int>> Buttons { get; set; }
        }
    }
}
