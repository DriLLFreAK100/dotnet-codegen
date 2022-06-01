using CodeGenerator.Common;

namespace CodeGenerator.Utils
{
    public static class MetadataHelper
    {
        /// <summary>
        /// Dictionary of built-in types for this codegen
        /// </summary>
        private static readonly Dictionary<Type, string> _builtInTypes = new()
        {
            { typeof(byte), TsConstants.Number },
            { typeof(sbyte), TsConstants.Number },
            { typeof(decimal), TsConstants.Number },
            { typeof(double), TsConstants.Number },
            { typeof(float), TsConstants.Number },
            { typeof(int), TsConstants.Number },
            { typeof(uint), TsConstants.Number },
            { typeof(nint), TsConstants.Number },
            { typeof(nuint), TsConstants.Number },
            { typeof(long), TsConstants.Number },
            { typeof(ulong), TsConstants.Number },
            { typeof(short), TsConstants.Number },
            { typeof(ushort), TsConstants.Number },

            { typeof(bool), TsConstants.Boolean },

            { typeof(char), TsConstants.String },
            { typeof(string), TsConstants.String },
            { typeof(DateTime), TsConstants.String },

            { typeof(object), TsConstants.Any },
        };

        /// <summary>
        /// Check if it is built-in types
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBuiltInType(this Type type)
        {
            return _builtInTypes.ContainsKey(type);
        }

        /// <summary>
        /// To distill type to generate.
        /// Should handle case of generics, e.g. List, Dictionary, etc.
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetTargetTypes(this Type type)
        {
            if (type.IsBuiltInType())
            {
                return new();
            }

            // Try add current type
            if (!type.IsGenericType)
            {
                List<Type> result = new() { type };

                var types = type
                    .GetProperties()
                    .Where(p =>
                    {
                        return !p.PropertyType.IsBuiltInType();
                    })
                    .SelectMany(p =>
                    {
                        return p.PropertyType.GetTargetTypes();
                    })
                    .ToList();

                result.AddRange(types);

                return result;
            }

            // Check for generics
            {
                List<Type> result = new();

                List<Type> recurseNonBuiltInType(Type t)
                {
                    return !t.IsBuiltInType() ? GetTargetTypes(t) : new();
                }

                List<Type> deepGetType(Type t)
                {
                    List<Type> types = new();

                    var genericType = t.GetGenericTypeDefinition();
                    var args = t.GetGenericArguments();

                    // Dictionary
                    if (genericType.IsDictionary())
                    {
                        types.AddRange(recurseNonBuiltInType(args[0]));
                        types.AddRange(recurseNonBuiltInType(args[1]));
                    }

                    // Other Enumerables
                    if (genericType.IsList())
                    {
                        types.AddRange(recurseNonBuiltInType(args[0]));
                    }

                    return types;
                }

                result.AddRange(deepGetType(type));

                return result.Distinct().ToList();
            }
        }

        /// <summary>
        /// Check if type is of Dictionary
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDictionary(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        /// <summary>
        /// Check if type is considered a List
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsList(this Type type)
        {
            if (!type.IsGenericType || type.IsDictionary())
            {
                return false;
            }

            return type
                .GetInterfaces()
                .Any(e => e.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}
