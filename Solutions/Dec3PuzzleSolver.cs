using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec3PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            long totalJoltage = 0;

            foreach (string line in PuzzleReader.GetPuzzleInput(3, test))
            {
                totalJoltage += FindMaxJotage(line);
            }

            return totalJoltage.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private static long FindMaxJotage(string bank)
        {
            int l = bank.Length;

            long max = Int64.MinValue;

            for (int i = 0; i < l; i++)
            {
                for (int j = i + 1; j < l; j++)
                {
                    long val = Int64.Parse($"{bank[i]}{bank[j]}");
                    if (val > max)
                    {
                        max = val;
                    }
                }
            }

            return max;
        }
    }
}
