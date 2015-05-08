using System;

namespace JetBlack.Monads2
{
    public sealed class Maybe<T>
    {
        public static Maybe<T> None = new Maybe<T>(default(T)); 

        public Maybe(T value)
        {
            Value = value;
            HasValue = Equals(value, default(ValueType));
        }

        public T Value { get; private set; }
        public bool HasValue { get; private set; }
    }
}
