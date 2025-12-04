using System.ComponentModel.Design.Serialization;
using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec1PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            int dial = 50;
            int numZeroes = 0;

            foreach (string line in PuzzleReader.GetPuzzleInput(1, test))
            {
                char dir = line[0];
                int rotations = Int32.Parse(line.Substring(1));

                if (dir == 'L')
                {
                    dial -= rotations;
                    while (dial < 0)
                    {
                        dial += 100;
                    }
                }
                else
                {
                    dial = (dial + rotations) % 100;
                }

                if (dial == 0)
                {
                    numZeroes++;
                }
            }

            return numZeroes.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            int dial = 50;
            int numZeroes = 0;
            int newDial = 50;

            foreach (string line in PuzzleReader.GetPuzzleInput(1, test))
            {
                char dir = line[0];
                int rotations = Int32.Parse(line.Substring(1));

                // Each rotation of 100 clicks passes zero.
                numZeroes += rotations / 100;
                rotations = rotations % 100;

                if (dir == 'L')
                {
                    newDial = dial - rotations;
                    
                    // Do we cross zero in the final set of clicks?
                    if (newDial < 0)
                    {
                        newDial += 100;

                        if (dial != 0 && newDial != 0)
                        {
                            numZeroes++;
                        }
                    }
                }
                else
                {
                    newDial = dial + rotations;
                    if (newDial >= 100)
                    {
                        newDial -= 100;

                        if (dial != 0 && newDial != 0)
                        {
                            numZeroes++;
                        }
                    }
                }

                if (newDial == 0)
                {
                    numZeroes++;
                }

                dial = newDial;
            }

            return numZeroes.ToString();
        }
    }
}
