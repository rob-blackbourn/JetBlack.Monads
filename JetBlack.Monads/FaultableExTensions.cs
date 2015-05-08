using System;

namespace JetBlack.Monads
{
    public static class FaultableExtensions
    {
        public static Faultable<TResult> Bind<T, TResult>(this Faultable<T> value, Func<T, Faultable<TResult>> transform)
        {
            return value.IsFaulted ? value.Error : transform(value.Value);
        }

        public static Faultable<TResult> Bind<T, TResult>(this Faultable<T> value, Func<T, TResult> transform)
        {
            return value.Bind(transform.Return);
        }

        public static Faultable<TResult> SelectMany<T, TIntermediate, TResult>(this Faultable<T> value, Func<T, Faultable<TIntermediate>> transform, Func<T, TIntermediate, TResult> project)
        {
            return value.Bind(x => transform(x).Bind(y => Faultable.Return(project(x, y))));
        }
    }
}