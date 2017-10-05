using System;

namespace Baffi.Helpers
{
    public static class TagHelper
    {
        public static string Between(string text, string firstString, string lastString)
        {
            var pos1 = text.IndexOf(firstString, StringComparison.Ordinal) + firstString.Length;
            var pos2 = text.IndexOf(lastString, StringComparison.Ordinal);
            var finalString = text.Substring(pos1, pos2 - pos1);

            return finalString;
        }

        public static string BetweenTagText(string text, string firstString, string lastString)
        {
            var pos1 = text.IndexOf(firstString, StringComparison.Ordinal);
            var pos2 = text.IndexOf(lastString, StringComparison.Ordinal) + lastString.Length;
            var finalString = text.Substring(pos1, pos2 - pos1);

            return finalString;
        }

        public static string ParseTag(this string text)
        {
            return $@"{{{{{text}}}}}";
        }
    }
}
