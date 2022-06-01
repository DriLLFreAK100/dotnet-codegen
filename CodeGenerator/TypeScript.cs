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

            // 2 - Get recursively all subtypes
            types = types
                .SelectMany(type => type.GetTargetTypes())
                .Distinct()
                .ToList();

            // 3 - Generate metadata
            List<TypeMetadata> metadata = types.Select(type =>
            {
                return new TypeMetadata(type, GetOption().BaseOutputPath);
            }).ToList();

            // 4 - Generate output based on metadata


            return new();
        }
    }
}