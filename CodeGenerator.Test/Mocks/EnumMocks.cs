using CodeGenerator.Attributes;

namespace CodeGenerator.Test.Mocks
{
	[GenerateTs]
    public enum LanguageType
	{
		CSharp = 1,
		Java = 2,
		JavaScript = 4,
		Others = 8,
	}

	[GenerateTs]
	public class ClassWithEnum
	{
        public LanguageType LanguageType { get; set; }
    }
}
