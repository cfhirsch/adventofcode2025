using System.ComponentModel;
using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec2PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            long invalidIdCount = 0;

            string line = PuzzleReader.GetPuzzleInput(2, test).First();

            string[] lineParts = line.Split(',');

            // 32055-104187
            // Skip 32055, so we start with 100000 (10^6)
            // 
            foreach (string part in lineParts)
            {
                string[] partParts = part.Split('-');

                string lower = partParts[0];
                string upper = partParts[1];

                long lowerVal = Int64.Parse(lower);
                long upperVal = Int64.Parse(upper);

                var visited = new HashSet<long>();

                invalidIdCount += FindSumInvalid(lowerVal, upperVal, 2, visited);
            }

            return invalidIdCount.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            long invalidIdCount = 0;

            string line = PuzzleReader.GetPuzzleInput(2, test).First();

            string[] lineParts = line.Split(',');

            // 32055-104187
            // Skip 32055, so we start with 100000 (10^6)
            // 
            foreach (string part in lineParts)
            {
                string[] partParts = part.Split('-');

                string lower = partParts[0];
                string upper = partParts[1];

                long lowerVal = Int64.Parse(lower);
                long upperVal = Int64.Parse(upper);

                int maxLength = (int)Math.Max(lower.Length, upper.Length);

                var visited = new HashSet<long>();
                for (int n = 2; n <= maxLength; n++)
                {
                    invalidIdCount += FindSumInvalid(lowerVal, upperVal, n, visited);
                }
            }

            return invalidIdCount.ToString();
        }

        private static long FindSumInvalid(long lower, long upper, int n, HashSet<long> visited)
        {
            long sum = 0;
            string lowerStr = lower.ToString();
            string upperStr = upper.ToString();

            int lowerLength = lowerStr.Length;
            int upperLength = upperStr.Length;

            /*if (n == 1)
            {
                char lowerCh = lowerStr[0];
                char upperCh = upperStr[0];

                long lowerId = Int64.Parse(new String(lowerCh, lowerStr.Length));

                if (!visited.Contains(lowerId) && lowerId >= lower && lowerId <= upper)
                {
                    sum += lowerId;
                    visited.Add(lowerId);
                }

                long upperId = Int64.Parse(new String(upperCh, upperStr.Length));

                if (!visited.Contains(upperId) && upperId >= lower && upperId <= upper)
                {
                    sum += upperId;
                    visited.Add(upperId);
                }
            }
            else
            {*/
                bool lowerLengthDivisible = (lowerStr.Length % n == 0);
                bool upperLengthDivisible = (upperStr.Length % n == 0);

                if (!lowerLengthDivisible && !upperLengthDivisible)
                {
                    return 0;
                }

                long min, max;
                if (lowerLengthDivisible && upperLengthDivisible)
                {
                    min = Int64.Parse(lowerStr.Substring(0, lowerStr.Length / n));
                    max = Int64.Parse(upperStr.Substring(0, upperStr.Length / n));
                }
                else if (lowerLengthDivisible)
                {
                    min = Int64.Parse(lowerStr.Substring(0, lowerStr.Length / n));
                    max = (long)Math.Pow(10, lowerStr.Length / n);
                }
                else
                {
                    min = (long)Math.Pow(10, (upperStr.Length / n) - 1);
                    max = Int64.Parse(upperStr.Substring(0, upperStr.Length / n));
                }

                for (long l = min; l <= max; l++)
                {
                    string lStr = l.ToString();
                    long val = Int64.Parse(String.Concat(Enumerable.Repeat(lStr, n)));
                    
                    if (visited.Contains(val))
                    {
                        continue;
                    }

                    if (val > upper)
                    {
                        break;
                    }

                    if (val < lower)
                    {
                        continue;
                    }

                    sum += val;
                    visited.Add(val);
                }
            //}

            return sum;
        }
    }
}
