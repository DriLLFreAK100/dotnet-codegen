using CodeGenerator.Models;

namespace CodeGenerator.Utils
{
    public static class FileHelper
    {
        /// <summary>
        /// Delete all folders and files in the given directory
        /// </summary>
        /// <param name="path"></param>
        public static void Clear(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// Write the outputs to physical files
        /// </summary>
        /// <param name="outputs"></param>
        public static void WriteOutputs(List<Output> outputs)
        {
            outputs.ForEach(o =>
            {
                var dir = Path.GetDirectoryName(o.Path);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(o.Path, o.Content);
            });
        }
    }
}

