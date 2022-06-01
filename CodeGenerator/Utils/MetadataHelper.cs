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
            { typeof(byte), TsType.Number },
            { typeof(sbyte), TsType.Number },
            { typeof(decimal), TsType.Number },
            { typeof(double), TsType.Number },
            { typeof(float), TsType.Number },
            { typeof(int), TsType.Number },
            { typeof(uint), TsType.Number },
            { typeof(nint), TsType.Number },
            { typeof(nuint), TsType.Number },
            { typeof(long), TsType.Number },
            { typeof(ulong), TsType.Number },
            { typeof(short), TsType.Number },
            { typeof(ushort), TsType.Number },

            { typeof(bool), TsType.Boolean },

            { typeof(char), TsType.String },
            { typeof(string), TsType.String },
            { typeof(DateTime), TsType.String },

            { typeof(object), TsType.Any },
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

        /// <summary>
        /// Get custom attribute in target type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this Type type)
        {
            var attr = type.GetCustomAttributes(typeof(T), false);

            return attr.Length > 0
                ? (T)attr[0]
                : default;
        }
    }
}
