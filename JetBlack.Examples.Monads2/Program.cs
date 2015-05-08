using System.Diagnostics;

namespace JetBlack.Examples.Monads2
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
            Test2();
            Test3();
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
    }
}
