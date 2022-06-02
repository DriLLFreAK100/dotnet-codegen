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

        private List<Output> GenerateOutputs(List<TypeMetadata> typeMetadatas)
        {
            List<Output> outputs = new();
            var dict = typeMetadatas.ToDictionary(x => x.Type, x => x);

            typeMetadatas.ForEach(x =>
            {
                if (x.IsAnnotated)
                {
                    List<string> content = new();

                    // 1 - Header Note
                    content.Add(FileContent.HeaderNotes);

                    // 2 - Populate Imports
                    var toImports = x.Type.GetTypeDependencies().Where(t =>
                    {
                        dict.TryGetValue(t, out var type);
                        return type.IsAnnotated;
                    })
                    .Select(t =>
                    {
                        dict.TryGetValue(t, out var type);
                        return type;
                    })
                    .ToList();

                    content.Add(ConstructImports(x, toImports));

                    // 3 - Populate Interface
                    // TODO...

                    outputs.Add(new(
                        x.FullOutputPath,
                        string.Join(GetOption().LineSeparator, content)));
                }
            });

            return outputs;
        }

        private string ConstructImports(
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