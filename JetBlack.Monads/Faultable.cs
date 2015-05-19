using System;

namespace JetBlack.Monads
{
    public static class Faultable
    {
        public static Faultable<T> Return<T>(this T value)
        {
            return value;
        }

        public static Faultable<T> Return<T>(this Exception error)
        {
            return error;
        }

        public static Faultable<T> Return<T>(this Func<T> func)
        {
            return func;
        }

        public static Faultable<TResult> Return<T, TResult>(this Func<T, TResult> func, T value)
        {
            return Return(() => func(value));
        }

        public static Faultable<TResult> Bind<T, TResult>(this Faultable<T> value, Func<T, Faultable<TResult>> transform)
        {
            return value.IsFaulted ? value.Error : transform(value.Value);
        }

        public static Faultable<TResult> Bind<T, TResult>(this Faultable<T> value, Func<T, TResult> transform)
        {
            return value.Bind(arg => Return(transform, arg));
        }

        public static Faultable<TResult> SelectMany<T, TIntermediate, TResult>(this Faultable<T> value, Func<T, Faultable<TIntermediate>> transform, Func<T, TIntermediate, TResult> project)
        {
            return value.Bind(x => transform(x).Bind(y => Return(project(x, y))));
        }
    }

    public struct Faultable<T> : IEquatable<Faultable<T>>, IEquatable<T>
    {
        private static readonly bool IsValueType = typeof (T).IsValueType;

        public bool IsFaulted { get; private set; }
        public Exception Error { get; private set; }
        public T Value { get; private set; }

        private Faultable(T value) : this()
        {
            IsFaulted = false;
            Value = value;
        }

        private Faultable(Exception error) : this()
        {
            IsFaulted = true;
            Error = error;
        }

        public bool Equals(Faultable<T> other)
        {
            return !(IsFaulted || other.IsFaulted) && Equals(Value, other.Value);
        }

        public bool Equals(T other)
        {
            return !IsFaulted && Equals(Value, other);
        }

        public override bool Equals(object obj)
        {
            return
                (obj is Faultable<T> && Equals((Faultable<T>)obj)) ||
                (obj is T && Equals((T)obj));
        }

        public override int GetHashCode()
        {
            return IsFaulted ? Error.GetHashCode() : IsValueType || !Equals(Value, default(T)) ? Value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return
                IsFaulted
                    ? Error.ToString()
                    : IsValueType
                        ? Value.ToString()
                        : Equals(Value, default(T))
                            ? string.Empty
                            : Value.ToString();
        }

        public static implicit operator Faultable<T>(T value)
        {
            return new Faultable<T>(value);
        }

        public static implicit operator Faultable<T>(Exception error)
        {
            return new Faultable<T>(error);
        }

        public static implicit operator Faultable<T>(Func<T> func)
        {
            try
            {
                return new Faultable<T>(func());
            }
            catch (Exception error)
            {
                return new Faultable<T>(error);
            }
        }
    }
}
