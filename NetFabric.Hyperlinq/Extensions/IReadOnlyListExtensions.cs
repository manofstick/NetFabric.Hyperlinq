﻿using System;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public static class IReadOnlyListExtensions
    {
        public static int Count<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate)
            => ReadOnlyList.Count<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source, predicate);

        public static bool All<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate)
            => ReadOnlyList.All<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source, predicate);

        public static bool Any<TSource>(this IReadOnlyList<TSource> source)
            => ReadOnlyList.Any<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source);

        public static bool Any<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate)
            => ReadOnlyList.Any<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source, predicate);

        public static bool Contains<TSource>(this IReadOnlyList<TSource> source, TSource value)
            => ReadOnlyList.Contains<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source, value);

        public static bool Contains<TSource>(this IReadOnlyList<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
            => ReadOnlyList.Contains<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source, value, comparer);

        public static ReadOnlyList.SelectEnumerable<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource, TResult> Select<TSource, TResult>(
            this IReadOnlyList<TSource> source,
            Func<TSource, TResult> selector) 
            => ReadOnlyList.Select<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource, TResult>(source, selector);

        public static ReadOnlyList.WhereEnumerable<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource> Where<TSource>(
            this IReadOnlyList<TSource> source,
            Func<TSource, bool> predicate) 
            => ReadOnlyList.Where<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source, predicate);

        public static TSource First<TSource>(this IReadOnlyList<TSource> source) 
            => ReadOnlyList.First<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source);

        public static TSource First<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate) 
            => ReadOnlyList.First<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source, predicate);

        public static TSource FirstOrDefault<TSource>(this IReadOnlyList<TSource> source) 
            => ReadOnlyList.FirstOrDefault<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source);

        public static TSource FirstOrDefault<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate) 
            => ReadOnlyList.FirstOrDefault<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source, predicate);

        public static TSource Single<TSource>(this IReadOnlyList<TSource> source) 
            => ReadOnlyList.Single<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source);

        public static TSource Single<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate) 
            => ReadOnlyList.Single<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source, predicate);

        public static TSource SingleOrDefault<TSource>(this IReadOnlyList<TSource> source) 
            => ReadOnlyList.SingleOrDefault<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source);

        public static TSource SingleOrDefault<TSource>(this IReadOnlyList<TSource> source, Func<TSource, bool> predicate) 
            => ReadOnlyList.SingleOrDefault<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source, predicate);

        public static ReadOnlyList.AsValueReadOnlyListEnumerable<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource> AsValueReadOnlyList<TSource>(this IReadOnlyList<TSource> source)
            => ReadOnlyList.AsValueReadOnlyList<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source);

        public static TSource[] ToArray<TSource>(this IReadOnlyList<TSource> source)
            => ReadOnlyList.ToArray<IReadOnlyList<TSource>, IEnumerator<TSource>, TSource>(source);
    }
}
