using System.Diagnostics;

namespace JetBlack.Examples.Monads3
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
        }

        static void Test1()
        {
            var r = from x in 5.ToMaybe()
                    from y in 6.ToMaybe()
                    select x + y;
            Debug.Print("r={0}", r);
        }

        static void Test2()
        {
            var r = from x in 5.ToMaybe()
                    from y in Maybe<int>.Nothing
                    select x + y;
            Debug.Print("r={0}", r);
        }

        static void Test3()
        {
            var r = from x in 5.ToMaybe()
                    from y in 0.ToMaybe()
                    select x + y;
            Debug.Print("r={0}", r);
        }

        static void Test4()
        {
            var r1 = Arithmatic.Add2(2, 3);
            Debug.Print("r1={0}", r1);
            var r2 = Arithmatic.Add2(Maybe<int>.Nothing, 3);
            Debug.Print("r2={0}", r2);
            var r3 = Arithmatic.Add2(2, Maybe<int>.Nothing);
            Debug.Print("r3={0}", r3);
            var r4 = Arithmatic.Add2(Maybe<int>.Nothing, Maybe<int>.Nothing);
            Debug.Print("r4={0}", r4);
            var r5 = from x in 2.ToMaybe()
                from y in 3.ToMaybe()
                select x + y;
            Debug.Print("r5={0}", r5);
        }

        static void Test5()
        {
            var a = 5.ToMaybe();
            var b = 5.ToMaybe();
            var c = 6.ToMaybe();
            var d = Maybe<int>.Nothing;
            var e = Maybe<int>.Nothing;

            Debug.Print("a == a is {0}", a == a);
            Debug.Print("a == b is {0}", a == b);
            Debug.Print("a == c is {0}", a == c);
            Debug.Print("a == d is {0}", a == d);
            Debug.Print("d == d is {0}", d == d);
            Debug.Print("e == e is {0}", d == e);
        }
    }
}
