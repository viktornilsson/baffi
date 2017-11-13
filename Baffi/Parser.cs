using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Baffi.Configuration;
using Baffi.Helpers;

namespace Baffi
{
    public static class Parser
    {
        private static List<Type> _customTypes = new List<Type>();
        private static List<string> _customTags =  new List<string>();

        /// <summary>
        /// Initialize parser with custom tags.
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public static void Initialize(Action<IBaffiConfigurationExpression> configuration)
        {
            var expression = new BaffiConfigurationExpression();
            configuration(expression);

            _customTypes = expression.CustomTypes;
            _customTags = _customTypes.Select(x => x.Name.ToLower()).ToList();
        }

        /// <summary>
        /// Extract JSON object from HTML template.
        /// </summary>
        /// <param name="text">HTML template</param>
        /// <returns>JSON object</returns>
        public static object Extract(string text)
        {
            var obj = new ExpandoObject() as IDictionary<string, object>;

            while (true)
            {
                var props = ExtractHelper.ExtractProps(text);
                var tag = props.FirstOrDefault(x => x.StartsWith("for", StringComparison.CurrentCultureIgnoreCase));

                if (string.IsNullOrEmpty(tag))
                    break;

                var tagName = tag.Replace("for", string.Empty).Trim();

                string newText;
                var childObjects = ExtractHelper.GetChildObjects(tagName, text, out newText);

                text = newText;

                obj.Add(tagName, childObjects);
            }

            var parents = ExtractHelper.ExtractProps(text);

            foreach (var propName in parents)
            {
                obj.Add(propName, string.Empty);
            }

            return obj;
        }

        /// <summary>
        /// Parse HTML-template and JSON object to translated HTML.
        /// </summary>
        /// <param name="template">HTML template</param>
        /// <param name="obj">JSON object</param>
        /// <returns>Parsed HTML</returns>
        public static string Compile(string template, dynamic obj)
        {
            foreach (var entry in obj)
            {
                CompileProperty(entry, ref template);
            }

            return template;
        }

        #region Private methods

        private static void CompileProperty(dynamic property, ref string template)
        {
            string name = property.Name;

            if (property.Value is ICollection)
            {
                template = CompileCollection(template, property);
            }
            else
            {
                string value = property.Value.ToString();
                var newValue = _customTags.Contains(name.ToLower()) ? ProcessCustomTagValue(name, value) : value;

                template = template.Replace(name.ParseTag(), newValue);
            }
        }

        private static string CompileCollection(string template, dynamic entry)
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
                    CompileProperty(childItem, ref listText);
                }

                newChildText += listText;
            }

            return template.Replace(fullChildText, newChildText);
        }

        private static string ProcessCustomTagValue(string tagName, string value)
        {
            var type = _customTypes.FirstOrDefault(x => string.Equals(x.Name, tagName, StringComparison.CurrentCultureIgnoreCase));
            var customTag = (ICustomTag)Activator.CreateInstance(type);
            var parameters = CustomTagHelper.GetTagParameters(value);

            return customTag.Process(parameters);
        }

        #endregion
    }
}