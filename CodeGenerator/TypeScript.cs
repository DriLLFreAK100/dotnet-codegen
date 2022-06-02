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
            //var store = types.ToDictionary(x => x, _ => new Output());

            // 2 - Generate metadata
            var metadata = types
                .SelectMany(type => type.GetTargetTypes())
                .Distinct()
                .Select(type =>
                {
                    return new TypeMetadata(type, GetOption().RelativeBaseOutputPath);
                })
                .ToList();

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

            typeMetadatas.ForEach(x =>
            {
                // Only types annotated with GeneratedTs will be generated
                if (x.IsAnnotated)
                {
                    List<string> content = new()
                    {
                        FileContent.HeaderNotes,
                        GetImportsContent(x, dict),
                    };

                    outputs.Add(new(
                        x.FullOutputPath,
                        string.Join(GetOption().LineSeparator, content)));
                }
            });

            return outputs;
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
                .ToList();

            // Construct import content
            var result = ConstructImportsByMetadata(meta, toImports);

            return result;
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

                return $"import {{ {t.OutputName} }} from '{relativePath}';";
            });

            return string.Join(GetOption().LineSeparator, result);
        }
    }
}