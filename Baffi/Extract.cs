using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using Baffi.Helpers;

namespace Baffi
{
    public static class Extract
    {
        /// <summary>
        /// Extract JSON object from HTML template.
        /// </summary>
        /// <param name="text">HTML template</param>
        /// <returns>JSON object</returns>
        public static object GetObject(string text)
        {
            var obj = new ExpandoObject() as IDictionary<string, object>;

            while (true)
            {
                var props = ExtractProps(text);
                var tag = props.FirstOrDefault(x => x.StartsWith("for", StringComparison.CurrentCultureIgnoreCase));

                if (string.IsNullOrEmpty(tag))
                    break; 

                var tagName = tag.Replace("for", string.Empty).Trim();

                string newText;
                var childObjects = GetChildObjects(tagName, text, out newText);

                text = newText;

                obj.Add(tagName, childObjects);                       
            }
            
            var parents = ExtractProps(text);

            foreach (var propName in parents)
            {
                obj.Add(propName, string.Empty);
            }

            return obj;
        }

        #region Private methods

        private static List<object> GetChildObjects(string name, string text, out string newText)
        {
            var forTag = $"for {name}";
            var endTag = $"end {name}";

            var fullForTag = forTag.ParseTag();
            var fullEndTag = endTag.ParseTag();

            var childText = TagHelper.Between(text, fullForTag, fullEndTag);

            var obj = GetObject(childText);

            var removeText = TagHelper.BetweenTagText(text, fullForTag, fullEndTag);
            newText = text.Replace(removeText, string.Empty);

            return new List<object> {(ExpandoObject)obj };
        }

        private static IEnumerable<string> ExtractProps(string text)
        {
            var regex = new Regex(@"(?<=\{\{).*?(?=}})");
            var matchCollection = regex.Matches(text);
            return matchCollection.Cast<Match>().Select(match => match.Value).ToList();
        }

        #endregion
    }
}
