using System;

namespace JetBlack.Monads
{
    public static class Either
    {
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft left)
        {
            return new Either<TLeft, TRight>(left, default(TRight), true);
        }

        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight right)
        {
            return new Either<TLeft, TRight>(default(TLeft), right, false);
        }
    }

    public struct Either<TLeft, TRight>
    {
        private readonly TLeft _left;
        private readonly TRight _right;
        private readonly bool? _isLeft;

        internal Either(TLeft left, TRight right, bool isLeft)
            : this()
        {
            _left = left;
            _right = right;
            _isLeft = isLeft;
        }

        public bool IsValid { get { return _isLeft.HasValue; } }

        public bool IsLeft
        {
            get
            {
                if (!_isLeft.HasValue) throw new InvalidOperationException();
                return _isLeft.Value;
            }
        }

        public bool IsRight { get { return !IsLeft; } }

        public TLeft Left
        {
            get
            {
                if (IsLeft) return _left;
                throw new InvalidOperationException();
            }
        }

        public TRight Right
        {
            get
            {
                if (IsRight) return _right;
                throw new InvalidOperationException();
            }
        }

        public T Match<T>(Func<TLeft, T> left, Func<TRight, T> right)
        {
            return IsLeft ? MatchLeft(left) : MatchRight(right);
        }

        public T MatchLeft<T>(Func<TLeft, T> left)
        {
            return left(Left);
        }

        public T MatchRight<T>(Func<TRight, T> right)
        {
            return right(Right);
        }

        public T MatchLeft<T>(Func<TLeft, T> left, T defaultValue)
        {
            return IsLeft ? left(Left) : defaultValue;
        }

        public T MatchRight<T>(Func<TRight, T> right, T defaultValue)
        {
            return IsLeft ? defaultValue : right(Right);
        }

        public override string ToString()
        {
            return !IsValid ? "Invalid" : IsLeft ? "Left: " + Left : "Right: " + Right;
        }
    }
}
