using System;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Hyperlinq
{
    public readonly struct ValuePredicateWrapper<TSource>
        : IPredicate<TSource>
    {
        readonly Predicate<TSource> predicate;

        public ValuePredicateWrapper(Predicate<TSource> predicate)
            => this.predicate = predicate;

        public bool Invoke([AllowNull] TSource item)
            => predicate(item);
    }
}
