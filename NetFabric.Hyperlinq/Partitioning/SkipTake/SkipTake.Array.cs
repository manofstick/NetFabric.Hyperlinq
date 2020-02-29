using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class Array
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SkipTakeEnumerable<TSource> SkipTake<TSource>(this TSource[] source, int skipCount, int takeCount)
            => new SkipTakeEnumerable<TSource>(source, skipCount, takeCount);

        public readonly partial struct SkipTakeEnumerable<TSource>
            : IValueReadOnlyList<TSource, SkipTakeEnumerable<TSource>.Enumerator>
            , IList<TSource>
        {
            internal readonly TSource[] source;
            internal readonly int skipCount;
            internal readonly int takeCount;

            internal SkipTakeEnumerable(TSource[] source, int skipCount, int takeCount)
            {
                this.source = source;
                (this.skipCount, this.takeCount) = Utils.SkipTake(source.Length, skipCount, takeCount);
            }

            public readonly int Count
                => takeCount;

            [MaybeNull]
            public readonly TSource this[int index]
            {
                get
                {
                    if (index < 0 || index >= takeCount) 
                        Throw.ArgumentOutOfRangeException(nameof(index));

                    return source[index + skipCount];
                }
            }

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly Enumerator GetEnumerator() => new Enumerator(in this);
            readonly IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => new Enumerator(in this);
            readonly IEnumerator IEnumerable.GetEnumerator() => new Enumerator(in this);

            [MaybeNull]
            TSource IList<TSource>.this[int index]
            {
                get => this[index];
                set => throw new NotImplementedException();
            }

            bool ICollection<TSource>.IsReadOnly  
                => true;

            void ICollection<TSource>.CopyTo(TSource[] array, int arrayIndex) 
            {
                for (var index = 0; index < takeCount; index++)
                    array[index + arrayIndex] = source[index + skipCount];
            }
            void ICollection<TSource>.Add(TSource item) 
                => throw new NotImplementedException();
            void ICollection<TSource>.Clear() 
                => throw new NotImplementedException();
            bool ICollection<TSource>.Contains(TSource item) 
                => Array.Contains<TSource>(source, item, null, skipCount, takeCount);
            bool ICollection<TSource>.Remove(TSource item) 
                => throw new NotImplementedException();
            int IList<TSource>.IndexOf(TSource item)
            {
                var end = skipCount + takeCount;
                for (var index = skipCount; index < end; index++)
                {
                    if (EqualityComparer<TSource>.Default.Equals(source[index], item))
                        return index;
                }
                return -1;
            }
            void IList<TSource>.Insert(int index, TSource item)
                => throw new NotImplementedException();
            void IList<TSource>.RemoveAt(int index)
                => throw new NotImplementedException();

            public struct Enumerator
                : IEnumerator<TSource>
            {
                readonly TSource[] source;
                readonly int end;
                int index;

                internal Enumerator(in SkipTakeEnumerable<TSource> enumerable)
                {
                    source = enumerable.source;
                    end = enumerable.skipCount + enumerable.takeCount;
                    index = enumerable.skipCount - 1;
                }

                [MaybeNull]
                public readonly TSource Current
                    => source[index];
                readonly object? IEnumerator.Current 
                    => source[index];

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool MoveNext() 
                    => ++index < end;

                [ExcludeFromCodeCoverage]
                public readonly void Reset() 
                    => throw new NotSupportedException();

                public readonly void Dispose() { }
            }

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public SkipTakeEnumerable<TSource> Take(int count)
                => Array.SkipTake<TSource>(source, skipCount, Math.Min(takeCount, count));

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool All(Predicate<TSource> predicate)
                => Array.All<TSource>(source, predicate, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool All(PredicateAt<TSource> predicate)
                => Array.All<TSource>(source, predicate, skipCount, takeCount);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any()
                => Array.Any<TSource>(source, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any(Predicate<TSource> predicate)
                => Array.Any<TSource>(source, predicate, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Any(PredicateAt<TSource> predicate)
                => Array.Any<TSource>(source, predicate, skipCount, takeCount);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Contains(TSource value, IEqualityComparer<TSource>? comparer = null)
                => Array.Contains<TSource>(source, value, comparer, skipCount, takeCount);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Array.ArrayWhereEnumerable<TSource> Where(Predicate<TSource> predicate)
                => Array.Where<TSource>(source, predicate, skipCount, takeCount);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Array.ArrayWhereIndexEnumerable<TSource> Where(PredicateAt<TSource> predicate)
                => Array.Where<TSource>(source, predicate, skipCount, takeCount);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Array.ArraySelectEnumerable<TSource, TResult> Select<TResult>(Selector<TSource, TResult> selector)
                => Array.Select<TSource, TResult>(source, selector, skipCount, takeCount);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Array.ArraySelectIndexEnumerable<TSource, TResult> Select<TResult>(SelectorAt<TSource, TResult> selector)
                => Array.Select<TSource, TResult>(source, selector, skipCount, takeCount);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource ElementAt(int index)
                => Array.ElementAt<TSource>(source, index, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [return: MaybeNull]
            public TSource ElementAtOrDefault(int index)
                => Array.ElementAtOrDefault<TSource>(source, index, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Maybe<TSource> TryElementAt(int index)
                => Array.TryElementAt<TSource>(source, index, skipCount, takeCount);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref readonly TSource First()
                => ref Array.First<TSource>(source, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref readonly TSource First(Predicate<TSource> predicate)
                => ref predicate is null
                    ? ref Throw.ArgumentNullExceptionRef<TSource>(nameof(predicate))
                    : ref Array.First<TSource>(source, predicate, skipCount, takeCount);
                [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref readonly TSource First(PredicateAt<TSource> predicate)
                => ref predicate is null
                    ? ref Throw.ArgumentNullExceptionRef<TSource>(nameof(predicate))
                    : ref Array.First<TSource>(source, predicate, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [return: MaybeNull]
            public ref readonly TSource FirstOrDefault()
                => ref Array.FirstOrDefault<TSource>(source, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [return: MaybeNull]
            public ref readonly TSource FirstOrDefault(Predicate<TSource> predicate)
                => ref predicate is null
                    ? ref Throw.ArgumentNullExceptionRef<TSource>(nameof(predicate))
                    : ref Array.FirstOrDefault<TSource>(source, predicate, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [return: MaybeNull]
            public ref readonly TSource FirstOrDefault(PredicateAt<TSource> predicate)
                => ref predicate is null
                    ? ref Throw.ArgumentNullExceptionRef<TSource>(nameof(predicate))
                    : ref Array.FirstOrDefault<TSource>(source, predicate, skipCount, takeCount);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref readonly TSource Single()
                => ref Array.Single<TSource>(source, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref readonly TSource Single(Predicate<TSource> predicate)
                => ref predicate is null
                    ? ref Throw.ArgumentNullExceptionRef<TSource>(nameof(predicate))
                    : ref Array.Single<TSource>(source, predicate, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ref readonly TSource Single(PredicateAt<TSource> predicate)
                => ref predicate is null
                    ? ref Throw.ArgumentNullExceptionRef<TSource>(nameof(predicate))
                    : ref Array.Single<TSource>(source, predicate, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [return: MaybeNull]
            public ref readonly TSource SingleOrDefault()
                => ref Array.SingleOrDefault<TSource>(source, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [return: MaybeNull]
            public ref readonly TSource SingleOrDefault(Predicate<TSource> predicate)
                => ref predicate is null
                    ? ref Throw.ArgumentNullExceptionRef<TSource>(nameof(predicate))
                    : ref Array.SingleOrDefault<TSource>(source, predicate, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [return: MaybeNull]
            public ref readonly TSource SingleOrDefault(PredicateAt<TSource> predicate)
                => ref predicate is null
                    ? ref Throw.ArgumentNullExceptionRef<TSource>(nameof(predicate))
                    : ref Array.SingleOrDefault<TSource>(source, predicate, skipCount, takeCount);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TSource[] ToArray()
                => Array.ToArray<TSource>(source, skipCount, takeCount);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public List<TSource> ToList()
                => Array.ToList<TSource>(source, skipCount, takeCount);

            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Dictionary<TKey, TSource> ToDictionary<TKey>(Selector<TSource, TKey> keySelector)
                => Array.ToDictionary<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Dictionary<TKey, TSource> ToDictionary<TKey>(Selector<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
                => Array.ToDictionary<TSource, TKey>(source, keySelector, comparer, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(Selector<TSource, TKey> keySelector, Selector<TSource, TElement> elementSelector)
                => Array.ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default, skipCount, takeCount);
            [Pure]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(Selector<TSource, TKey> keySelector, Selector<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
                => Array.ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, skipCount, takeCount);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void ForEach(Action<TSource> action)
                => Array.ForEach<TSource>(source, action, skipCount, takeCount);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void ForEach(ActionAt<TSource> action)
                => Array.ForEach<TSource>(source, action, skipCount, takeCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource>(this SkipTakeEnumerable<TSource> source)
            => source.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource>(this SkipTakeEnumerable<TSource> source, Predicate<TSource> predicate)
        {
            if (predicate is null) Throw.ArgumentNullException(nameof(predicate));

            return Array.Count<TSource>(source.source, predicate, source.skipCount, source.takeCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource>(this SkipTakeEnumerable<TSource> source, PredicateAt<TSource> predicate)
        {
            if (predicate is null) Throw.ArgumentNullException(nameof(predicate));

            return Array.Count<TSource>(source.source, predicate, source.skipCount, source.takeCount);
        }
    }
}