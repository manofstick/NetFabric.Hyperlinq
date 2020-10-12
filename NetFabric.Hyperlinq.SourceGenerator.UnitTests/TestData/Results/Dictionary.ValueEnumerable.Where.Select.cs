﻿using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class DictionaryBindings
    {
        public readonly partial struct ValueWrapper<TKey, TValue>
        {
            [GeneratedCode("NetFabric.Hyperlinq.SourceGenerator", "1.0.0")]
            [DebuggerNonUserCode]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly NetFabric.Hyperlinq.ValueEnumerableExtensions.WhereEnumerable<NetFabric.Hyperlinq.DictionaryBindings.ValueWrapper<TKey, TValue>, System.Collections.Generic.Dictionary<TKey, TValue>.Enumerator, System.Collections.Generic.KeyValuePair<TKey, TValue>, FunctionWrapper<System.Collections.Generic.KeyValuePair<TKey, TValue>, bool>> Where(System.Func<System.Collections.Generic.KeyValuePair<TKey, TValue>, bool> predicate)
            => NetFabric.Hyperlinq.ValueEnumerableExtensions.Where<NetFabric.Hyperlinq.DictionaryBindings.ValueWrapper<TKey, TValue>, System.Collections.Generic.Dictionary<TKey, TValue>.Enumerator, System.Collections.Generic.KeyValuePair<TKey, TValue>>(this, predicate);

            [GeneratedCode("NetFabric.Hyperlinq.SourceGenerator", "1.0.0")]
            [DebuggerNonUserCode]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public readonly NetFabric.Hyperlinq.ValueEnumerableExtensions.SelectEnumerable<NetFabric.Hyperlinq.DictionaryBindings.ValueWrapper<TKey, TValue>, System.Collections.Generic.Dictionary<TKey, TValue>.Enumerator, System.Collections.Generic.KeyValuePair<TKey, TValue>, TResult, FunctionWrapper<System.Collections.Generic.KeyValuePair<TKey, TValue>, TResult>> Select<TResult>(System.Func<System.Collections.Generic.KeyValuePair<TKey, TValue>, TResult> selector)
            => NetFabric.Hyperlinq.ValueEnumerableExtensions.Select<NetFabric.Hyperlinq.DictionaryBindings.ValueWrapper<TKey, TValue>, System.Collections.Generic.Dictionary<TKey, TValue>.Enumerator, System.Collections.Generic.KeyValuePair<TKey, TValue>, TResult>(this, selector);

        }

    }
}
