namespace CodeGenerator.Utils
{
    public static class MetadataHelper
    {
        private static readonly Dictionary<Type, string> _builtInTypes = new()
        {
            { typeof(byte), "number" },
            { typeof(sbyte), "number" },
            { typeof(decimal), "number" },
            { typeof(double), "number" },
            { typeof(float), "number" },
            { typeof(int), "number" },
            { typeof(uint), "number" },
            { typeof(nint), "number" },
            { typeof(nuint), "number" },
            { typeof(long), "number" },
            { typeof(ulong), "number" },
            { typeof(short), "number" },
            { typeof(ushort), "number" },

            { typeof(bool), "boolean" },

            { typeof(char), "string" },
            { typeof(string), "string" },
            { typeof(DateTime), "string" },

            { typeof(object), "any" },
        };

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

