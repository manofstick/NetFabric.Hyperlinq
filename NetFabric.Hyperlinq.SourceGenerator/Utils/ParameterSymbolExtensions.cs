using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace NetFabric.Hyperlinq.SourceGenerator
{
    static class ParameterSymbolExtensions
    {
        public static string AsParametersString(this ImmutableArray<IParameterSymbol> parameters) 
            => parameters.Length == 0 
                ? string.Empty 
                : parameters.Select(parameter => parameter.Name).ToCommaSeparated();

        public static string AsParametersDeclarationString(this ImmutableArray<IParameterSymbol> parameters, ImmutableArray<(string, string, bool)> genericsMapping)
            => parameters.Length switch
            {
                0 => string.Empty,
                1 => string.Empty,
                _ => parameters
                    .Skip(1)
                    .Select(parameter => {
                        var builder = new StringBuilder($"{parameter.Type.ToDisplayString(genericsMapping)} {parameter.Name}");
                        if (parameter.HasExplicitDefaultValue)
                        {
                            _ = parameter.ExplicitDefaultValue is null
                            ? builder.Append(" = default")
                            : builder.Append($" = {parameter.ExplicitDefaultValue}");
                        }
                        return builder.ToString();
                    })
                    .ToCommaSeparated(),
            };

    }
}
