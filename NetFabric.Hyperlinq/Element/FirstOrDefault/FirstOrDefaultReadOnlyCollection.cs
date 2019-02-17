using System;
using System.Collections.Generic;

namespace NetFabric.Hyperlinq
{
    public static partial class ReadOnlyCollection
    {
        public static TSource FirstOrDefault<TReadOnlyCollection, TEnumerator, TSource>(this TReadOnlyCollection source) 
            where TReadOnlyCollection : IReadOnlyCollection<TSource>
            where TEnumerator : IEnumerator<TSource>
        {
            if (source == null) ThrowHelper.ThrowArgumentNullException(nameof(source));
            if (source.Count == 0) return default;

            using (var enumerator = (TEnumerator)source.GetEnumerator())
            {
                enumerator.MoveNext();
                return enumerator.Current;
            }
        }
    }
}
