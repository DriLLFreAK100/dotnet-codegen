namespace CodeGenerator.Models
{
    public class Output
    {
        /// <summary>
        /// Physical write path of the file
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Generated file contents
        /// </summary>
        public string Content { get; set; }

        public void Deconstruct(out string path, out string content)
        {
            path = Path;
            content = Content;
        }

        public Output(string path, string content)
        {
            Path = path;
            Content = content;
        }
    }
}