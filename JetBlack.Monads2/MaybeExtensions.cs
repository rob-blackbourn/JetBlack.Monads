using System;

namespace JetBlack.Monads2
{
    public static class MaybeExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return Equals(value, default(T)) ? Maybe<T>.None : new Maybe<T>(value);
        }

        public static Maybe<TResult> Get<T, TResult>(this Maybe<T> maybe, Func<T, TResult> func)
        {
            return maybe.HasValue ? func(maybe.Value).ToMaybe() : Maybe<TResult>.None;
        }

        public static Maybe<T> If<T>(this Maybe<T> value, Func<T, bool> predicate)
        {
            return value.HasValue && predicate(value.Value) ? value : Maybe<T>.None;
        }

        public static TResult Return<T, TResult>(this Maybe<T> maybe, Func<T, TResult> func, TResult defaultValue)
        {
            return maybe.HasValue ? func(maybe.Value) : defaultValue;
        }

        public static Maybe<TResult> SelectMany<T, TResult>(this Maybe<T> value, Func<T, Maybe<TResult>> apply)
        {
            return value.HasValue ? apply(value.Value) : Maybe<TResult>.None;
        }

        public static Maybe<TResult> SelectMany<T, TIntermediate, TResult>(this Maybe<T> value, Func<T, Maybe<TIntermediate>> transform, Func<T, TIntermediate, TResult> project)
        {
            return value.SelectMany(x => transform(x).SelectMany(y => project(x, y).ToMaybe())); 
        }
    }
}
