using CodeGenerator.Utils;

namespace CodeGenerator.Test.Utils
{
	[TestClass]
    public class FormatterTest
	{
		[TestMethod("Should Be Able To Convert Pascal To Kebab")]
		public void ShouldBeAbleToConvertPascalToKebab()
		{
			Assert.IsTrue(Formatter.PascalToKebab("Test") == "test");
			Assert.IsTrue(Formatter.PascalToKebab("UnitTest") == "unit-test");
			Assert.IsTrue(Formatter.PascalToKebab("UnitTestAgain") == "unit-test-again");
			Assert.IsTrue(Formatter.PascalToKebab("UnitTest1") == "unit-test-1");
		}
	}
}

