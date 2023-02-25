namespace BandcampRenameV2.Extensions
{
    public static class StringExtensions
    {
        public static string Replace(this string source, char[] charsToReplace, char charToReplaceWith)
        {
            var sourceArray = source.Split(charsToReplace, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(charToReplaceWith, sourceArray);
        }
    }
}
