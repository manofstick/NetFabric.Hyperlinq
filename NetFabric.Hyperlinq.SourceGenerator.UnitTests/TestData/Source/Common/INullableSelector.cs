using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Hyperlinq
{
    public interface INullableSelector<TSource, TResult>
    {
        [return: MaybeNull] TResult Invoke([AllowNull] TSource item);
    }
}
