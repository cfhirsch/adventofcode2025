using System;
using System.Reflection.Metadata;
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
            var ranges = new List<(long, long)>();
            var ingredients = new List<long>();

            foreach (string line in PuzzleReader.GetPuzzleInput(5, test))
            {
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                long[] rangeValues = line.Split('-').Select(s => Int64.Parse(s)).ToArray();
                ranges.Add((rangeValues[0], rangeValues[1]));
            }

            List<(long, long)> merged = Merge(ranges);
            long sum = merged.Sum(r => r.Item2 - r.Item1 + 1);
            return sum.ToString();
        }

        // Taken from https://aoc.csokavar.hu/2025/5/.
        private static List<(long, long)> Merge(List<(long, long)> ranges)
        {
            ranges = ranges.OrderBy(x => x.Item1).ToList();
            for (var i = 0; i < ranges.Count - 1; i++)
            {
                if (ranges[i + 1].Item1 <= ranges[i].Item2)
                {
                    var end = Math.Max(ranges[i].Item2, ranges[i + 1].Item2);
                    ranges[i] = (ranges[i].Item1, ranges[i + 1].Item1 - 1);
                    ranges[i + 1] = (ranges[i + 1].Item1, end);
                }
            }

            return ranges;
        }
    }
}
