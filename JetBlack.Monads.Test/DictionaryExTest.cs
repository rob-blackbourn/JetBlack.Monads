using System.Collections.Generic;
using NUnit.Framework;

namespace JetBlack.Monads.Test
{
    [TestFixture]
    public class DictionaryExTest
    {
        [Test]
        public void Test()
        {
            var dictionary = new Dictionary<string, IDictionary<string, string>>
            {
                {
                    "Berkshire", new Dictionary<string, string>
                    {
                        {"Reading", "Eldon Square"},
                        {"Wokingham", "High Street"}
                    }
                },
                {
                    "Hertfordshire", new Dictionary<string, string>
                    {
                        {"Ascot", "Windsor Road"},
                        {"Eaton", "Posh Avenue"}
                    }
                }
            };

            Assert.AreEqual(dictionary.TryGetValue("Berkshire").TryGetValue("Reading").Value, "Eldon Square");
            Assert.IsFalse(dictionary.TryGetValue("Berkshire").TryGetValue("Picadilly").HasValue);
            Assert.IsFalse(dictionary.TryGetValue("London").TryGetValue("Reading").HasValue);
        }
    }
}
