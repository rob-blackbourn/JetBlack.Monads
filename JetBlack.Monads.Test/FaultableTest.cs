using System;
using NUnit.Framework;

namespace JetBlack.Monads.Test
{
    [TestFixture]
    public class FaultableTest
    {
        [Test]
        public void TestBind()
        {
            var r1 = Faultable.Return(2)
                .Bind(x => x + 2)
                .Bind(y => 8 / y);
            Assert.IsFalse(r1.IsFaulted);
            Assert.AreEqual(2, r1.Value);

            var r2 = Faultable.Return(0)
                .Bind(x => x + 6 / x)
                .Bind(y => y + 7);
            Assert.IsTrue(r2.IsFaulted);
            Assert.IsTrue(r2.Error is DivideByZeroException);

            var r3 = Faultable.Return(2)
                .Bind(x => x - 2)
                .Bind(y => 7 / y);
            Assert.IsTrue(r3.IsFaulted);
            Assert.IsTrue(r3.Error is DivideByZeroException);
        }

        [Test]
        public void TestSelectMany()
        {
            var r4 = from x in Faultable.Return(2)
                     from y in Faultable.Return(() => 6 / x)
                     from z in Faultable.Return(4)
                     select x + y + z;
            Assert.IsFalse(r4.IsFaulted);
            Assert.AreEqual(9, r4.Value);

            var r5 = from x in Faultable.Return(0)
                     from y in Faultable.Return(() => 6 / x)
                     from z in Faultable.Return(7)
                     select x + y + z;
            Assert.IsTrue(r5.IsFaulted);
            Assert.IsTrue(r5.Error is DivideByZeroException);
        }
    }
}
