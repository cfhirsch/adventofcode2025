using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec10PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            List<FactoryMachine> machines = GetMachines(test);
            long sum = 0;

            foreach (FactoryMachine machine in machines)
            {
                string startState = new string('.', machine.Lights.Length);

                int presses = MinButtonPresses(
                    startState,
                    machine.Lights,
                    machine.Buttons);

                sum += presses;
            }

            return sum.ToString();

        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

       
        private static List<FactoryMachine> GetMachines(bool test)
        {
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
                    else if (line[pos] == '{')
                    {
                        machine.Joltages = line.Substring(pos + 1, line.IndexOf('}') - pos - 1);
                        break;
                    }
                    else
                    {
                        pos++;
                    }
                }

                machines.Add(machine);
            }

            return machines;
        }

        /*
         * Can I build up a dictionary memoized[state] of minimum button presses from state to target.
         * memoized[target] = 0
         * memoized[neighbor] = 1 for every neighbor of target
         * 
         */
        private static int MinButtonPresses(
            string state, 
            string target, 
            List<List<int>> buttons)
        {
            var distances = new Dictionary<string, int>();
            distances[target] = 0;
            int dist = 0;

            while (true)
            {
                IEnumerable<string> keys = distances.Where(kvp => kvp.Value == dist).Select(kvp => kvp.Key).ToList();
                foreach (string key in keys)
                {
                    foreach (List<int> button in buttons)
                    {
                        string nextState = GetNextState(key, button);
                        if (!distances.ContainsKey(nextState))
                        {
                            distances[nextState] = dist + 1;
                        }

                        if (nextState == state)
                        {
                            return distances[nextState];
                        }
                    }
                }

                dist++;
            }
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

            public string Joltages { get; set; }
        }
    }
}
