using System.Collections.Generic;

namespace JetBlack.Monads
{
    public static class DictionaryEx
    {
        public static Maybe<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Maybe<TKey> key)
        {
            return Maybe.Return(dictionary).TryGetValue(key);
        }

        public static Maybe<TValue> TryGetValue<TKey, TValue>(this Maybe<IDictionary<TKey, TValue>> dictionary, Maybe<TKey> key)
        {
            return dictionary.Bind(x => key.Bind(x.TryGetValue));
        }

        private static Maybe<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? Maybe.Return(value) : Maybe<TValue>.Nothing;
        }
    }
}
