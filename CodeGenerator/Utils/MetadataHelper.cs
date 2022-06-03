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
        /// Get built-in TypeScript type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetBuiltInTsType(this Type type)
        {
            _builtInTypes.TryGetValue(type, out var value);

            if (string.IsNullOrEmpty(value))
            {
                return TsType.Any;
            }

            return value;
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
                    .Where(p => !p.PropertyType.IsBuiltInType())
                    .SelectMany(p => p.PropertyType.GetTargetTypes())
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
        /// Check if a type is Nullable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) is not null;
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

        /// <summary>
        /// To determine whether the type is a custom type
        /// </summary>
        /// <returns></returns>
        public static bool IsCustomType(this Type type)
        {
            if (type.IsBuiltInType())
            {
                return false;
            }

            if (!type.IsGenericType)
            {
                return true;
            }

            var genericType = type.GetGenericTypeDefinition();
            var args = type.GetGenericArguments();

            // Dictionary
            if (genericType.IsDictionary()
                && (args[0].IsCustomType() || args[1].IsCustomType()))
            {
                return true;
            }

            // Other Enumerables
            if (genericType.IsList() && args[0].IsCustomType())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determine target generated types,
        /// i.e. direct class, classes within Dictionary, List, etc.
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetDependencyTargetTypes(this Type type)
        {
            if (!type.IsGenericType)
            {
                return new() { type };
            }

            var genericType = type.GetGenericTypeDefinition();
            var args = type.GetGenericArguments();

            // Dictionary
            if (genericType.IsDictionary())
            {
                List<Type> result = new();

                if (args[0].IsCustomType())
                {
                    result.AddRange(args[0].GetDependencyTargetTypes());
                }

                if (args[1].IsCustomType())
                {
                    result.AddRange(args[1].GetDependencyTargetTypes());
                }

                return result;
            }

            // Other Enumerables
            if (genericType.IsList())
            {
                List<Type> result = new();

                if (args[0].IsCustomType())
                {
                    result.AddRange(args[0].GetDependencyTargetTypes());
                }

                return result;
            }

            return new();
        }

        /// <summary>
        /// Get types that require import from current type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Type> GetTypeDependencies(this Type type)
        {
            return type
                .GetProperties()
                .Where(p => p.PropertyType.IsCustomType())
                .SelectMany(p => p.PropertyType.GetDependencyTargetTypes())
                .ToList();
        }
    }
}
