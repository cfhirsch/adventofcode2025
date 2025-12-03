using AdventOfCode2025.Solutions;

namespace Adventofcode2025.Utilities
{
    internal static class PuzzleSolverFactory
    {
        public static IPuzzleSolver GetPuzzleSolver(int day)
        {
            switch (day)
            {
                case 1:
                    return new Dec1PuzzleSolver();

                default:
                    throw new ArgumentOutOfRangeException("day");
            }
        }
    }
}