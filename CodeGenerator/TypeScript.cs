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
                    return new TypeMetadata(type, GetOption().BaseOutputPath);
                })
                .ToList();

            // 3 - Generate output based on metadata
            var result = GenerateOutputs(metadata);

            return result;
        }

        private List<Output> GenerateOutputs(List<TypeMetadata> typeMetadatas)
        {
            List<Output> outputs = new();

            typeMetadatas.ForEach(x =>
            {
                if (x.IsAnnotated)
                {
                    // 1 - Header Note
                    var content = FileContent.HeaderNotes;

                    // 2 - Populate Imports

                    // 3 - Populate Interface

                    outputs.Add(new(x.FullOutputPath, content));
                }
            });

            return outputs;
        }
    }
}