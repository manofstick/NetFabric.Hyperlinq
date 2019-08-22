﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace NetFabric.Hyperlinq
{
    public static partial class ValueEnumerator
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<TSource> ToEnumerator<TSource, TEnumerator>(in TEnumerator source, Action<TEnumerator> disposeAction = null)
            where TEnumerator : struct, IValueEnumerator<TSource>
            => new Enumerator<TSource, TEnumerator>(source, disposeAction);

        sealed class Enumerator<TSource, TEnumerator>
            : IEnumerator<TSource>
            where TEnumerator : struct, IValueEnumerator<TSource>
        {
            [SuppressMessage("Style", "IDE0044:Add readonly modifier")]
            TEnumerator source; // do not make readonly

            readonly Action<TEnumerator> disposeAction;

            public Enumerator(in TEnumerator source, Action<TEnumerator> disposeAction = null)
            {
                this.source = source;
                this.disposeAction = disposeAction;
            }

            public TSource Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => source.Current;
            }
            object IEnumerator.Current => source.Current;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => source.MoveNext();

            public void Reset() => throw new NotSupportedException();

            public void Dispose()
            {
                if (disposeAction is object)
                    disposeAction(source);
            }
        }
    }
}