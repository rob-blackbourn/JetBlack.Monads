using System;

namespace JetBlack.Examples.Monads4
{
    public abstract class Either<TLeft, TRight>
    {
        public abstract bool IsLeft { get; }

        public abstract TLeft Left { get; }

        public abstract TRight Right { get; }

        public abstract void Switch(Action<TLeft> left, Action<TRight> right);

        public abstract TResult Switch<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right);

        public override string ToString()
        {
            return
                IsLeft
                    ? string.Format("{{Left: {0}}}", Left)
                    : string.Format("{{Right: {0}}}", Right);
        }
    }

    public static class Either
    {
        private sealed class LeftValue<TLeft, TRight> : Either<TLeft, TRight>
        {
            public override bool IsLeft { get { return true; } }

            public override TLeft Left { get { return _value; } }

            public override TRight Right { get { return default(TRight); } }

            private readonly TLeft _value;

            public LeftValue(TLeft value)
            {
                _value = value;
            }

            public override void Switch(Action<TLeft> left, Action<TRight> right)
            {
                left(_value);
            }

            public override TResult Switch<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right)
            {
                return left(_value);
            }
        }

        private sealed class RightValue<TLeft, TRight> : Either<TLeft, TRight>
        {
            public override bool IsLeft { get { return false; } }

            public override TLeft Left { get { return default(TLeft); } }

            public override TRight Right { get { return _value; } }

            private readonly TRight _value;

            public RightValue(TRight value)
            {
                _value = value;
            }

            public override void Switch(Action<TLeft> left, Action<TRight> right)
            {
                right(_value);
            }

            public override TResult Switch<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right)
            {
                return right(_value);
            }
        }

        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
        {
            return new LeftValue<TLeft, TRight>(value);
        }

        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value)
        {
            return new RightValue<TLeft, TRight>(value);
        }
    }
}
