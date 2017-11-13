using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Baffi.Helpers
{
    internal class ExtractHelper
    {
        internal static List<object> GetChildObjects(string name, string text, out string newText)
        {
            var forTag = $"for {name}";
            var endTag = $"end {name}";

            var fullForTag = forTag.ParseTag();
            var fullEndTag = endTag.ParseTag();

            var childText = TagHelper.Between(text, fullForTag, fullEndTag);

            var obj = Parser.Extract(childText);

            var removeText = TagHelper.BetweenTagText(text, fullForTag, fullEndTag);
            newText = text.Replace(removeText, string.Empty);

            return new List<object> { (ExpandoObject)obj };
        }

        internal static IEnumerable<string> ExtractProps(string text)
        {
            var regex = new Regex(@"(?<=\{\{).*?(?=}})");
            var matchCollection = regex.Matches(text);
            return matchCollection.Cast<Match>().Select(match => match.Value).ToList();
        }
    }
}
