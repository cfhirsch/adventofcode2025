using System.Net.NetworkInformation;
using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec4PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            char[,] grid = GetGrid(test);

            int numRows = grid.GetLength(0);
            int numCols = grid.GetLength(1);

            int numReachable = 0;

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (grid[i, j] == '@')
                    {
                        int numRolls = 0;
                        foreach ((int x, int y) in GetNeighbors(i, j, numRows, numCols))
                        {
                            if (grid[x, y] == '@')
                            {
                                numRolls++;
                            }
                        }

                        if (numRolls < 4)
                        {
                            numReachable++;
                        }
                    }
                }
            }

            return numReachable.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<(int, int)> GetNeighbors(int i, int j, int numRows, int numCols)
        {
            if (i > 0)
            {
                if (j > 0)
                {
                    yield return (i - 1, j - 1);
                }

                yield return (i - 1, j);

                if (j < numCols - 1)
                {
                    yield return (i - 1, j + 1);
                }
            }

            if (j > 0)
            {
                yield return (i, j - 1);
            }

            if (j < numCols - 1)
            {
                yield return (i, j + 1);
            }

            if (i < numRows - 1)
            {
                if (j > 0)
                {
                    yield return (i + 1, j - 1);
                }

                yield return (i + 1, j);

                if (j < numCols - 1)
                {
                    yield return (i + 1, j + 1);
                }
            }
            
        }

        private static char[,] GetGrid(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(4, test).ToList();

            var grid = new char[lines.Count, lines[0].Length];

            for (int i = 0; i <  lines.Count; i++)
            {
                for (int j = 0;  j < lines[i].Length; j++)
                {
                    grid[i, j] = lines[i][j];
                }
            }

            return grid;
        }
    }
}
