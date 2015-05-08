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
    }
}
