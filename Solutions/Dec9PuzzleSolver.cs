using Adventofcode2025.Utilities;
using Geometry;

namespace AdventOfCode2025.Solutions
{
    internal class Dec9PuzzleSolver : IPuzzleSolver
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
            var tiles = new List<(long, long)>();
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

                    List<(long, long)> corners = GetRectangleCorners(tiles[i], tiles[j]);
                    
                    if (area > maxArea && (!isPartTwo || IsRectangleInsidePolygon(tiles, corners)))
                    {
                        maxArea = area;
                    }
                }
            }

            return maxArea.ToString();
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

        private static List<(long, long)> GetRectangleCorners((long, long) c1, (long, long) c2)
        {
            var corners = new List<(long, long)>();

            if (c1.Item1 == c2.Item1 || c1.Item2 == c2.Item2)
            {
                corners.Add(c1);
                corners.Add(c2);
                corners.Add(c1);
                corners.Add(c2);
            }
            else
            {
                long minX = Math.Min(c1.Item1, c2.Item1);
                long maxX = Math.Max(c1.Item1, c2.Item1);
                long minY = Math.Min(c1.Item2, c2.Item2);
                long maxY = Math.Max(c1.Item2, c2.Item2);

                corners.Add((minX, minY));
                corners.Add((minX, maxY));
                corners.Add((maxX, minY));
                corners.Add((maxX, maxY));
            }

            return corners;
        }

        private static bool IsRectangleInsidePolygon(List<(long, long)> polygon, List<(long, long)> rectangleCorners)
        {
            // 1) Order rectangle corners cyclically so edges are correct even if input order is arbitrary.
            var rect = OrderCornersCyclic(rectangleCorners);

            // 2) Every corner must be inside or on the polygon boundary.
            foreach (var c in rect)
            {
                if (!PointInPolygonNonStrict(c, polygon))
                    return false;
            }

            // 3) No rectangle edge may properly (non-colinearly) intersect the polygon boundary.
            for (int i = 0; i < rect.Count; i++)
            {
                var a = rect[i];
                var b = rect[(i + 1) % rect.Count];
                if (SegmentProperlyIntersectsPolygon(a, b, polygon))
                    return false;
            }

            return true;
        }

        private static List<(long, long)> OrderCornersCyclic(List<(long, long)> corners)
        {
            double cx = corners.Average(p => (double)p.Item1);
            double cy = corners.Average(p => (double)p.Item2);

            return corners
                .OrderBy(p => Math.Atan2(p.Item2 - cy, p.Item1 - cx)) // angle around centroid
                .ToList();
        }

        private static bool PointInPolygonNonStrict((long, long) p, List<(long, long)> poly)
        {
            // Boundary check first (inclusive)
            for (int i = 0; i < poly.Count; i++)
            {
                var a = poly[i];
                var b = poly[(i + 1) % poly.Count];
                if (OnSegment(a, b, p))
                    return true;
            }

            bool inside = false;
            for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            {
                var a = poly[j];
                var b = poly[i];

                // Consider edges that straddle p.Item2 (half-open: [minY, maxY))
                bool cond = (a.Item2 <= p.Item2 && p.Item2 < b.Item2) || (b.Item2 <= p.Item2 && p.Item2 < a.Item2);
                if (cond)
                {
                    // If p is left of the upward edge (for upward (long, long)ing) toggle; vice versa for downward
                    long orient = Cross(a, b, p);
                    if ((a.Item2 <= p.Item2 && p.Item2 < b.Item2 && orient > 0) ||
                        (b.Item2 <= p.Item2 && p.Item2 < a.Item2 && orient < 0))
                    {
                        inside = !inside;
                    }
                }
            }
            return inside;
        }

        private static bool OnSegment((long, long) a, (long, long) b, (long, long) p)
        {
            if (Orient(a, b, p) != 0) return false;
            return Math.Min(a.Item1, b.Item1) <= p.Item1 && p.Item1 <= Math.Max(a.Item1, b.Item1)
                && Math.Min(a.Item2, b.Item2) <= p.Item2 && p.Item2 <= Math.Max(a.Item2, b.Item2);
        }

        private static int Orient((long, long) a, (long, long) b, (long, long) c)
        {
            long v = Cross(a, b, c);
            if (v > 0) return 1;
            if (v < 0) return -1;
            return 0;
        }

        private static long Cross((long, long) a, (long, long) b, (long, long) c)
            => (b.Item1 - a.Item1) * (c.Item2 - a.Item2) - (b.Item2 - a.Item2) * (c.Item1 - a.Item1);

        private static bool SegmentProperlyIntersectsPolygon((long, long) a, (long, long) b, IReadOnlyList<(long, long)> poly)
        {
            for (int i = 0; i < poly.Count; i++)
            {
                var c = poly[i];
                var d = poly[(i + 1) % poly.Count];
                if (SegmentsProperlyIntersect(a, b, c, d))
                    return true;
            }
            return false;
        }

        private static bool SegmentsProperlyIntersect((long, long) a, (long, long) b, (long, long) c, (long, long) d)
        {
            int o1 = Orient(a, b, c);
            int o2 = Orient(a, b, d);
            int o3 = Orient(c, d, a);
            int o4 = Orient(c, d, b);

            // Endpoint touches or colinear overlaps are allowed (non-strict containment),
            // so if any orientation is zero and the corresponding point lies on segment, treat as NOT proper.
            if (o1 == 0 && OnSegment(a, b, c)) return false;
            if (o2 == 0 && OnSegment(a, b, d)) return false;
            if (o3 == 0 && OnSegment(c, d, a)) return false;
            if (o4 == 0 && OnSegment(c, d, b)) return false;

            // Proper intersection if signs strictly differ on both segment pairs
            return (o1 * o2 < 0) && (o3 * o4 < 0);
        }
    }
}
