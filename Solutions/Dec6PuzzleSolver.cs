using System.Net;
using System.Text;
using Adventofcode2025.Utilities;

namespace AdventOfCode2025.Solutions
{
    internal class Dec6PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test)
        {
            var numbers = new List<List<long>>();
            var ops = new List<string>();

            var lines = PuzzleReader.GetPuzzleInput(6, test).ToList();

            for (int i = 0; i < lines.Count - 1; i++)
            {
                numbers.Add(lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => Int64.Parse(s)).ToList());
            }

            ops = lines[lines.Count - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            long sum = 0;

            for (int j = 0; j < ops.Count; j++)
            {
                string op = ops[j];

                long temp = (op == "*") ? 1 : 0;

                for (int i = 0; i < numbers.Count; i++)
                {
                    if (op == "*")
                    {
                        temp *= numbers[i][j];
                    }
                    else
                    {
                        temp += numbers[i][j];
                    }
                }

                sum += temp;
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test)
        {
            long sum = 0;

            char[,] homework = GetGrid(test);

            int rows = homework.GetLength(0);
            int cols = homework.GetLength(1);

            int j = cols - 1;

            while (j >= 0)
            {
                // Find the index of the operator in the left most column.
                int opJ = j;
                while (homework[rows - 1, opJ] != '*' &&
                       homework[rows - 1, opJ] != '+')
                {
                    opJ--;
                }

                char op = homework[rows - 1, opJ];
                long temp = (op == '*') ? 1 : 0;

                for (int k = j; k >= opJ; k--)
                {
                    // Get the number in this column.
                    var sb = new StringBuilder();
                    for (int i = 0; i < rows - 1; i++)
                    {
                        if (char.IsAsciiDigit(homework[i, k]))
                        {
                            sb.Append(homework[i, k]);
                        }
                    }

                    long num = Int64.Parse(sb.ToString());

                    if (op == '*')
                    {
                        temp *= num;
                    }
                    else 
                    {
                        temp += num;
                    }
                }

                sum += temp;
                j = opJ - 2;
            }

            return sum.ToString();
        }

        private static char[,] GetGrid(bool test)
        {
            var lines = PuzzleReader.GetPuzzleInput(6, test).ToList();

            var grid = new char[lines.Count, lines[0].Length];

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    grid[i, j] = lines[i][j];
                }
            }

            return grid;
        }
    }
}
