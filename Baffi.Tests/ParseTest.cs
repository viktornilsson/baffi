using Baffi.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Baffi.Tests
{
    [TestClass]
    public class ParseTest
    {
        public ParseTest()
        {
            Parser.Initialize(cfg =>
            {
                cfg.AddTag<PriceTag>();
            });
        }

        [TestMethod]
        public void ExtractHtmlTemplateToJsonObject()
        {
            var text = System.IO.File.ReadAllText("content/template.html");

            var obj = Parser.Extract(text);

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

            string template = Parser.Compile(text, parsedObject);

            Assert.IsTrue(parsedObject != null);
            Assert.IsTrue(!string.IsNullOrEmpty(template));
        }
    }
}
