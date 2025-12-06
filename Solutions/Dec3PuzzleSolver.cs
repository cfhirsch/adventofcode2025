using System.Text;
using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec3PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            return Solve(test, 2);
        }

        public string SolvePartTwo(bool test)
        {
            return Solve(test, 12);
        }

        private static string Solve(bool test, int n)
        {
            long totalJoltage = 0;

            foreach (string line in PuzzleReader.GetPuzzleInput(3, test))
            {
                totalJoltage += FindMaxJotage(line, n);
            }

            return totalJoltage.ToString();
        }

        private static long FindMaxJotage(string bank, int n)
        {
            int index = 0;
            var sb = new StringBuilder();
            int m = n;
            for (int i = 0; i < n; i++)
            {
                if (index + m > bank.Length)
                {
                    sb.Append(bank.Substring(index));
                    break;
                }
                
                int max = bank.Substring(index, bank.Length - m - index + 1).Select(s => Int32.Parse(s.ToString())).Max();
                index = bank.IndexOf(max.ToString(), index) + 1;
                sb.Append(max);
                m--;
            }

            return Int64.Parse(sb.ToString());
        }
    }
}
