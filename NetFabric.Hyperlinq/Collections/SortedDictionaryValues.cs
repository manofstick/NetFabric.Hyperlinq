﻿using System;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public static partial class ReadOnlyCollection
    {
        public static int Count<TKey, TValue>(this SortedDictionary<TKey, TValue>.ValueCollection source)
            => source.Count;

        public static SelectReadOnlyCollection<SortedDictionary<TKey, TValue>.ValueCollection, SortedDictionary<TKey, TValue>.ValueCollection.Enumerator, TValue, TResult> Select<TKey, TValue, TResult>(
            this SortedDictionary<TKey, TValue>.ValueCollection source,
            Func<TValue, TResult> selector) 
            => Select<SortedDictionary<TKey, TValue>.ValueCollection, SortedDictionary<TKey, TValue>.ValueCollection.Enumerator, TValue, TResult>(source, selector);

        public static Enumerable.WhereEnumerable<SortedDictionary<TKey, TValue>.ValueCollection, SortedDictionary<TKey, TValue>.ValueCollection.Enumerator, TValue> Where<TKey, TValue>(
            this SortedDictionary<TKey, TValue>.ValueCollection source,
            Func<TValue, bool> predicate) 
            => Enumerable.Where<SortedDictionary<TKey, TValue>.ValueCollection, SortedDictionary<TKey, TValue>.ValueCollection.Enumerator, TValue>(source, predicate);

        public static TValue First<TKey, TValue>(this SortedDictionary<TKey, TValue>.ValueCollection source)
            => First<SortedDictionary<TKey, TValue>.ValueCollection, SortedDictionary<TKey, TValue>.ValueCollection.Enumerator, TValue>(source);

        public static TValue FirstOrDefault<TKey, TValue>(this SortedDictionary<TKey, TValue>.ValueCollection source)
            => FirstOrDefault<SortedDictionary<TKey, TValue>.ValueCollection, SortedDictionary<TKey, TValue>.ValueCollection.Enumerator, TValue>(source);

        public static TValue Single<TKey, TValue>(this SortedDictionary<TKey, TValue>.ValueCollection source)
            => Single<SortedDictionary<TKey, TValue>.ValueCollection, SortedDictionary<TKey, TValue>.ValueCollection.Enumerator, TValue>(source);

        public static TValue SingleOrDefault<TKey, TValue>(this SortedDictionary<TKey, TValue>.ValueCollection source)
            => SingleOrDefault<SortedDictionary<TKey, TValue>.ValueCollection, SortedDictionary<TKey, TValue>.ValueCollection.Enumerator, TValue>(source);
    }
}