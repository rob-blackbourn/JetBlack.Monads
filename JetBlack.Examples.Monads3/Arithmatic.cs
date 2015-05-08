namespace JetBlack.Examples.Monads3
{
    public static class Arithmatic
    {
        public static Maybe<int> Add2(Maybe<int> a, Maybe<int> b)
        {
            return a.Bind(x => b.Bind(y => (x + y).ToMaybe()));
        }

        public static Maybe<int> Add(Maybe<int> a, Maybe<int> b)
        {
            return a.HasValue && b.HasValue ? a.Value + b.Value : Maybe<int>.Nothing;
        }

        public static Maybe<int> Subtract(Maybe<int> a, Maybe<int> b)
        {
            return a.HasValue && b.HasValue ? a.Value - b.Value : Maybe<int>.Nothing;
        }

        public static Maybe<int> Multiple(Maybe<int> a, Maybe<int> b)
        {
            return a.HasValue && b.HasValue ? a.Value * b.Value : Maybe<int>.Nothing;
        }

        public static Maybe<int> Divide(Maybe<int> a, Maybe<int> b)
        {
            return a.HasValue && b.HasValue && b.Value != 0 ? a.Value + b.Value : Maybe<int>.Nothing;
        }
    }
}
