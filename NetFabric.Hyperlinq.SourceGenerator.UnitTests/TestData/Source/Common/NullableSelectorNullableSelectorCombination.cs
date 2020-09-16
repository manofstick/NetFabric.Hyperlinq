using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Hyperlinq
{
    public readonly struct NullableSelectorNullableSelectorCombination<TFirstSelector, TSecondSelector, TSource, TMiddle, TResult>
        : INullableSelector<TSource, TResult>
        where TFirstSelector : struct, INullableSelector<TSource, TMiddle>
        where TSecondSelector : struct, INullableSelector<TMiddle, TResult>
    {
        readonly TFirstSelector first;
        readonly TSecondSelector second;

        public NullableSelectorNullableSelectorCombination(TFirstSelector first, TSecondSelector second)
            => (this.first, this.second) = (first, second);

        [return: MaybeNull]
        public TResult Invoke([AllowNull] TSource item)
            => second.Invoke(first.Invoke(item));
    }
}
