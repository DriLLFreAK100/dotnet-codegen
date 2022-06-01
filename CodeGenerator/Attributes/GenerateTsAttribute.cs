namespace CodeGenerator.Attributes
{
    public class GenerateTsAttribute: Attribute
	{
		/// <summary>
        /// Path to generate
        /// </summary>
		public string Path { get; set; }

        /// <summary>
        /// File name for the generated file
        /// </summary>
        public string FileName { get; set; }
    }
}
