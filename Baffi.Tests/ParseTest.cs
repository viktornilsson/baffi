using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Baffi.Tests
{
    [TestClass]
    public class ParseTest
    {
        [TestMethod]
        public void ExtractHtmlTemplateToJsonObject()
        {
            var text = System.IO.File.ReadAllText("content/template.html");

            var obj = Extract.GetObject(text);

            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);           

            Assert.IsTrue(obj != null);
            Assert.IsTrue(!string.IsNullOrEmpty(json));
        }

        [TestMethod]
        public void ParseHtmlTemplateAndJsonObject()
        {
            var text = System.IO.File.ReadAllText("content/template.html");
            var jsonData = System.IO.File.ReadAllText("content/data.json");

            dynamic parsedObject = JsonConvert.DeserializeObject(jsonData);

            string template = Parse.GetTemplate(text, parsedObject);

            Assert.IsTrue(parsedObject != null);
            Assert.IsTrue(!string.IsNullOrEmpty(template));
        }
    }
}
