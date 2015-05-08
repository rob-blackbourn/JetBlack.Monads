using System;
using System.Collections.Generic;
using System.Linq;

namespace JetBlack.Examples.Monads1
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
            Test2();
            Test3();
            Test4();
            Test5();
            Test6();

            var foo = new List<List<int>>();
            var bar = foo.SelectMany(x => x);
        }

        static void Test1()
        {
            Func<int, int> add2 = x => x + 2;
            Func<int, int> mult2 = x => x * 2;

            Func<int, int> add2Mult2 = x => mult2(add2(x));

            var r2 = add2Mult2(5);
            Console.Out.WriteLine("r2 = {0}", r2);
        }

        static void Test2()
        {
            Func<int, Identity<int>> add2 = x => new Identity<int>(x + 2);
            Func<int, Identity<int>> mult2 = x => new Identity<int>(x * 2);

            // we can't compose them directly, the types don't match. This won't compile:
            //Func<int, Identity<int>> add2Mult2 = x => mult2(add2(x));
            Func<int, Identity<int>> add2Mult2 = x => add2(x).Bind(mult2);

            var r1 = add2Mult2(5);
            Console.Out.WriteLine("r1.Value = {0}", r1.Value);
        }

        private static void Test3()
        {
            var result = "Hello World!".ToIdentity()
                .Bind(a => 7.ToIdentity()
                    .Bind(b => new DateTime(2010, 1, 11).ToIdentity()
                        .Bind(c => (a + ", " + b + ", " + c.ToShortDateString()).ToIdentity())));

            Console.WriteLine(result.Value);
        }

        private static void Test4()
        {
            var result =
                from a in "Hello World!".ToIdentity()
                from b in 7.ToIdentity()
                from c in (new DateTime(2010, 1, 11)).ToIdentity()
                select a + ", " + b + ", " + c.ToShortDateString();

            Console.WriteLine(result.Value);
        }

        private static void Test5()
        {
            var result = from a in "Hello World!".ToMaybe()
                         from b in DoSomeDivision(2)
                         from c in (new DateTime(2010, 1, 14)).ToMaybe()
                         select a + " " + b + " " + c.ToShortDateString();

            Console.WriteLine(result);
        }

        private static void Test6()
        {
            var result = from a in "Hello World!".ToMaybe()
                         from b in DoSomeDivision(0)
                         from c in (new DateTime(2010, 1, 14)).ToMaybe()
                         select a + " " + b.ToString() + " " + c.ToShortDateString();

            Console.WriteLine(result);
        }

        private static void Test7()
        {
            var result = "Hello World!".ToMaybe()
                .SelectMany(_ => DoSomeDivision(0), (a, b) => new {a, b})
                .SelectMany(_ => (new DateTime(2010, 1, 14)).ToMaybe(), (x, c) => x.a + " " + x.b + " " + c.ToShortDateString());

            Console.WriteLine(result);
        }

        public static Maybe<int> DoSomeDivision(int denominator)
        {
            return from a in 12.Div(denominator)
                   from b in a.Div(2)
                   select b;
        }
    }

    public static class TestExtentions
    {
        public static Maybe<int> Div(this int numerator, int denominator)
        {
            return denominator == 0
                       ? (Maybe<int>)new Nothing<int>()
                       : new Just<int>(numerator / denominator);
        }
    }
}
