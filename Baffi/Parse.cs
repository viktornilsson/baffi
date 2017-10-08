using System.Collections;
using Baffi.Helpers;

namespace Baffi
{
    public static class Parse
    {
        /// <summary>
        /// Parse HTML-template and JSON object to translated HTML.
        /// </summary>
        /// <param name="template">HTML template</param>
        /// <param name="obj">JSON object</param>
        /// <returns>Parsed HTML</returns>
        public static string GetTemplate(string template, dynamic obj)
        {
            foreach (var entry in obj)
            {
                string name = entry.Name;

                if (entry.Value is ICollection)
                {
                    template = ParseCollection(template, entry);
                }
                else
                {
                    template = template.Replace(name.ParseTag(), entry.Value.ToString());
                }
            }

            return template;
        }

        #region Private methods
      
        private static string ParseCollection(string template, dynamic entry)
        {
            var forTag = $"for {entry.Name}";
            var endTag = $"end {entry.Name}";

            var fullForTag = forTag.ParseTag();
            var fullEndTag = endTag.ParseTag();

            var fullChildText = TagHelper.BetweenTagText(template, fullForTag, fullEndTag);
            var childText = TagHelper.Between(template, fullForTag, fullEndTag);

            var newChildText = string.Empty;

            foreach (var listItem in entry.Value)
            {
                var listText = childText;

                foreach (var childItem in listItem)
                {
                    string name = childItem.Name;

                    if (childItem.Value is ICollection)
                    {
                        listText = ParseCollection(listText, childItem);
                    }
                    else
                    {
                        listText = listText.Replace(name.ParseTag(), childItem.Value.ToString());
                    }
                }

                newChildText += listText;
            }

            return template.Replace(fullChildText, newChildText);
        }

        #endregion
    }
}