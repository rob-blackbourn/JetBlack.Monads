using System;

namespace JetBlack.Monads
{
    public static class MaybeExtensions
    {
        public static Maybe<TResult> Bind<T, TResult>(this Maybe<T> value, Func<T, Maybe<TResult>> apply)
        {
            return value.HasValue ? apply(value.Value) : Maybe<TResult>.Nothing;
        }

        public static Maybe<TResult> SelectMany<T, TIntermediate, TResult>(this Maybe<T> value, Func<T, Maybe<TIntermediate>> transform, Func<T, TIntermediate, TResult> project)
        {
            return value.Bind(x => transform(x).Bind(y => Maybe.Return(project(x, y)))); 
        }
    }
}
