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

                bool lowerLengthEven = (lower.Length % 2 == 0);
                bool upperLengthEven = (upper.Length % 2 == 0);

                if (!lowerLengthEven && !upperLengthEven)
                {
                    continue;
                }

                long min, max;
                if (lowerLengthEven && upperLengthEven)
                {
                    min = Int64.Parse(lower.Substring(0, lower.Length / 2));
                    max = Int64.Parse(upper.Substring(0, upper.Length / 2));
                }
                else if (lowerLengthEven)
                {
                    min = Int64.Parse(lower.Substring(0, lower.Length / 2));
                    max = (long)Math.Pow(10, lower.Length / 2);
                }
                else
                {
                    min = (long)Math.Pow(10, (upper.Length / 2) - 1);
                    max = Int64.Parse(upper.Substring(0, upper.Length / 2));
                }

                for (long l = min; l <= max; l++)
                {
                    string lStr = l.ToString();
                    long val = Int64.Parse(lStr + lStr);
                    if (val > upperVal)
                    {
                        break;
                    }

                    if (val < lowerVal)
                    {
                        continue;
                    }

                    invalidIdCount += val;
                }
            }   

            return invalidIdCount.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            throw new NotImplementedException();
        }

        private static long NumInvalid(long lower, long upper)
        {
            return upper - lower; 
        }
    }
}
