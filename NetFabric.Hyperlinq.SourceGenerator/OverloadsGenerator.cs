using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NetFabric.Hyperlinq.SourceGenerator
{
    [Generator]
    public class OverloadsGenerator: ISourceGenerator
    {
        static readonly DiagnosticDescriptor UnhandledExceptionError = new DiagnosticDescriptor(
            id: "HPLG001",
            title: "Unhandled exception while generating oveloads",
            messageFormat: "Unhandled exception while generating oveloads: {0}",
            category: "OverloadsGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            //_ = Debugger.Launch(); // uncomment to debug this source generator

            try
            {
                var collectedExtensionMethods = CollectExtensionMethods(context.Compilation);

                var generatedSources = GenerateSource(context.Compilation, collectedExtensionMethods);
                foreach ((var containerClass, var extendedType, var generatedSource) in generatedSources)
                {
                    var hitName = $"{containerClass.OriginalDefinition.MetadataName}.{extendedType.OriginalDefinition.MetadataName}.cs";
                    hitName = hitName.Replace('`', '.');
                    context.AddSource(hitName, SourceText.From(generatedSource, Encoding.UTF8));
                }

            }
            catch (Exception ex)
            {
                context.ReportDiagnostic(Diagnostic.Create(UnhandledExceptionError, Location.None, ex.Message));
            }
        }

        /// <summary>
        /// Collects all the extension methods defined
        /// </summary>
        /// <param name="context"></param>
        /// <returns>A dictionary containing collections of the extension methods per type extended.</returns>
        internal ImmutableDictionary<string, List<MethodInfo>> CollectExtensionMethods(Compilation compilation)
        {
            var result = ImmutableDictionary.CreateBuilder<string, List<MethodInfo>>();

            // go through all implemented static types and get all the extension methods implemented
            var extensionMethods = compilation.SourceModule.GlobalNamespace
                .GetAllTypes()
                .Where(typeSymbol =>
                    typeSymbol.IsStatic
                    && typeSymbol.IsPublic())
                .SelectMany(typeSymbol =>
                    typeSymbol.GetMembers()
                        .OfType<IMethodSymbol>()
                        .Where(methodSymbol =>
                            methodSymbol.IsExtensionMethod
                            && methodSymbol.IsPublic()
                            && !methodSymbol.ShouldIgnore(compilation)));

            // go through all extension methods and store the ones where the extended type is a constrained generic parameter
            foreach (var extensionMethod in extensionMethods)
            {
                var extensionType = extensionMethod.Parameters[0].Type;
                var generic = extensionMethod.TypeParameters
                    .FirstOrDefault(typeParameter 
                        => typeParameter.ConstraintTypes.Length > 0
                        && typeParameter.Name == extensionType.Name);
                if (generic is object)
                {
                    var extendedType = generic.ConstraintTypes[0]; // assume it's the first constraint
                    var key = extendedType.OriginalDefinition.MetadataName;
                    if (!result.TryGetValue(key, out var list))
                    {
                        list = new List<MethodInfo>();
                        result.Add(key, list);
                    }
                    list.Add(extensionMethod.GetInfo(1));
                }
            }

            return result.ToImmutable();
        }

        /// <summary>
        /// Generates the source for the overloads based on the defined extension methods.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="collectedExtensionMethods">A dictionary containing the defined extension methods.</param>
        /// <param name="generatedPath">The path where to serialize the generated code for debugging.</param>
        internal IEnumerable<(INamedTypeSymbol ContainerClass, INamedTypeSymbol ExtendedType, string Source)> GenerateSource(Compilation compilation, ImmutableDictionary<string, List<MethodInfo>> collectedExtensionMethods)
        {
            // go through all public static types
            // the enumerables are defined inside of these
            foreach (var containerClass in compilation.SourceModule.GlobalNamespace
                .GetAllTypes()
                .Where(type =>
                    type.IsStatic
                    && type.IsReferenceType
                    && type.IsPublic()
                    && !type.ShouldIgnore(compilation)))
            {
                // cache a GeneratedCodeAttribute string to use on all generated methods
                var generatorAssembly = GetType().Assembly;
                var generatorAssemblyName = generatorAssembly.GetName().Name;
                var generatorAssemblyVersion = AttributeExtensions.GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>(generatorAssembly)?.InformationalVersion ?? string.Empty;
                var generatedCodeAttribute = $"[GeneratedCode(\"{generatorAssemblyName}\", \"{generatorAssemblyVersion}\")]";

                // get the inner instance types 
                // these can be enumerables
                foreach (var extendedType in
                    containerClass.GetTypeMembers()
                    .OfType<INamedTypeSymbol>()
                    .Where(type => !(type.IsStatic || type.IsInterface())))
                {
                    // check if it's a value enumerable and keep a reference to the implemented interface
                    var valueEnumerableInterface = extendedType.GetAllInterfaces()
                        .FirstOrDefault(@interface => @interface.Name == "IValueEnumerable" || @interface.Name == "IAsyncValueEnumerable");
                    if (valueEnumerableInterface is null)
                        continue;

                    // get the typed of the enumerable, enumerator and source from the generic parameters declaration
                    var enumerableType = extendedType;
                    var enumeratorType = valueEnumerableInterface.TypeArguments[1];
                    var sourceType = valueEnumerableInterface.TypeArguments[0];

                    // get the type mappings from the GeneratorMappingsAttribute, if found.
                    var genericsMapping = ImmutableArray.CreateRange(extendedType.GetGenericMappings(compilation));

                    // get the name and parameter list of all the instance methods declared in this type
                    var implementedInstanceMethods = extendedType.GetMembers().OfType<IMethodSymbol>()
                        .Where(method => method.Name != ".ctor")
                        .Select(method => method.GetInfo())
                        .ToArray();

                    // get the extension methods for this type declared in the outter static type
                    var implementedExtensionMethods = containerClass.GetMembers().OfType<IMethodSymbol>()
                        .Where(method =>
                            method.IsExtensionMethod 
                            && method.Parameters[0].Type.ToDisplayString() == extendedType.ToDisplayString())
                        .Select(method => method.GetInfo(1).ApplyMappings(genericsMapping)) 
                        .ToArray();

                    // join the two lists together as these are the implemented methods for this type
                    // the generated methods will be added to this list
                    var implementedMethods = implementedInstanceMethods.Concat(implementedExtensionMethods)
                        .ToList();

                    var instanceMethods = new List<MethodInfo>();
                    var extensionMethods = new List<MethodInfo>();

                    // go through all the implemented interfaces so that 
                    // the overloads are generated based on the extension methods defined for these
                    var extendedTypeInterfaces = extendedType.AllInterfaces;
                    for (var interfaceIndex = 0; interfaceIndex < extendedTypeInterfaces.Length; interfaceIndex++)
                    {
                        var implementedInterfaceType = extendedTypeInterfaces[interfaceIndex];

                        // get the extension methods collected for this interface
                        var key = implementedInterfaceType.OriginalDefinition.MetadataName;
                        if (!collectedExtensionMethods.TryGetValue(key, out var overloadingMethods))
                            continue;

                        // check which ones should be generated
                        // the method can be already defined by a more performant custom implementation
                        for (var methodIndex = 0; methodIndex < overloadingMethods.Count; methodIndex++)
                        {
                            var overloadingMethod = overloadingMethods[methodIndex];

                            // check if already implemented
                            if (!implementedMethods.Any(method => method.IsOverload(overloadingMethod.ApplyMappings(genericsMapping))))
                            {
                                // check if there's a collision with a property
                                if (extendedType.GetMembers().OfType<IPropertySymbol>()
                                    .Any(property => property.Name == overloadingMethod.Name))
                                {
                                    // this method will be generated as an extension method
                                    extensionMethods.Add(overloadingMethod); 
                                }
                                else
                                {
                                    // this method will generated as an instance method
                                    instanceMethods.Add(overloadingMethod); 
                                }

                                // add to the implemented methods collection
                                implementedMethods.Add(overloadingMethod);
                            }
                        }
                    }

                    // generate the code for the instance methods and extension methods, if any...
                    if (instanceMethods.Count != 0 || extensionMethods.Count != 0)
                    {
                        var builder = new CodeBuilder();
                        _ = builder
                            .AppendLine("using System;")
                            .AppendLine("using System.CodeDom.Compiler;")
                            .AppendLine("using System.Diagnostics;")
                            .AppendLine("using System.Runtime.CompilerServices;")
                            .AppendLine();

                        using (builder.AppendBlock($"namespace NetFabric.Hyperlinq"))
                        {
                            // the generator extends the types by adding partial types
                            // both the outter and the inner types have to be declared as partial
                            using (builder.AppendBlock($"public static partial class {containerClass.Name}"))
                            {
                                // generate the instance methods in the inner type
                                if (instanceMethods.Count != 0)
                                {
                                    var extendedTypeGenericParameters = string.Empty;
                                    if (extendedType.IsGenericType)
                                    {
                                        var parametersDefinition = new StringBuilder();
                                        _ = parametersDefinition.Append($"<{extendedType.TypeParameters.Select(parameter => parameter.ToDisplayString()).ToCommaSeparated()}>");
                                        foreach (var typeParameter in extendedType.TypeParameters.Where(typeParameter => typeParameter.ConstraintTypes.Length != 0))
                                            _ = parametersDefinition.Append($" where {typeParameter.Name} : {typeParameter.AsConstraintsStrings().ToCommaSeparated()}");
                                        extendedTypeGenericParameters = parametersDefinition.ToString();
                                    }

                                    var entity = extendedType.IsValueType
                                        ? "readonly partial struct"
                                        : "partial class"; 
                                    using (builder.AppendBlock($"public {entity} {extendedType.Name}{extendedTypeGenericParameters}"))
                                    {
                                        foreach (var instanceMethod in instanceMethods)
                                        {
                                            GenerateMethodSource(builder, extendedType, instanceMethod, enumerableType, enumeratorType, generatedCodeAttribute, genericsMapping, false);
                                        }
                                    }
                                }
                                _ = builder.AppendLine();

                                // generate the extension methods in the outter type
                                foreach (var extensionMethod in extensionMethods)
                                {
                                    GenerateMethodSource(builder, extendedType, extensionMethod, enumerableType, enumeratorType, generatedCodeAttribute, genericsMapping, true);
                                }
                            }
                        }

                        var source = builder.ToString();
                        yield return (containerClass, extendedType, source);
                    }
                }
            }
        }

        void GenerateMethodSource(CodeBuilder builder, INamedTypeSymbol extendedType, MethodInfo implementedTypeMethod, ITypeSymbol enumerableType, ITypeSymbol enumeratorType, string generatedCodeAttribute, ImmutableArray<(string, string, bool)> genericsMapping, bool isExtensionMethod)
        {
            var typeTypeParameters = extendedType.MappedTypeParameters(genericsMapping)
                .ToArray();
            var typeParameters = implementedTypeMethod.TypeParameters
                .Where(typeParameter => 
                    typeParameter.Name.StartsWith("T") 
                    && typeParameter.Name is not "TEnumerable" and not "TEnumerator" and not "TList" 
                    && !typeTypeParameters.Any(t => typeParameter.Name == t.Name))
                .ToArray();

            var methodReturnType = implementedTypeMethod.ReturnType;
            if (methodReturnType == "TEnumerable")
                methodReturnType = extendedType.ToDisplayString();
            var methodName = implementedTypeMethod.Name;
            var methodExtensionType = extendedType.ToDisplayString();
            var methodParameters = implementedTypeMethod.Parameters
                .Select(parameter => parameter.DefaultValue is null 
                    ? $"{parameter.Type} {parameter.Name}" 
                    : $"{parameter.Type} {parameter.Name} = {parameter.DefaultValue}")
                .ToCommaSeparated();
            var methodGenericParameters = typeParameters.Any() 
                ? $"<{typeParameters.Select(typeParameter => typeParameter.Name).ToCommaSeparated()}>" 
                : string.Empty;

            var returnKeyword = string.Empty;
            var callContainingType = implementedTypeMethod.ContainingType;
            var callTypeParameters = implementedTypeMethod.TypeParameters.Count == 0 
                ? string.Empty 
                : $"<{implementedTypeMethod.TypeParameters.Select(parameter => parameter.Name).ToCommaSeparated()}>";
            var callParameters = implementedTypeMethod.Parameters.Select(parameter => parameter.Name).ToCommaSeparated();

            // generate the source
            _ = builder
                .AppendLine(generatedCodeAttribute)
                .AppendLine("[DebuggerNonUserCode]")
                .AppendLine("[MethodImpl(MethodImplOptions.AggressiveInlining)]");
            if (isExtensionMethod)
            {
                _ = builder.AppendLine($"public static {methodReturnType} {methodName}{methodGenericParameters}(this {methodExtensionType} source{methodParameters})");
            }
            else
            {
                callParameters = string.IsNullOrEmpty(callParameters) 
                    ? "this" 
                    : $"this, {callParameters}";

                var methodReadonly = extendedType.IsValueType ? "readonly" : string.Empty;
                _ = builder.AppendLine($"public {methodReadonly} {methodReturnType} {methodName}{methodGenericParameters}({methodParameters})");
            }
            foreach (var (name, constraints) in typeParameters.Where(typeParameter => typeParameter.Constraints.Any()))
                _ = builder.AppendLine($"where {name} : {constraints}");
            _ = builder
                .AppendLine($"=> {callContainingType}.{methodName}{callTypeParameters}({callParameters});")
                .AppendLine();
        }
    }
}
