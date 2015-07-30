using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace JetBlack.Monads.Test
{
    [TestFixture]
    public class EitherTest
    {
        [Test]
        public void ConstructorTest()
        {
            var e0 = new Either<int,double> ();
            Assert.IsFalse(e0.IsValid);
            var e1 = Either.Left<int, double>(1);
            Assert.IsTrue(e1.IsLeft);
            var e2 = Either.Right<int, double>(1);
            Assert.IsTrue(e2.IsRight);
        }

        [Test]
        public void TestFactorial()
        {
            var calculator = new FactorialCalculator();

            var expected = Enumerable.Range(1, 10).Select(i => new {i, r = calculator.Factorial(i)}).ToDictionary(key => key.i, value => value.r);

            foreach (var pair in expected)
                Assert.AreEqual(pair.Value, calculator.Calculate(pair.Key).Match(x => x, x => x.Value));

            foreach (var pair in expected)
                Assert.AreEqual(pair.Value, calculator.Calculate(pair.Key).Match(x => x, x => x.Value));
        }

        public class FactorialCalculator
        {
            private readonly IDictionary<int,int> _cache = new Dictionary<int, int>();

            public Either<int, Lazy<int>> Calculate(int i)
            {
                int factorial;
                if (_cache.TryGetValue(i, out factorial))
                    return Either.Left<int, Lazy<int>>(factorial);
                
                return Either.Right<int, Lazy<int>>(new Lazy<int>(() =>
                {
                    factorial = Factorial(i);
                    _cache[i] = factorial;
                    return factorial;
                }));
            }

            public int Factorial(int i)
            {
                if (i <= 1)
                    return 1;
                return i * Factorial(i - 1);
            }
        }
    }
}
