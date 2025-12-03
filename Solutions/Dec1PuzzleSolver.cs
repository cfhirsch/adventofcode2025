using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec1PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            int dial = 50;
            int numZeroes = 0;

            foreach (string line in PuzzleReader.GetPuzzleInput(1, test))
            {
                char dir = line[0];
                int rotations = Int32.Parse(line.Substring(1));

                if (dir == 'L')
                {
                    dial -= rotations;
                    while (dial < 0)
                    {
                        dial += 100;
                    }
                }
                else
                {
                    dial = (dial + rotations) % 100;
                }

                if (dial == 0)
                {
                    numZeroes++;
                }
            }

            return numZeroes.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }
    }
}
