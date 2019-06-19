using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

static class Utils
{
    static readonly string IgnoreAttributeName = "NetFabric.Hyperlinq.IgnoreAttribute";
    static readonly string GenericsMappingAttributeName = "NetFabric.Hyperlinq.GenericsMappingAttribute";
    static readonly string GenericsTypeMappingAttributeName = "NetFabric.Hyperlinq.GenericsTypeMappingAttribute";
    static readonly string ExtensionAttributeName = "System.Runtime.CompilerServices.ExtensionAttribute";

    static readonly string[] BuildCustomAttributeNames = new string[] {
        IgnoreAttributeName,
        GenericsMappingAttributeName,
        GenericsTypeMappingAttributeName,
    };

    public static T GetCustomAttribute<T>(this System.Reflection.Assembly assembly) where T : Attribute
        => Attribute.GetCustomAttribute(assembly, typeof(T), false) as T;

    public static CustomAttribute GetCustomAttribute(this ICustomAttributeProvider attributeProvider, string attributeFullName)
    {
        if (!attributeProvider.HasCustomAttributes)
            return null;

        foreach (var attribute in attributeProvider.CustomAttributes) // avoids enumerator boxing
        {
            if (attribute.AttributeType.FullName == attributeFullName)
                return attribute;
        }

        return null;
    }

    public static void ForEachCustomAttribute(this ICustomAttributeProvider attributeProvider, string attributeFullName, Action<CustomAttribute> action)
    {
        if (!attributeProvider.HasCustomAttributes)
            return;

        foreach (var attribute in attributeProvider.CustomAttributes) // avoids enumerator boxing
        {
            if (attribute.AttributeType.FullName == attributeFullName)
                action(attribute);
        }
    }

    public static bool IsExtensionMethod(this MethodDefinition method)
        => method.HasParameters && method.GetCustomAttribute(ExtensionAttributeName) is object;

    public static bool ShouldBeIgnored(this TypeDefinition type)
    {
        var attribute = type.GetCustomAttribute(IgnoreAttributeName);
        if (attribute is null)
            return false;
        return (bool)attribute.ConstructorArguments[0].Value;
    }

    public static IReadOnlyDictionary<string, string> GenericsMapping(TypeDefinition type)
    {
        var mappings = new Dictionary<string, string>();

        type.ForEachCustomAttribute(GenericsMappingAttributeName, 
            attribute => mappings.Add(
                (string)attribute.ConstructorArguments[0].Value, 
                (string)attribute.ConstructorArguments[1].Value));

        return mappings;
    }

    public static IReadOnlyDictionary<string, TypeReference> GenericsTypeMapping(TypeDefinition type, ModuleDefinition moduleDefinition)
    {
        var mappings = new Dictionary<string, TypeReference>();

        type.ForEachCustomAttribute(GenericsTypeMappingAttributeName,
            attribute => mappings.Add(
                (string)attribute.ConstructorArguments[0].Value,
                (TypeReference)attribute.ConstructorArguments[1].Value));

        return mappings;
    }

    public static TypeReference ResolveGenericType(TypeReference type, MethodDefinition method, IReadOnlyDictionary<string, string> genericsMapping = null)
    {
        switch (type)
        {
            case ArrayType arrayType:
                return new ArrayType(ResolveGenericType(arrayType.ElementType, method, genericsMapping));

            case GenericParameter _:
                var paramName = type.Name;

                if (genericsMapping is object && genericsMapping.TryGetValue(paramName, out var mappedParamName))
                    paramName = mappedParamName;

                var genericParameter = method.GenericParameters.FirstOrDefault(param => param.Name == paramName);
                if (genericParameter is object)
                    return genericParameter;

                return type;

            case GenericInstanceType genericInstance:
                var temp = new GenericInstanceType(genericInstance.ElementType);
                foreach (var param in genericInstance.GenericArguments)
                    temp.GenericArguments.Add(ResolveGenericType(param, method, genericsMapping));
                return temp;

            default:
                return type;
        }
    }


    public static TypeReference ResolveGenericType(TypeReference type, TypeDefinition declaringType, IReadOnlyDictionary<string, string> genericsMapping, IReadOnlyDictionary<string, TypeReference> genericsTypeMapping)
    {
        switch (type)
        {
            case ArrayType arrayType:
                return new ArrayType(ResolveGenericType(arrayType.ElementType, declaringType, genericsMapping, genericsTypeMapping));

            case GenericParameter _:
                var paramName = type.Name;

                if (genericsTypeMapping.TryGetValue(paramName, out var mappedParamType))
                    return ResolveGenericType(mappedParamType, declaringType, genericsMapping, genericsTypeMapping);

                if (genericsMapping.TryGetValue(paramName, out var mappedParamName))
                    paramName = mappedParamName;

                if (declaringType.HasGenericParameters)
                {
                    foreach (var genericParameter in declaringType.GenericParameters)
                    {
                        if (genericParameter.Name == paramName)
                            return genericParameter;
                    }
                }

                return type;

            case GenericInstanceType genericInstance:
                var temp = new GenericInstanceType(genericInstance.ElementType);
                if (genericInstance.HasGenericArguments)
                {
                    foreach (var param in genericInstance.GenericArguments)
                    {
                        temp.GenericArguments.Add(ResolveGenericType(param, declaringType, genericsMapping, genericsTypeMapping));
                    }
                }
                return temp;

            default:
                return type;
        }
    }

    public static void RemoveBuildAttributes(this ICustomAttributeProvider definition)
    {
        if (!definition.HasCustomAttributes)
            return;

        var customAttributes = definition.CustomAttributes;
        var index = 0;
        while (index < customAttributes.Count)
        {
            var attribute = customAttributes[index];
            if (BuildCustomAttributeNames.Contains(attribute.AttributeType.FullName))
            {
                customAttributes.RemoveAt(index);
            }
            else
            {
                index++;
            }
        }
    }

    public static bool IsSame(this MethodDefinition left, MethodDefinition right)
        => left.Name == right.Name &&
            left.HasParameters == right.HasParameters &&
            IsSame(left.Parameters, right.Parameters);

    static bool IsSame(Collection<ParameterDefinition> left, Collection<ParameterDefinition> right)
    {
        if (left.Count != right.Count)
            return false;

        var count = left.Count;
        for (var index = 0; index < count; index++)
        {
            var leftParameter = left[index];
            var rightParameter = right[index];

            if (leftParameter.ParameterType.FullName != rightParameter.ParameterType.FullName ||
                leftParameter.IsOut != rightParameter.IsOut)
                return false;
        }

        return true;
    }

    public static void LoadArg(ILProcessor processor, int index)
    {
        switch (index)
        {
            case 0:
                processor.Emit(OpCodes.Ldarg_0);
                break;
            case 1:
                processor.Emit(OpCodes.Ldarg_1);
                break;
            case 2:
                processor.Emit(OpCodes.Ldarg_2);
                break;
            case 3:
                processor.Emit(OpCodes.Ldarg_3);
                break;
            default:
                processor.Emit(OpCodes.Ldarg, index);
                break;
        }
    }
}