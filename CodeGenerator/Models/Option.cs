namespace CodeGenerator.Models
{
    public class Option
    {
        /// <summary>
        /// Use DryRun for testing without actual file writes
        /// </summary>
        public bool IsDryRun { get; set; } = false;

        /// <summary>
        /// Base directory to write files
        /// </summary>
        public string BaseOutputPath { get; set; } = "./Outputs";

        public void Deconstruct(out bool isDryRun, out string baseOutputPath)
        {
            isDryRun = IsDryRun;
            baseOutputPath = BaseOutputPath;
        }
    }
}