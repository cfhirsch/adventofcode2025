using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec7PuzzleSolver : IPuzzleSolver
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

            var queue = new Queue<Node>();

            var startNode = new Node { Location = startPos, Children = new List<Node>() };
            queue.Enqueue(startNode);

            var visited = new HashSet<Node>();

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                (int x, int y) = current.Location;

                (int, int) next = (x + 1, y);

                if (next.Item1 < numRows)
                {
                    if (splitters.Contains(next))
                    {
                        AddChild((x + 1, y - 1), visited, queue, current);
                        AddChild((x + 1, y + 1), visited, queue, current);

                        splitCount++;
                    }
                    else
                    {
                        AddChild((x + 1, y), visited, queue, current);
                    }
                }
            }

            if (isPartTwo)
            {
               var memoized = new Dictionary<Node, long>();
               long numPaths = FindPaths(startNode, numRows - 1, memoized);

               return numPaths.ToString();
            }

            return splitCount.ToString();
        }

        private static void AddChild((int, int) nextLoc, HashSet<Node> visited, Queue<Node> queue, Node current)
        {
            Node nextNode = visited.FirstOrDefault(n => n.Location == nextLoc);
            if (nextNode == null)
            {
                nextNode = new Node { Location = nextLoc, Children = new List<Node>() };
                queue.Enqueue(nextNode);
                visited.Add(nextNode);
            }

            current.Children.Add(nextNode);
        }

        private static long FindPaths(Node node, int maxX, Dictionary<Node, long> memoized)
        {
            if (node.Location.Item1 == maxX)
            {
                return 1;
            }

            if (memoized.ContainsKey(node))
            {
                return memoized[node];
            }

            long sum = 0;
            foreach (Node child in node.Children)
            {
                sum += FindPaths(child, maxX, memoized);
            }

            memoized[node] = sum;

            return sum;    
        }
        
        private class Node : IEquatable<Node>
        {
            public (int, int) Location;

            public List<Node> Children;

            public bool Equals(Node? other)
            {
                if (other == null)
                {
                    return false;
                }

                return (this.Location == other.Location);
            }
        }
    }
}
