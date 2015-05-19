# JetBlack.Monads

## Overview

Some example monads with practical applications.

## Maybe

The `Maybe<T>` monad has two states, the value of type `T` or nothing. It can
be used when chaining a series of methods which may or may not provide a result. The
type `Maybe<T>` is said to "amplify" the type `T`, as it gives the type more
functionality. In this case to determine it's existance.

The key to the pattern is the `Bind` method which has the following implementation.

```cs
    public static Maybe<TResult> Bind<T, TResult>(this Maybe<T> value, Func<T, Maybe<TResult>> apply)
    {
        return value.HasValue ? apply(value.Value) : Maybe<TResult>.Nothing;
    }
```

If the supplied parameter has a value the function is applied, otherwise the
Nothing value is propogated.

Another key feature of a monad is the `Return` function which returns a monad amplifying the appropriate type. This is achieved with an implicit operator:

```cs
    public struct Maybe<T>
    {
        ...

        public static implicit operator Maybe<T>(T value)
        {
            return IsValueType || !Equals(value, default(T)) ? new Maybe<T>(value) : Nothing;
        }

        ...
    }
```

And a helper extension:

```cs
    public static class Maybe
    {
        ...

        public static Maybe<T> Return<T>(this T value)
        {
            return value;
        }

        ...
    }
```

### Simple Example

In the test assembly we can see an implementation of a dictionary search:

```cs
    var dictionary = new Dictionary<string, IDictionary<string, string>>
    {
        {
            "Berkshire", new Dictionary<string, string>
            {
                {"Reading", "Eldon Square"},
                {"Wokingham", "High Street"}
            }
        },
        {
            "Hertfordshire", new Dictionary<string, string>
            {
                {"Ascot", "Windsor Road"},
                {"Eaton", "Posh Avenue"}
            }
        }
    };

    Assert.AreEqual(dictionary.TryGetValue("Berkshire").TryGetValue("Reading").Value, "Eldon Square");
    Assert.IsFalse(dictionary.TryGetValue("Berkshire").TryGetValue("Picadilly").HasValue);
    Assert.IsFalse(dictionary.TryGetValue("London").TryGetValue("Reading").HasValue);
```

In this case we don't care why the search failed; only that there was no result.

The implementations of `TryGetValue` are as follows:

```cs
    public static class DictionaryExtensions
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
```

The logic for the propogation of success and failure are factored out, as they
are the responsibility of the monad.

### Heterogeneous Example

In the above example the result at each stage was the same type; a `string`. In this example
we use a class structure where the first type is a `Person`, but the final result is a `Maybe<Address>`.

```cs
    var people = new[]
    {
        new Person("Tom", "tom@fictional.com", new DateTime(1952, 3, 28),
            new List<Address>
            {
                new Address("1 High Street", "Pink Town", "786123", "United States"),
                new Address("23 Long Acre", "Little Grove", "786123", "United States")
            }),
        new Person("Dick", "dick@toolman.com", new DateTime(1971, 11, 1),
            new List<Address>
            {
                new Address("42 Blossom Hill", "Green Town", "566722", "Canada"),
                new Address("921 Long Street", "Sleepy Hollow", "877766", "Canada")
            }),
        new Person("Harry", "harry@localfarmers.com", new DateTime(2001, 1, 3),
            new List<Address>
            {
                new Address("12 Ocean Rise", "Blue Town", "712812", "United States"),
                new Address("1220 The Mount", "Moon Valley", "927612", "United States")
            })
    }.ToDictionary(x => x.Name, y => y);

    Assert.AreEqual(typeof (Address), people.TryGetValue("Harry").Bind(x => x.Addresses.FirstOrDefault(y => y.Country == "United States")).Value.GetType());
    Assert.IsFalse(people.TryGetValue("Nancy").Bind(x => x.Addresses.FirstOrDefault(y => y.Country == "United States")).HasValue);
    Assert.IsFalse(people.TryGetValue("Harry").Bind(x => x.Addresses.FirstOrDefault(y => y.Country == "Canada")).HasValue);
}
```

### Implementation

A number of implementations were experimented with. The version provided here
satisfies the following design goals.

 1.  The monad should be a struct as it has few properties and will be frequently created and destroyed.
 2.  It should not be possible to create the monad in an invalid state.
 3.  The `null` value should be considered equivalent to `Maybe<T>.Nothing` when constructing the monad.

## Faultable

The `Faultable` monad captures the case where an operation my throw an exception.

The example in the test assembly is as follows:

```cs
    var r1 = Faultable.Return(2)
        .Bind(x => x + 2)
        .Bind(y => 8 / y);
    Assert.IsFalse(r1.IsFaulted);
    Assert.AreEqual(2, r1.Value);

    var r2 = Faultable.Return(0)
        .Bind(x => x + 6 / x)
        .Bind(y => y + 7);
    Assert.IsTrue(r2.IsFaulted);
    Assert.IsTrue(r2.Error is DivideByZeroException);

    var r3 = Faultable.Return(2)
        .Bind(x => x - 2)
        .Bind(y => 7 / y);
    Assert.IsTrue(r3.IsFaulted);
    Assert.IsTrue(r3.Error is DivideByZeroException);
```

We can see that the point at which the exception is thrown is not important. We
simply wish to attempt to apply the functions and capture either the result or
the error.