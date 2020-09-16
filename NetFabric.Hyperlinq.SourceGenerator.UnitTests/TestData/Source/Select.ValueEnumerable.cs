using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        public static SelectEnumerable<TEnumerable, TEnumerator, TSource, TResult, ValueNullableSelectorWrapper<TSource, TResult>> Select<TEnumerable, TEnumerator, TSource, TResult>(this TEnumerable source, NullableSelector<TSource, TResult> selector)
            where TEnumerable : notnull, IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            => Select<TEnumerable, TEnumerator, TSource, TResult, ValueNullableSelectorWrapper<TSource, TResult>>(source, new ValueNullableSelectorWrapper<TSource, TResult>(selector));

        public static SelectEnumerable<TEnumerable, TEnumerator, TSource, TResult, TSelector> Select<TEnumerable, TEnumerator, TSource, TResult, TSelector>(this TEnumerable source, TSelector selector)
            where TEnumerable : notnull, IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TSelector : struct, INullableSelector<TSource, TResult>
            => new SelectEnumerable<TEnumerable, TEnumerator, TSource, TResult, TSelector>(source, selector);

        public readonly partial struct SelectEnumerable<TEnumerable, TEnumerator, TSource, TResult, TSelector>
            : IValueEnumerable<TResult, SelectEnumerable<TEnumerable, TEnumerator, TSource, TResult, TSelector>.DisposableEnumerator>
            where TEnumerable : notnull, IValueEnumerable<TSource, TEnumerator>
            where TEnumerator : struct, IEnumerator<TSource>
            where TSelector : struct, INullableSelector<TSource, TResult>
        {
            readonly TEnumerable source;
            readonly TSelector selector;

            internal SelectEnumerable(TEnumerable source, TSelector selector)
                => (this.source, this.selector) = (source, selector);

            public readonly Enumerator GetEnumerator()
                => new Enumerator();
            readonly DisposableEnumerator IValueEnumerable<TResult, SelectEnumerable<TEnumerable, TEnumerator, TSource, TResult, TSelector>.DisposableEnumerator>.GetEnumerator()
                => new DisposableEnumerator();
            readonly IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
                => new DisposableEnumerator();
            readonly IEnumerator IEnumerable.GetEnumerator()
                => new DisposableEnumerator();

            public struct Enumerator
            {
            }

            public struct DisposableEnumerator
                : IEnumerator<TResult>
            {
                public readonly TResult Current => default!;
                readonly TResult IEnumerator<TResult>.Current => default!;
                readonly object? IEnumerator.Current => default;

                public bool MoveNext()
                    => false;

                [ExcludeFromCodeCoverage]
                public readonly void Reset()
                    => throw new NotSupportedException();

                public void Dispose() { }
            }

            public int Count()
                => 0;

            public SelectEnumerable<TEnumerable, TEnumerator, TSource, TResult2, NullableSelectorNullableSelectorCombination<TSelector, ValueNullableSelectorWrapper<TResult, TResult2>, TSource, TResult, TResult2>> Select<TResult2>(NullableSelector<TResult, TResult2> selector)
                => Select<ValueNullableSelectorWrapper<TResult, TResult2>, TResult2>(new ValueNullableSelectorWrapper<TResult, TResult2>(selector));

            public SelectEnumerable<TEnumerable, TEnumerator, TSource, TResult2, NullableSelectorNullableSelectorCombination<TSelector, TSelector2, TSource, TResult, TResult2>> Select<TSelector2, TResult2>(TSelector2 selector)
                where TSelector2 : struct, INullableSelector<TResult, TResult2>
                => Select<TEnumerable, TEnumerator, TSource, TResult2, NullableSelectorNullableSelectorCombination<TSelector, TSelector2, TSource, TResult, TResult2>>(source, new NullableSelectorNullableSelectorCombination<TSelector, TSelector2, TSource, TResult, TResult2>(this.selector, selector));
        }
    }
}

