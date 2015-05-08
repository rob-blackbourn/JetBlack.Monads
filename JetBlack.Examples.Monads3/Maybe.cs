using System;

namespace JetBlack.Examples.Monads3
{
    public abstract class Maybe<T> : IEquatable<Maybe<T>>, IEquatable<T>
    {
        public static readonly Maybe<T> Nothing = new Empty();

        private static readonly bool IsValueType = typeof (T).IsValueType;

        public abstract T Value { get; }

        public bool HasValue
        {
            get { return !ReferenceEquals(this, Nothing); }
        }

        public bool Equals(Maybe<T> other)
        {
            return
                other != null &&
                ((this is Empty && other is Empty) ||
                (!(this is Empty || other is Empty) && (Value.Equals(other.Value))));
        }

        public bool Equals(T other)
        {
            return !ReferenceEquals(this, Nothing) && Value.Equals(other);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Maybe<T>);
        }

        public override int GetHashCode()
        {
            return ReferenceEquals(this, Nothing) ? 0 : Value.GetHashCode();
        }

        public static implicit operator Maybe<T>(T value)
        {
            return IsValueType || !Equals(value, default(T)) ? new Just(value) : Nothing;
        }

        public static bool operator ==(Maybe<T> lhs, Maybe<T> rhs)
        {
            return (Equals(lhs, null) && Equals(rhs, null)) || (!Equals(lhs, null) && lhs.Equals(rhs));
        }

        public static bool operator !=(Maybe<T> lhs, Maybe<T> rhs)
        {
            return !(lhs == rhs);
        }

        sealed class Just : Maybe<T>
        {
            private readonly T _value;

            public Just(T value)
            {
                _value = value;
            }

            public override T Value
            {
                get { return _value; }
            }

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        sealed class Empty : Maybe<T>
        {
            public override T Value
            {
                get { throw new InvalidOperationException("Maybe<T>.Nothing has no value"); }
            }

            public override string ToString()
            {
                return "@Nothing!";
            }
        }
    }
}
