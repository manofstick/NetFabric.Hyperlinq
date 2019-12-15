using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ReadOnlySpan
    {
        [Pure]
        public static ref readonly TSource ElementAt<TSource>(this ReadOnlySpan<TSource> source, int index)
        {
            if (index < 0 || index >= source.Length) ThrowHelper.ThrowArgumentOutOfRangeException<TSource>(nameof(index));

            return ref source[index];
        }

        [Pure]
        static ref readonly TSource ElementAt<TSource>(this ReadOnlySpan<TSource> source, int index, int skipCount, int takeCount)
        {
            if (index < 0 || index >= takeCount) ThrowHelper.ThrowArgumentOutOfRangeException<TSource>(nameof(index));

            return ref source[index + skipCount];
        }

        [Pure]
        [return: MaybeNull]
        public static ref readonly TSource ElementAtOrDefault<TSource>(this ReadOnlySpan<TSource> source, int index)
        {

            if (index < 0 || index >= source.Length) return ref Default<TSource>.Value;

            return ref source[index];
        }

        [Pure]
        [return: MaybeNull]
        static ref readonly TSource ElementAtOrDefault<TSource>(this ReadOnlySpan<TSource> source, int index, int skipCount, int takeCount)
        {

            if (index < 0 || index >= takeCount) return ref Default<TSource>.Value;

            return ref source[index + skipCount];
        }
    }
}