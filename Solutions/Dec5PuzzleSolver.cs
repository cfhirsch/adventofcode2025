using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec5PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            bool gettingRanges = true;
            
            var ranges = new List<(long, long)>();
            var ingredients = new List<long>();

            foreach (string line in PuzzleReader.GetPuzzleInput(5, test))
            {
                if (string.IsNullOrEmpty(line))
                {
                    gettingRanges = false;
                    continue;
                }

                if (gettingRanges)
                {
                    long[] rangeValues = line.Split('-').Select(s => Int64.Parse(s)).ToArray();
                    ranges.Add((rangeValues[0], rangeValues[1]));
                }
                else
                {
                    ingredients.Add(Int64.Parse(line));
                }
            }

            int numFresh = ingredients.Count(i => ranges.Any(r => i >= r.Item1 && i <= r.Item2));
            return numFresh.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }
    }
}
