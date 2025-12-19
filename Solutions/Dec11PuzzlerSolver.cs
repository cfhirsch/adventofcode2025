using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec11PuzzlerSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            // aaa: you hhh
            var devices = new Dictionary<string, List<string>>();
            foreach (string line in PuzzleReader.GetPuzzleInput(11, test))
            {
                string[] devParts = line.Split(':');
                string label = devParts[0];
                devices[label] = new List<string>();

                foreach (string output in devParts[1].Trim().Split(' '))
                {
                    devices[label].Add(output);
                }
            }

            var queue = new Queue<string>();
           
            queue.Enqueue("you");
            int numPaths = 0;
            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                if (current == "out")
                {
                    numPaths++;
                    continue;
                }

                if (devices.ContainsKey(current))
                {
                    foreach (string output in devices[current])
                    {
                        queue.Enqueue(output);
                    }
                }
            }

            return numPaths.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private class Device
        {

        }
    }
}
