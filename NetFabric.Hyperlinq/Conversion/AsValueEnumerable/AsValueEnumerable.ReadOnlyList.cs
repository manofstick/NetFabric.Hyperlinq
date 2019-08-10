﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ReadOnlyList
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsValueEnumerableEnumerable<TSource> AsValueEnumerable<TSource>(this IReadOnlyList<TSource> source)
           => new AsValueEnumerableEnumerable<TSource>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueEnumerableWrapper<TEnumerable, TEnumerator, TSource> AsValueEnumerable<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TEnumerable, TEnumerator> getEnumerator)
            where TEnumerable : IReadOnlyList<TSource>
            where TEnumerator : struct, IEnumerator<TSource>
            => new ValueEnumerableWrapper<TEnumerable, TEnumerator, TSource>(source, getEnumerator);

        [GenericsTypeMapping("TEnumerable", typeof(ValueEnumerableWrapper<,,>))]
        public readonly struct ValueEnumerableWrapper<TEnumerable, TEnumerator, TSource>
            : IValueReadOnlyList<TSource, TEnumerator>
            where TEnumerable : IReadOnlyList<TSource>
            where TEnumerator : struct, IEnumerator<TSource>
        {
            readonly TEnumerable source;
            readonly Func<TEnumerable, TEnumerator> getEnumerator;

            internal ValueEnumerableWrapper(in TEnumerable source, Func<TEnumerable, TEnumerator> getEnumerator)
            {
                this.source = source;
                this.getEnumerator = getEnumerator;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public TEnumerator GetEnumerator() => getEnumerator(source);
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => getEnumerator(source);
            IEnumerator IEnumerable.GetEnumerator() => getEnumerator(source);

            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => source.Count;
            }

            public TSource this[int index]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => source[index];
            }
        }

        [GenericsTypeMapping("TEnumerable", typeof(AsValueEnumerableEnumerable<>))]
        [GenericsTypeMapping("TEnumerator", typeof(AsValueEnumerableEnumerable<>.Enumerator))]
        public readonly struct AsValueEnumerableEnumerable<TSource>
            : IValueReadOnlyList<TSource, AsValueEnumerableEnumerable<TSource>.Enumerator>
        {
            readonly IReadOnlyList<TSource> source;

            internal AsValueEnumerableEnumerable(IReadOnlyList<TSource> source)
            {
                this.source = source;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator GetEnumerator() => new Enumerator(source);
            IEnumerator<TSource> IEnumerable<TSource>.GetEnumerator() => new Enumerator(source);
            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(source);

            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => source.Count;
            }

            public TSource this[int index]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => source[index];
            }

            public struct Enumerator
                : IEnumerator<TSource>
            {
                IEnumerator<TSource> enumerator;

                internal Enumerator(IReadOnlyList<TSource> enumerable)
                {
                    enumerator = enumerable.GetEnumerator();
                }

                public TSource Current
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get => enumerator.Current;
                }
                object IEnumerator.Current => enumerator.Current;

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool MoveNext() => enumerator.MoveNext();

                void IEnumerator.Reset() => throw new NotSupportedException();

                public void Dispose() => enumerator.Dispose();
            }
        }
    }
}
