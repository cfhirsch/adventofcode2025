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

            // Let N(s,t,l) = number of paths from s to t that do not include any devices in l.
            // N(s, s, *) = 1
            // N(s, t, l) = sum(o in outputs(s), N(o, t, l))

            var memozied = new Dictionary<(string, string, string[]), long>();

            long dac_fft = NumPaths(devices, "dac", "fft", memozied, "svr");

            long fft_dac = NumPaths(devices, "fft", "dac", memozied, "svr");
            

            long svr_fft = NumPaths(devices, "svr", "fft", memozied, "dac");
            long dac_out = NumPaths(devices, "dac", "out", memozied, "svr", "fft");

            long svr_dac = NumPaths(devices, "svr", "dac", memozied, "fft");
            long fft_out = NumPaths(devices, "fft", "out", memozied, "svr", "dac");

            long numPaths = svr_dac * dac_fft * fft_out + svr_fft * fft_dac * dac_out;
            return numPaths.ToString();
        }

        private static long NumPaths(
            Dictionary<string, List<string>> devices, 
            string start, 
            string end,
            Dictionary<(string, string, string[]), long> memozied,
            params string[] except)
        {
            if (start == "out")
            {
                return (start == end) ? 1 : 0;
            }

            if (start == end)
            {
                return 1;
            }

            if (memozied.ContainsKey((start, end, except)))
            {
                return memozied[(start, end, except)];
            }

            long sum = 0;
            foreach (string output in devices[start])
            {
                sum += NumPaths(devices, output, end, memozied, except);
            }

            memozied[(start, end, except)] = sum;
            return sum;
        }
    }
}
