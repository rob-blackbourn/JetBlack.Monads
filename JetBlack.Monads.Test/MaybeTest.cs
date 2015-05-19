using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
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

        [Test]
        public void TestHeterogenious()
        {
            var people = new[]
            {
                new Person("Tom", "tom@fictional.com", new DateTime(1952, 3, 28),
                    new List<Address>
                    {
                        new Address("1 High Street", "Pink Town", "786123", "United States"),
                        new Address("23 Long Acre", "Little Grove", "786123", "United States")
                    }),
                new Person("Dick", "dick@toolman.com", new DateTime(1971, 11, 1),
                    new List<Address>
                    {
                        new Address("42 Blossom Hill", "Green Town", "566722", "Canada"),
                        new Address("921 Long Street", "Sleepy Hollow", "877766", "Canada")
                    }),
                new Person("Harry", "harry@localfarmers.com", new DateTime(2001, 1, 3),
                    new List<Address>
                    {
                        new Address("12 Ocean Rise", "Blue Town", "712812", "United States"),
                        new Address("1220 The Mount", "Moon Valley", "927612", "United States")
                    })
            }.ToDictionary(x => x.Name, y => y);

            Assert.AreEqual(typeof (Address), people.TryGetValue("Harry").Bind(x => x.Addresses.FirstOrDefault(y => y.Country == "United States")).Value.GetType());
            Assert.IsFalse(people.TryGetValue("Nancy").Bind(x => x.Addresses.FirstOrDefault(y => y.Country == "United States")).HasValue);
            Assert.IsFalse(people.TryGetValue("Harry").Bind(x => x.Addresses.FirstOrDefault(y => y.Country == "Canada")).HasValue);
        }
    }
}
