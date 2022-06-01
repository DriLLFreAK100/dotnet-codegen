using System.Text.RegularExpressions;

namespace CodeGenerator.Utils
{
    public static class Formatter
    {
        /// <summary>
        /// Convert PascalCase to kebab-case
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string PascalToKebab(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return Regex.Replace(
                value,
                "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])",
                "-$1",
                RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }
    }
}

