using System;
using Adventofcode2025.Utilities;
using Google.OrTools.Sat;

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
            List<FactoryMachine> machines = GetMachines(test);
            long sum = 0;

            foreach (FactoryMachine machine in machines)
            {
                (int[,] A, int[] b) = ToLinearProgrammingProblem(machine);
                int[] x = SolveMinSumNonnegative(A, b, true);
                sum += x.Sum();
            }

            return sum.ToString();
        }

        private (int[,], int[]) ToLinearProgrammingProblem(FactoryMachine machine)
        {
            var joltages = machine.Joltages.Split(',').Select(c => Int32.Parse(c)).ToArray();
            var A = new int[joltages.Length, machine.Buttons.Count];
            for (int i = 0; i < joltages.Length; i++)
            {
                for (int j = 0; j < machine.Buttons.Count; j++)
                {
                    if (machine.Buttons[j].Contains(i))
                    {
                        A[i, j] = 1;
                    }
                }
            }

            // A is j x b, b is j x 1, we're looking for b x 1 array x.
            return (A, joltages);
        }

        public static int[] SolveMinSumNonnegative(int[,] A, int[] b, bool transposeIfNeeded)
        {
            // Dimension checks and optional transpose to interpret A as n x m.
            int n = A.GetLength(0);
            int m = A.GetLength(1);

            if (b == null || b.Length == 0)
                throw new ArgumentException("b must be a non-empty int array.");

            // If A does not match n == b.Length, but its columns do, try transposing.
            if (n != b.Length && transposeIfNeeded && m == b.Length)
            {
                A = Transpose(A);
                n = A.GetLength(0);
                m = A.GetLength(1);
            }

            if (n != b.Length)
                throw new ArgumentException($"Dimension mismatch: A has {n} rows, but b has length {b.Length}.");

            // Validate entries of A are 0/1 as specified.
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    int v = A[i, j];
                    if (v != 0 && v != 1)
                        throw new ArgumentException("A must contain only 0/1 entries.");
                }

            // Assume b is nonnegative for a well-posed nonnegative integer solution.
            if (b.Any(val => val < 0))
            {
                throw new ArgumentException(
                    "This solver assumes x >= 0 and b >= 0. Found negative b. " +
                    "Please confirm if you want signed x with finite bounds and I will adapt the model.");
            }

            // A safe upper bound for x is max(b). If all b are zero, UB=0.
            int UB = b.Length == 0 ? 0 : b.Max();
            if (UB < 0) UB = 0; // defensive

            // Build CP-SAT model.
            var model = new CpModel();

            // Decision variables: x_j in [0, UB], integer.
            IntVar[] x = new IntVar[m];
            for (int j = 0; j < m; j++)
            {
                x[j] = model.NewIntVar(0, UB, $"x_{j}");
            }

            // Constraints: for each row i, sum_j A[i,j] * x_j == b[i]
            for (int i = 0; i < n; i++)
            {
                // Collect terms where A[i,j] == 1
                var terms = x
                    .Select((var, j) => (var, coef: A[i, j]))
                    .Where(t => t.coef != 0)
                    .Select(t => t.var)
                    .ToArray();

                if (terms.Length == 0)
                {
                    // Row i is all zeros: requires b[i] == 0 or infeasible.
                    if (b[i] != 0)
                    {
                        // Infeasible immediately.
                        return null;
                    }
                    // Else it's vacuous, no constraint needed.
                    continue;
                }

                // Since all coefs are 1 here, sum terms == b[i]
                model.Add(LinearExpr.Sum(terms) == b[i]);
            }

            // Objective: Minimize sum(x_j)
            var objective = LinearExpr.Sum(x);
            model.Minimize(objective);

            // Solve
            CpSolver solver = new CpSolver();
            solver.StringParameters = "num_search_workers:8"; // parallelism if available
            var status = solver.Solve(model);

            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            {
                int[] result = new int[m];
                for (int j = 0; j < m; j++)
                {
                    result[j] = (int)solver.Value(x[j]);
                }
                return result;
            }

            // UNSAT or UNKNOWN
            return null;
        }

        private static int[,] Transpose(int[,] A)
        {
            int r = A.GetLength(0);
            int c = A.GetLength(1);
            int[,] T = new int[c, r];
            for (int i = 0; i < r; i++)
                for (int j = 0; j < c; j++)
                    T[j, i] = A[i, j];
            return T;
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
