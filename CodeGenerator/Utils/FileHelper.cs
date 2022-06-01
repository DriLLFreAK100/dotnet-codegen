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
			Directory.Delete(path, true);
		}

		/// <summary>
        /// Write the outputs to physical files
        /// </summary>
        /// <param name="outputs"></param>
		public static void WriteOutputs(List<Output> outputs)
		{
			outputs.ForEach(o =>
			{
				if (Directory.Exists(o.Path))
				{
					File.WriteAllText(o.Path, o.Content);
				}
			});
		}
	}
}

