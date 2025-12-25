using System.Linq;
using System.Text;
using Adventofcode2025.Utilities;
using Google.OrTools.ConstraintSolver;

namespace AdventOfCode2025.Solutions
{
    internal class Dec12PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            var presents = new Dictionary<int, HashSet<Point>>();
            var trees = new List<Tree>();
            bool presentMode = true;
            List<string> lines = PuzzleReader.GetPuzzleInput(12, test).ToList();
            int i = 0;
            while (i < lines.Count)
            {
                string line = lines[i];
                if (string.IsNullOrEmpty(line))
                {
                    i++;
                    continue;
                }

                if (line.Contains("x"))
                {
                    presentMode = false;
                }

                if (presentMode)
                {
                    var present = new HashSet<Point>();
                    int id = Int32.Parse(line.Substring(0, line.IndexOf(":")));
                    i++;
                    line = lines[i];
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            if (line[y] == '#')
                            {
                                present.Add(new Point { X = x, Y = y });
                            }
                        }

                        i++;
                        line = lines[i];
                    }

                    presents.Add(id, present);
                }
                else
                {
                    string[] lineParts = line.Split(':');
                    string[] dimParts = lineParts[0].Split("x");

                    var dims = new Point { X = Int32.Parse(dimParts[0]), Y = Int32.Parse(dimParts[1]) };
                    int[] quantities = lineParts[1].Trim().Split(" ").Select(s => Int32.Parse(s)).ToArray();

                    trees.Add(new Tree { Dimensions = dims, Quantities = quantities });
                    i++;
                }
            }

            int canFit = 0;
            var memoized = new Dictionary<string, bool>();
            foreach (Tree tree in trees)
            {
                double area = (tree.Dimensions.X / 3.0) * (tree.Dimensions.Y / 3.0);
                int numPresents = tree.Quantities.Sum();

                if (numPresents <= area)
                {
                    canFit++;
                }
            }

            return canFit.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            return "Merry Christmas!";
        }

        private struct Point
        {
            public int X; public int Y; 
        }

        private class Tree
        {
            public Point Dimensions { get; set; }

            public int[] Quantities { get; set; }
        }
    }
}
