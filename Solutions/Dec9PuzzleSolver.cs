using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec9PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            var tiles = new List<(long, long)> ();
            foreach (string line in PuzzleReader.GetPuzzleInput(9, test))
            {
                string[] lineParts = line.Split(',');

                tiles.Add((Int64.Parse(lineParts[0]), Int64.Parse(lineParts[1])));
            }

            long maxArea = Int64.MinValue;

            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = i + 1; j < tiles.Count; j++)
                {
                    long area = Area(tiles[i], tiles[j]);

                    if (area > maxArea)
                    {
                        maxArea = area;
                    }
                }
            }

            return maxArea.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private static long Area((long, long) p1, (long, long) p2)
        {
            if (p1.Item1 == p2.Item1)
            {
                return (long)Math.Abs(p2.Item2 - p1.Item2) + 1;
            }

            if (p1.Item2 == p2.Item2)
            {
                return (long)Math.Abs(p2.Item1 - p1.Item1) + 1;
            }

            return ((long)Math.Abs(p2.Item2 - p1.Item2) + 1) * ((long)Math.Abs(p2.Item1 - p1.Item1) + 1);
        }
    }
}
