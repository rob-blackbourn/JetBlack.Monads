using System;

namespace JetBlack.Examples.Monads2
{
    public abstract class Maybe<T> : IEquatable<Maybe<T>>, IEquatable<T>
    {
        public static readonly Maybe<T> Nothing = Nothing<T>.Instance;

        public abstract T Value { get; }

        public bool HasValue
        {
            get { return !ReferenceEquals(this, Nothing); }
        }

        public bool Equals(Maybe<T> other)
        {
            return
                other != null &&
                (ReferenceEquals(this, Nothing) && ReferenceEquals(other, Nothing)) ||
                (!(ReferenceEquals(this, Nothing) || ReferenceEquals(other, Nothing)) && (Value.Equals(other.Value)));
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

        public static bool operator ==(Maybe<T> lhs, Maybe<T> rhs)
        {
            return (lhs == null && rhs == null) || (lhs != null && lhs.Equals(rhs));
        }

        public static bool operator !=(Maybe<T> lhs, Maybe<T> rhs)
        {
            return !(lhs == rhs);
        }
    }

    internal sealed class Nothing<T> : Maybe<T>
    {
        public static Nothing<T> Instance = new Nothing<T>();

        private Nothing()
        {
        }

        public override T Value
        {
            get { throw new InvalidOperationException("Maybe<T>.Nothing has no value"); }
        }

        public override string ToString()
        {
            return "@Nothing!";
        }
    }

    internal sealed class Just<T> : Maybe<T>
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

    public static class Maybe
    {
        public static Maybe<T> FromClass<T>(this T value) where T : class
        {
            return value == null ? Maybe<T>.Nothing : new Just<T>(value);
        }

        public static Maybe<T> FromStruct<T>(this T value) where T : struct
        {
            return new Just<T>(value);
        }

        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return typeof (T).IsValueType || ! Equals(value, default(T)) ? new Just<T>(value) : Maybe<T>.Nothing;
        }
    }
}
