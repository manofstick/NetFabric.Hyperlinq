using System;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueReadOnlyList
    {
        public static TSource First<TEnumerable, TEnumerator, TSource>(this TEnumerable source)
            where TEnumerable : IValueReadOnlyList<TSource, TEnumerator>
            where TEnumerator : struct, IValueEnumerator<TSource>
        {
            if (source == null) ThrowHelper.ThrowArgumentNullException(nameof(source));
            if (source.Count() == 0) ThrowHelper.ThrowEmptySequence();

            return source[0];
        }

        public static TSource First<TEnumerable, TEnumerator, TSource>(this TEnumerable source, Func<TSource, bool> predicate)
            where TEnumerable : IValueReadOnlyList<TSource, TEnumerator>
            where TEnumerator : struct, IValueEnumerator<TSource>
        {
            if (source == null) ThrowHelper.ThrowArgumentNullException(nameof(source));

            for (var index = 0; index < source.Count(); index++)
            {
                if (predicate(source[index]))
                    return source[index];
            }
            ThrowHelper.ThrowEmptySequence();
            return default;
        }
    }
}
