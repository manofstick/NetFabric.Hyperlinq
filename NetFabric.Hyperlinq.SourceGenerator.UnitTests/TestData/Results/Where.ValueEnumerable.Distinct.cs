﻿using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerableExtensions
    {
        public readonly partial struct WhereEnumerable<TEnumerable, TEnumerator, TSource, TPredicate> where TEnumerable : notnull, NetFabric.Hyperlinq.IValueEnumerable<TSource, TEnumerator> where TEnumerator : struct, System.Collections.Generic.IEnumerator<TSource> where TPredicate : struct, IFunction<TSource, bool>
        {
            [GeneratedCode("NetFabric.Hyperlinq.SourceGenerator", "1.0.0")]
            [DebuggerNonUserCode]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly NetFabric.Hyperlinq.ValueEnumerableExtensions.WhereEnumerable<NetFabric.Hyperlinq.ValueEnumerableExtensions.WhereEnumerable<TEnumerable, TEnumerator, TSource, TPredicate>, NetFabric.Hyperlinq.ValueEnumerableExtensions.WhereEnumerable<TEnumerable, TEnumerator, TSource, TPredicate>.DisposableEnumerator, TSource, TPredicate> Where(TPredicate predicate)
            => NetFabric.Hyperlinq.ValueEnumerableExtensions.Where<NetFabric.Hyperlinq.ValueEnumerableExtensions.WhereEnumerable<TEnumerable, TEnumerator, TSource, TPredicate>, NetFabric.Hyperlinq.ValueEnumerableExtensions.WhereEnumerable<TEnumerable, TEnumerator, TSource, TPredicate>.DisposableEnumerator, TSource, TPredicate>(this, predicate);

            [GeneratedCode("NetFabric.Hyperlinq.SourceGenerator", "1.0.0")]
            [DebuggerNonUserCode]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly NetFabric.Hyperlinq.ValueEnumerableExtensions.DistinctEnumerable<NetFabric.Hyperlinq.ValueEnumerableExtensions.WhereEnumerable<TEnumerable, TEnumerator, TSource, TPredicate>, NetFabric.Hyperlinq.ValueEnumerableExtensions.WhereEnumerable<TEnumerable, TEnumerator, TSource, TPredicate>.DisposableEnumerator, TSource> Distinct()
            => NetFabric.Hyperlinq.ValueEnumerableExtensions.Distinct<NetFabric.Hyperlinq.ValueEnumerableExtensions.WhereEnumerable<TEnumerable, TEnumerator, TSource, TPredicate>, NetFabric.Hyperlinq.ValueEnumerableExtensions.WhereEnumerable<TEnumerable, TEnumerator, TSource, TPredicate>.DisposableEnumerator, TSource>(this);

        }

    }
}
