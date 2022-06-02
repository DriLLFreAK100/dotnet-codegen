using System.Reflection;

namespace CodeGenerator.Models
{
    public class Option
    {
        /// <summary>
        /// Use DryRun for testing without actual file writes
        /// </summary>
        public bool IsDryRun { get; set; } = false;

        /// <summary>
        /// Base directory to write files.
        /// This will take precedence over RelativeBaseOutputPath
        /// </summary>
        public string AbsoluteBaseOutputPath { get; set; }

        /// <summary>
        /// Base directory to write files, relative to program execution path
        /// </summary>
        public string RelativeBaseOutputPath { get; set; } = "./Outputs";

        /// <summary>
        /// Line separator. Default to Environment.NewLine
        /// </summary>
        public string LineSeparator { get; set; } = Environment.NewLine;

        /// <summary>
        /// Assemblies involved for the code generation
        /// </summary>
        public List<Assembly> TargetAssemblies { get; set; } = new();

        /// <summary>
        /// Determine the base directory to use.
        /// AbsoluteBaseOutputPath takes precedence over RelativeBaseOutputPath
        /// </summary>
        public string ActiveBaseOutputPath
        {
            get
            {
                if (!string.IsNullOrEmpty(AbsoluteBaseOutputPath))
                {
                    return AbsoluteBaseOutputPath;
                }

                return $"{AppDomain.CurrentDomain.BaseDirectory}/{RelativeBaseOutputPath}";
            }
        }

        public void Deconstruct(out bool isDryRun, out string relativeBaseOutputPath)
        {
            isDryRun = IsDryRun;
            relativeBaseOutputPath = RelativeBaseOutputPath;
        }
    }
}