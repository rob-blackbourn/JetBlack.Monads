using System;
using System.Collections.Generic;

namespace JetBlack.Examples.Monads4
{
    public static class Maybe
    {
        public static Maybe<T> Empty<T>()
        {
            return Maybe<T>.Empty;
        }

        public static Maybe<T> Return<T>(T value)
        {
            var maybe = new Maybe<T>(value);
            return maybe;
        }
    }

    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        public bool HasValue { get { return _hasValue; } }

        public T Value { get { return _value; } }

        internal static readonly Maybe<T> Empty = new Maybe<T>();

        private readonly T _value;
        private readonly bool _hasValue;

        internal Maybe(T value)
        {
            _value = value;
            _hasValue = true;
        }

        public static bool operator ==(Maybe<T> first, Maybe<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Maybe<T> first, Maybe<T> second)
        {
            return !first.Equals(second);
        }

        public override bool Equals(object obj)
        {
            return obj is Maybe<T> && Equals((Maybe<T>)obj);
        }

        public bool Equals(Maybe<T> other)
        {
            return _hasValue == other._hasValue && (!_hasValue || EqualityComparer<T>.Default.Equals(_value, other._value));
        }

        public override int GetHashCode()
        {
            return _hasValue
                ? _value == null ? 0 : _value.GetHashCode()
                : -1;
        }

        public override string ToString()
        {
            return _value == null ? string.Empty : _value.ToString();
        }
    }
}
