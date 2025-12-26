using System.Security;
using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec11PuzzlerSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
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
            var devices = new Dictionary<string, List<string>>();
            foreach (string line in PuzzleReader.GetPuzzleInput(11, test, partTwoTestDifferent: true))
            {
                string[] devParts = line.Split(':');
                string label = devParts[0];
                devices[label] = new List<string>();

                foreach (string output in devParts[1].Trim().Split(' '))
                {
                    devices[label].Add(output);
                }
            }

            var memozied = new Dictionary<(string, string), long>();

            long dac_fft = NumPaths(devices, "dac", "fft", memozied);

            long fft_dac = NumPaths(devices, "fft", "dac", memozied);
            

            long svr_fft = NumPaths(devices, "svr", "fft", memozied);
            long dac_out = NumPaths(devices, "dac", "out", memozied);

            long svr_dac = NumPaths(devices, "svr", "dac", memozied);
            long fft_out = NumPaths(devices, "fft", "out", memozied);

            long numPaths = svr_dac * dac_fft * fft_out + svr_fft * fft_dac * dac_out;
            return numPaths.ToString();
        }

        private static long NumPaths(
            Dictionary<string, List<string>> devices, 
            string start, 
            string end,
            Dictionary<(string, string), long> memozied)
        {
            if (start == "out")
            {
                return (start == end) ? 1 : 0;
            }

            if (start == end)
            {
                return 1;
            }

            if (memozied.ContainsKey((start, end)))
            {
                return memozied[(start, end)];
            }

            long sum = 0;
            foreach (string output in devices[start])
            {
                sum += NumPaths(devices, output, end, memozied);
            }

            memozied[(start, end)] = sum;
            return sum;
        }
    }
}
