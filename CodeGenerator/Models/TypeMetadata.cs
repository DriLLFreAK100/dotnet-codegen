using CodeGenerator.Attributes;
using CodeGenerator.Common;
using CodeGenerator.Utils;

namespace CodeGenerator.Models
{
    public class TypeMetadata
    {
        /// <summary>
        /// C# target type
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Base directory
        /// </summary>
        public string BaseOutputPath { get; set; }

        /// <summary>
        /// Output path including base path and filename
        /// </summary>
        public string FullOutputPath
        {
            get
            {
                if (!IsAnnotated)
                {
                    return null;
                }

                var attr = Type.GetCustomAttribute<GenerateTsAttribute>();

                List<string> paths = new() { BaseOutputPath };

                if (!string.IsNullOrEmpty(attr.Path))
                {
                    paths.Add(attr.Path);
                }

                if (!string.IsNullOrEmpty(attr.FileName))
                {
                    paths.Add(attr.FileName);
                }
                else
                {
                    paths.Add($"{Formatter.PascalToKebab(OutputName)}");
                }

                return $"{string.Join("/", paths)}.ts";
            }
        }

        /// <summary>
        /// Is annotated with GenerateTs attribute
        /// </summary>
        public bool IsAnnotated
        {
            get
            {
                return Type.IsDefined(typeof(GenerateTsAttribute), false);
            }
        }

        /// <summary>
        /// TypeScript type name to generate
        /// </summary>
        public string OutputName
        {
            get
            {
                if (!IsAnnotated)
                {
                    return TsType.Any;
                }

                return Type.Name;
            }
        }

        public TypeMetadata(Type type, string relativeBaseOutputPath = "")
        {
            Type = type;
            BaseOutputPath = AppDomain.CurrentDomain.BaseDirectory;

            if (!string.IsNullOrEmpty(relativeBaseOutputPath))
            {
                BaseOutputPath = $"{BaseOutputPath}/{relativeBaseOutputPath}";
            }
        }
    }
}
