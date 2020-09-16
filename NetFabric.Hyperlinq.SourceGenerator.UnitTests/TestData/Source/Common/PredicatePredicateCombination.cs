using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Hyperlinq
{
    public readonly struct PredicatePredicateCombination<TFirstPredicate, TSecondPredicate, TSource>
        : IPredicate<TSource>
        where TFirstPredicate : struct, IPredicate<TSource>
        where TSecondPredicate : struct, IPredicate<TSource>
    {
        readonly TFirstPredicate first;
        readonly TSecondPredicate second;

        public PredicatePredicateCombination(TFirstPredicate first, TSecondPredicate second)
            => (this.first, this.second) = (first, second);

        public bool Invoke([AllowNull] TSource item)
            => first.Invoke(item)
            && second.Invoke(item);
    }
}
