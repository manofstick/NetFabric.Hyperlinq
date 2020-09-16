using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Hyperlinq
{
    [return: MaybeNull] public delegate TResult NullableSelector<in TSource, TResult>([AllowNull] TSource obj);
}
