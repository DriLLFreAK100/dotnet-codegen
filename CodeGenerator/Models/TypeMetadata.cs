using CodeGenerator.Common;

namespace CodeGenerator.Models
{
    public class TypeMetadata
	{
        /// <summary>
        /// C# target type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// TypeScript type to generate
        /// </summary>
        public string OutputName { get; set; } = TsConstants.Any;
    }
}
