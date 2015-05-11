using System.Collections.Generic;
using NUnit.Framework;

namespace JetBlack.Monads.Test
{
    [TestFixture]
    public class MaybeTest
    {
        [Test]
        public void ShouldEqual()
        {
            var a = Maybe.Return(5);
            var b = Maybe.Return(5);
            var c = Maybe.Return(6);
            var d = Maybe<int>.Nothing;
            var e = Maybe<int>.Nothing;

            Assert.AreEqual(a, b);
            Assert.AreNotEqual(a, c);
            Assert.AreNotEqual(a, d);
            Assert.AreEqual(d, e);

            Assert.IsTrue(a == b);
            Assert.IsFalse(a == c);
            Assert.IsFalse(a == d);
            Assert.IsTrue(d == e);
        }

        [Test]
        public void TestDictionary()
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

    public static class DictionaryExtensions
    {
        public static Maybe<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Maybe<TKey> key)
        {
            return Maybe.Return(dictionary).TryGetValue(key);
        }

        public static Maybe<TValue> TryGetValue<TKey, TValue>(this Maybe<IDictionary<TKey, TValue>> dictionary, Maybe<TKey> key)
        {
            return dictionary.Bind(x => key.Bind(x.TryGetValue));
        }

        private static Maybe<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? Maybe.Return(value) : Maybe<TValue>.Nothing;
        }
    }

}
