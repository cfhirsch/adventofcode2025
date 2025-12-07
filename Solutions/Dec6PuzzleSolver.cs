using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec6PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            var numbers = new List<List<long>>();
            var ops = new List<string>();

            var lines = PuzzleReader.GetPuzzleInput(6, test).ToList();

            for (int i = 0; i < lines.Count - 1; i++)
            {
                numbers.Add(lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => Int64.Parse(s)).ToList());
            }

            ops = lines[lines.Count - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            long sum = 0;

            for (int j = 0; j < ops.Count; j++)
            {
                string op = ops[j];

                long temp = (op == "*") ? 1 : 0;

                for (int i = 0; i < numbers.Count; i++)
                {
                    if (op == "*")
                    {
                        temp *= numbers[i][j];
                    }
                    else
                    {
                        temp += numbers[i][j];
                    }
                }

                sum += temp;
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }
    }
}
