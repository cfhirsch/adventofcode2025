namespace Adventofcode2025.Utilities
{
    internal static class PuzzleReader
    {
        public static IEnumerable<string> GetPuzzleInput(int day, bool test, bool partTwoTestDifferent = false)
        {
            string testString = test ? "test" : string.Empty;
            string part2TestSuffix = (test && partTwoTestDifferent) ? "2" : string.Empty;
            string filePath = $"c:\\AdventOfCode\\2025\\Dec{day}{testString}{part2TestSuffix}.txt";
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    yield return reader.ReadLine();
                }
            }
        }
    }
}