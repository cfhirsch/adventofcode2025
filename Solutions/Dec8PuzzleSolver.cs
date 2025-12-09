using System.Data;
using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec8PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            return Solve(test, isPartTwo: false);
        }

        public string SolvePartTwo(bool test)
        {
            return Solve(test, isPartTwo: true);
        }

        private static string Solve(bool test, bool isPartTwo)
        {
            var boxes = new List<Point3D>();

            foreach (string line in PuzzleReader.GetPuzzleInput(8, test))
            {
                string[] lineParts = line.Split(',');

                boxes.Add(
                    new Point3D
                    {
                        X = Int32.Parse(lineParts[0]),
                        Y = Int32.Parse(lineParts[1]),
                        Z = Int32.Parse(lineParts[2])
                    });
            }

            var circuits = new List<List<Point3D>>();

            // Initially each box is in its own circuit.
            foreach (Point3D box in boxes)
            {
                circuits.Add(new List<Point3D>(new[] { box }));
            }

            // Build up list of distances between boxes.
            var distances = new Dictionary<(Point3D, Point3D), double>();

            for (int i = 0; i < boxes.Count; i++)
            {
                for (int j = i + 1; j < boxes.Count; j++)
                {
                    distances.Add((boxes[i], boxes[j]), Distance(boxes[i], boxes[j]));
                }
            }

            distances = distances.OrderBy(k => k.Value).ToDictionary();

            var keyList = distances.Keys.ToList();

            int numConnections = isPartTwo ? keyList.Count : (test ? 10 : 1000);

            var connections = new List<(Point3D, Point3D)>();

            int firstX = Int32.MinValue;
            int secondX = Int32.MinValue;

            for (int i = 0; i < numConnections; i++)
            {
                (Point3D source, Point3D target) = keyList[i];

                List<Point3D> first = circuits.First(c => c.Contains(source));
                List<Point3D> second = circuits.First(c => c.Contains(target));

                if (first != second)
                {
                    first.AddRange(second);
                    circuits.Remove(second);
                }

                if (isPartTwo && circuits.Count == 1)
                {
                    firstX = source.X;
                    secondX = target.X;
                    break;
                }
            }

            long product = 1;
            if (isPartTwo)
            {
                if (firstX < 0 || secondX < 0)
                {
                    throw new ApplicationException("Oops, failed to find the boxes that form the connection.");
                }

                product = (long)(firstX) * secondX;
            }
            else
            {
                IEnumerable<List<Point3D>> topThree = circuits.OrderByDescending(c => c.Count).Take(3);

                foreach (List<Point3D> circuit in topThree)
                {
                    product *= circuit.Count;
                }
            }

            return product.ToString();
        }

        private static double Distance(Point3D source, Point3D target)
        {
            double dist = Math.Pow((double)source.X - (double)target.X, 2);
            dist += Math.Pow((double)source.Y - (double)target.Y, 2);
            dist += Math.Pow((double)source.Z - (double)target.Z, 2);

            return Math.Sqrt(dist);
        }

        private class Point3D
        {
            public int X;
            public int Y;
            public int Z;
        }

        private class CircuitNode : IEquatable<CircuitNode>
        {
            public Point3D Location;

            public CircuitNode Next;

            public bool Equals(CircuitNode? other)
            {
                if (other == null)
                {
                    return false;
                }

                return this.Location == other.Location;
            }
        }

    }
}
