using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec7PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            (int, int) startPos = (0, 0);
            var splitters = new HashSet<(int, int)>();
            int splitCount = 0;

            var lines = PuzzleReader.GetPuzzleInput(7, test).ToList();

            int numRows = lines.Count;
            int numCols = lines[0].Length;

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (lines[i][j] == 'S')
                    {
                        startPos = (i, j);
                    }

                    if (lines[i][j] == '^')
                    {
                        splitters.Add((i, j));
                    }
                }
            }

            var queue = new Queue<(int, int)>();
            queue.Enqueue(startPos);

            var visited = new HashSet<(int, int)>();

            int currX = 0;

            if (test)
            {
                Print(startPos, splitters, visited, numRows, numCols);
            }

            while (queue.Count > 0)
            {
                (int x, int y) = queue.Dequeue();

                (int, int) next = (x + 1, y);

                if (next.Item1 < numRows)
                {
                    if (splitters.Contains(next))
                    {
                        if (!visited.Contains((x + 1, y - 1)))
                        {
                            queue.Enqueue((x + 1, y - 1));
                            visited.Add((x + 1, y - 1));
                        }

                        if (!visited.Contains((x + 1, y + 1)))
                        {
                            queue.Enqueue((x + 1, y + 1));
                            visited.Add((x + 1, y + 1));
                        }

                        splitCount++;
                    }
                    else
                    {
                        if (!visited.Contains((x + 1, y)))
                        {
                            queue.Enqueue((x + 1, y));
                            visited.Add((x + 1, y));
                        }
                    }
                }

                //Print(startPos, splitters, visited, numRows, numCols);
            }

            return splitCount.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private static void Print(
            (int, int) startPos, 
            HashSet<(int, int)> splitters, 
            HashSet<(int, int)> visited,
            int numRows,
            int numCols)
        {
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (startPos == (i, j))
                    {
                        Console.Write('S');
                    }
                    else if (splitters.Contains((i, j)))
                    {
                        Console.Write('^');
                    }
                    else if (visited.Contains((i, j)))
                    {
                        Console.Write('|');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
