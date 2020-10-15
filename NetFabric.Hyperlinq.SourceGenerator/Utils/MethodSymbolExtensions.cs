using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace NetFabric.Hyperlinq.SourceGenerator
{
    static class MethodSymbolExtensions
    {
        public static string ToDisplayString(this IMethodSymbol method, ITypeSymbol enumerableType, ITypeSymbol enumeratorType, ImmutableArray<(string, string, bool)> genericsMapping)
            => $"{method.Name}{method.TypeArguments.AsTypeArgumentsString(enumerableType, enumeratorType, genericsMapping)}";

        public static IEnumerable<ITypeParameterSymbol> GetTypeParameterSymbols(this IMethodSymbol methodSymbol)
        {
            var typeArguments = methodSymbol.TypeArguments;
            for (var index = 0; index < typeArguments.Length; index++)
            {
                var typeArgument = typeArguments[index];
                if (typeArgument is ITypeParameterSymbol typeParameter)
                    yield return typeParameter;
            }
        }
    }
}
