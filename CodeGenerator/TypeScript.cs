using CodeGenerator.Attributes;
using CodeGenerator.Common;
using CodeGenerator.Models;
using CodeGenerator.Utils;

namespace CodeGenerator
{
    public class TypeScript : BaseGenerator
    {
        public TypeScript(Option option) : base(option) { }

        public override List<Output> GenerateMetadata()
        {
            // 1 - Get all annotated
            var types = GetAnnotatedTypes<GenerateTsAttribute>();

            // 2 - Generate metadata
            var metadata = GetTypeMetadatas(types);

            // 3 - Generate output based on metadata
            var result = GenerateOutputs(metadata);

            return result;
        }

        /// <summary>
        /// Generate the list of codegen output
        /// </summary>
        /// <param name="typeMetadatas"></param>
        /// <returns></returns>
        private List<Output> GenerateOutputs(List<TypeMetadata> typeMetadatas)
        {
            List<Output> outputs = new();
            var dict = typeMetadatas.ToDictionary(x => x.Type, x => x);

            return typeMetadatas
                .Where(t => t.IsAnnotated)
                .Select(t =>
                {
                    var content = new List<string>()
                    {
                        FileContent.HeaderNotes,
                        GetImportsContent(t, dict),
                        GetTypeContent(t, dict) + GetOption().LineSeparator,
                    }
                    .Where(c => !string.IsNullOrWhiteSpace(c));

                    return new Output(
                        t.FullOutputPath,
                        string.Join(GetOption().LineSeparator, content));
                })
                .ToList();
        }

        /// <summary>
        /// Generate metadata list
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        private List<TypeMetadata> GetTypeMetadatas(List<Type> types)
        {
            return types
                .SelectMany(type => type.GetTargetTypes())
                .Distinct()
                .Select(type =>
                {
                    return new TypeMetadata(type, GetOption().ActiveBaseOutputPath);
                })
                .ToList();
        }

        /// <summary>
        /// Retrieve content for meta type
        /// </summary>
        /// <param name="meta"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        private string GetTypeContent(TypeMetadata meta, Dictionary<Type, TypeMetadata> dict)
        {
            if (meta.Type.IsEnum)
            {
                return GetEnumContent(meta);
            }

            return GetInterfaceContent(meta, dict);
        }

        /// <summary>
        /// Retrieve enum content
        /// </summary>
        /// <param name="meta"></param>
        /// <returns></returns>
        private string GetEnumContent(TypeMetadata meta)
        {
            List<string> content = new();

            content.Add($"export enum {meta.OutputName} {{");

            foreach (var value in Enum.GetValues(meta.Type))
            {
                content.Add($"  {value} = {(int)value},");
            }

            content.Add("}");

            return string.Join(GetOption().LineSeparator, content);
        }

        /// <summary>
        /// Retrive interface content
        /// </summary>
        /// <param name="meta"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        private string GetInterfaceContent(TypeMetadata meta, Dictionary<Type, TypeMetadata> dict)
        {
            List<string> content = new();

            content.Add($"export interface {meta.OutputName} {{");

            content.AddRange(meta.Type.GetProperties().Select(p =>
            {
                var t = p.PropertyType;
                var fieldName = p.Name.PascalToCamel();

                // Built-in Types
                if (t.IsBuiltInType())
                {
                    return $"  {fieldName}: {t.GetBuiltInTsType()};";
                }

                // Objects
                return $"  {fieldName}: {GetTsTypeForObject(t, dict)};";
            }));

            content.Add("}");

            return string.Join(GetOption().LineSeparator, content);
        }

        /// <summary>
        /// Retrieve imports content
        /// </summary>
        /// <param name="meta"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        private string GetImportsContent(TypeMetadata meta, Dictionary<Type, TypeMetadata> dict)
        {
            Func<Type, TypeMetadata> getTypeMetadata = (source) =>
            {
                dict.TryGetValue(source, out var target);
                return target;
            };

            // Construct Dependencies' metadata
            var toImports = meta.Type
                .GetTypeDependencies()
                .Where(t => getTypeMetadata(t).IsAnnotated)
                .Select(t => getTypeMetadata(t))
                .DistinctBy(t => t.Type)
                .ToList();

            // Construct import content
            var result = ConstructImportsByMetadata(meta, toImports);

            return string.IsNullOrEmpty(result.Trim())
                ? result
                : result + GetOption().LineSeparator;
        }

        /// <summary>
        /// Construct imports content based on metadata received
        /// </summary>
        /// <param name="main"></param>
        /// <param name="imports"></param>
        /// <returns></returns>
        private string ConstructImportsByMetadata(
            TypeMetadata main,
            List<TypeMetadata> imports)
        {
            Uri from = new(main.FullOutputPath);

            var result = imports.Select(t =>
            {
                // Compute relative path
                Uri to = new(t.FullOutputPath);
                var relativeUri = from.MakeRelativeUri(to);
                string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

                // Append ./ for same directory
                if (!relativePath.StartsWith("."))
                {
                    relativePath = $"./{relativePath}";
                }

                // Remove file extension
                relativePath = relativePath[0..^3];

                return $"import {{ {t.OutputName} }} from '{relativePath}';";
            });

            return string.Join(GetOption().LineSeparator, result);
        }

        /// <summary>
        /// Get TypeScript type content for Object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        private string GetTsTypeForObject(Type type, Dictionary<Type, TypeMetadata> dict)
        {
            // Built-In Types
            if (type.IsBuiltInType())
            {
                return type.GetBuiltInTsType();
            }

            // Nullable
            if (type.IsNullable())
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return $"{GetTsTypeForObject(underlyingType, dict)} | null";
            }

            // Tracked Meta
            if (dict.TryGetValue(type, out var trackedType))
            {
                return $"{trackedType.OutputName}";
            }

            // Dictionary
            if (type.IsDictionary())
            {
                var args = type.GetGenericArguments();

                var key = GetTsTypeForObject(args[0], dict);
                var value = GetTsTypeForObject(args[1], dict);

                return $"{{ [key: {key}]: {value} }}";
            }

            // List Types
            if (type.IsList())
            {
                var args = type.GetGenericArguments();
                return $"{GetTsTypeForObject(args[0], dict)}[]";
            }

            // Other Objects
            return TsType.Any;
        }
    }
}
