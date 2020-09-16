using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Hyperlinq
{
    public interface IPredicate<TSource>
    {
        bool Invoke([AllowNull] TSource item);
    }
}
