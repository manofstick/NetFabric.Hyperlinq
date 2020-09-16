using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Hyperlinq
{
    public static partial class ArrayExtensions
    {
        public static ArraySegmentWhereEnumerable<TSource, ValuePredicateWrapper<TSource>> Where<TSource>(this in ArraySegment<TSource> source, Predicate<TSource> predicate)
            => Where(source, new ValuePredicateWrapper<TSource>(predicate));

        public static ArraySegmentWhereEnumerable<TSource, TPredicate> Where<TSource, TPredicate>(this in ArraySegment<TSource> source, TPredicate predicate)
            where TPredicate : struct, IPredicate<TSource>
            => new ArraySegmentWhereEnumerable<TSource, TPredicate>(source, predicate);

        public readonly partial struct ArraySegmentWhereEnumerable<TSource, TPredicate>
            : IValueEnumerable<TSource, ArraySegmentWhereEnumerable<TSource, TPredicate>.DisposableEnumerator>
            where TPredicate : struct, IPredicate<TSource>
        {
            readonly ArraySegment<TSource> source;
            readonly TPredicate predicate;

            internal ArraySegmentWhereEnumerable(in ArraySegment<TSource> source, TPredicate predicate)
                => (this.source, this.predicate) = (source, predicate);

            public readonly Enumerator GetEnumerator()
                => new Enumerator();
            readonly DisposableEnumerator IValueEnumerable<TSource, ArraySegmentWhereEnumerable<TSource, TPredicate>.DisposableEnumerator>.GetEnumerator()
                => new DisposableEnumerator();
            readonly IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator()
                => new DisposableEnumerator();
            readonly IEnumerator IEnumerable.GetEnumerator()
                => new DisposableEnumerator();

            public struct Enumerator
            {
            }

            public struct DisposableEnumerator
                : IEnumerator<TSource>
            {
                public readonly TSource Current => default!;
                readonly TSource IEnumerator<TSource>.Current => default!;
                readonly object? IEnumerator.Current => default;

                public bool MoveNext()
                    => false;

                [ExcludeFromCodeCoverage]
                public readonly void Reset()
                    => throw new NotSupportedException();

                public void Dispose() { }
            }

            public ArraySegmentWhereEnumerable<TSource, PredicatePredicateCombination<TPredicate, ValuePredicateWrapper<TSource>, TSource>> Where(Predicate<TSource> predicate)
                => Where(new ValuePredicateWrapper<TSource>(predicate));

            public ArraySegmentWhereEnumerable<TSource, PredicatePredicateCombination<TPredicate, TPredicate2, TSource>> Where<TPredicate2>(TPredicate2 predicate)
                where TPredicate2 : struct, IPredicate<TSource>
                => Where<TSource, PredicatePredicateCombination<TPredicate, TPredicate2, TSource>>(source, new PredicatePredicateCombination<TPredicate, TPredicate2, TSource>(this.predicate, predicate));
        }
    }
}

