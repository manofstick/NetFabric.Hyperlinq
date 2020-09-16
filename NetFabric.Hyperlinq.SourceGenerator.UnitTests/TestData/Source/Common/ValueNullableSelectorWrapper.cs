using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Hyperlinq
{
    public readonly struct ValueNullableSelectorWrapper<TSource, TResult>
        : INullableSelector<TSource, TResult>
    {
        readonly NullableSelector<TSource, TResult> selector;

        public ValueNullableSelectorWrapper(NullableSelector<TSource, TResult> selector)
            => this.selector = selector;

        [return: MaybeNull]
        public TResult Invoke([AllowNull] TSource item)
            => selector(item);
    }
}
