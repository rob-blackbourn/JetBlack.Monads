using System;
using System.Threading;

namespace JetBlack.Monads
{
    public static class Maybe
    {
        public static Maybe<T> Return<T>(this T value)
        {
            return value;
        }
    
        public static Maybe<TResult> Bind<T, TResult>(this Maybe<T> value, Func<T, Maybe<TResult>> apply)
        {
            return value.HasValue ? apply(value.Value) : Maybe<TResult>.Nothing;
        }

        public static Maybe<TResult> SelectMany<T, TIntermediate, TResult>(this Maybe<T> value, Func<T, Maybe<TIntermediate>> transform, Func<T, TIntermediate, TResult> project)
        {
            return value.Bind(x => transform(x).Bind(y => Return(project(x, y))));
        }
    }

    public struct Maybe<T> : IEquatable<Maybe<T>>, IEquatable<T>, IDisposable
    {
        public static readonly Maybe<T> Nothing = new Maybe<T>();

        private static readonly bool IsValueType = typeof(T).IsValueType;

        private int _disposed;

        private Maybe(T value) : this()
        {
            Value = value;
            HasValue = IsValueType || !Equals(value, default(T));
        }

        public T Value { get; private set; }

        public bool HasValue { get; private set; }

        public bool Equals(Maybe<T> other)
        {
            return
                !(HasValue || other.HasValue) ||
                (HasValue && (Value.Equals(other.Value)));
        }

        public bool Equals(T other)
        {
            return HasValue && Value.Equals(other);
        }

        public override bool Equals(object obj)
        {
            return 
                (obj is Maybe<T> && Equals((Maybe<T>)obj)) ||
                (obj is T && Equals((T)obj));
        }

        public override int GetHashCode()
        {
            return HasValue ? Value.GetHashCode() : 0;
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                if (!HasValue) return;

                var disposable = Value as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }

        public override string ToString()
        {
            return HasValue ? Value.ToString() : string.Empty;
        }

        public static implicit operator Maybe<T>(T value)
        {
            return IsValueType || !Equals(value, default(T)) ? new Maybe<T>(value) : Nothing;
        }

        public static bool operator ==(Maybe<T> lhs, Maybe<T> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Maybe<T> lhs, Maybe<T> rhs)
        {
            return !lhs.Equals(rhs);
        }
    }
}
