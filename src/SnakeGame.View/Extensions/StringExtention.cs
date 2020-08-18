namespace SnakeGame.View.Extensions
{
    internal static class StringExtension
    {
        public static string SubstringIfPossible(this string line, int startIndex, int length)
        {
            var result = line;

            if (line.Length > length)
            {
                result = result.Substring(startIndex, length);
            }

            return result;
        }
    }
}
