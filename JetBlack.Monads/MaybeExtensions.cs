using System;

namespace JetBlack.Monads
{
    public static class MaybeExtensions
    {
        //public static Maybe<TResult> Get<T, TResult>(this Maybe<T> maybe, Func<T, TResult> func)
        //{
        //    return maybe.HasValue ? func(maybe.Value).Return() : Maybe<TResult>.Nothing;
        //}

        //public static Maybe<T> If<T>(this Maybe<T> value, Func<T, bool> predicate)
        //{
        //    return value.HasValue && predicate(value.Value) ? value : Maybe<T>.Nothing;
        //}

        //public static TResult Return<T, TResult>(this Maybe<T> maybe, Func<T, TResult> func, TResult defaultValue)
        //{
        //    return maybe.HasValue ? func(maybe.Value) : defaultValue;
        //}

        public static Maybe<TResult> Bind<T, TResult>(this Maybe<T> a, Func<T, Maybe<TResult>> func)
        {
            return a.HasValue ? func(a.Value) : Maybe<TResult>.Nothing;
        }

        public static Maybe<TResult> SelectMany<T, TResult>(this Maybe<T> value, Func<T, Maybe<TResult>> apply)
        {
            return value.HasValue ? apply(value.Value) : Maybe<TResult>.Nothing;
        }

        public static Maybe<TResult> SelectMany<T, TIntermediate, TResult>(this Maybe<T> value, Func<T, Maybe<TIntermediate>> transform, Func<T, TIntermediate, TResult> project)
        {
            return value.SelectMany(x => transform(x).SelectMany(y => Maybe.Return(project(x, y)))); 
        }
    }
}
