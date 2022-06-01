using CodeGenerator.Attributes;
using CodeGenerator.Test.Mocks;
using CodeGenerator.Utils;

namespace CodeGenerator.Test.Utils
{ 
	[TestClass]
    public class MetadataHelperTest
	{
		[TestMethod("Should Be Able To Check If It Is A Dictionary")]
		public void ShouldBeAbleToCheckIfItIsADictionary()
		{
			Dictionary<string, int> dict = new();
			Assert.IsTrue(dict.GetType().IsDictionary());

			List<string> list = new();
			Assert.IsFalse(list.GetType().IsDictionary());

			string str = string.Empty;
			Assert.IsFalse(str.GetType().IsDictionary());
		}

		[TestMethod("Should Be Able To Check If It Is A List")]
		public void ShouldBeAbleToCheckIfItIsAList()
		{
			List<string> list = new();
			Assert.IsTrue(list.GetType().IsList());

			Dictionary<string, int> dict = new();
			Assert.IsFalse(dict.GetType().IsList());

			string str = string.Empty;
			Assert.IsFalse(str.GetType().IsList());
		}

		[TestMethod("Should Be Able To Consolidate Types For List")]
		public void ShouldBeAbleToConsolidateTypesForList()
		{
			var types = typeof(ListDto).GetTargetTypes();

			Assert.IsTrue(types.Any(t => t == typeof(ListDto)));
			Assert.IsTrue(types.Any(t => t == typeof(ChildDto)));
		}

		[TestMethod("Should Be Able To Consolidate Types For Dictionary")]
		public void ShouldBeAbleToConsolidateTypesForDictionary()
		{
			var types = typeof(DictionaryDto).GetTargetTypes();

			Assert.IsTrue(types.Any(t => t == typeof(DictionaryDto)));
			Assert.IsTrue(types.Any(t => t == typeof(ChildDto)));
		}

		[TestMethod("Should Be Able To Consolidate Types For Object")]
		public void ShouldBeAbleToConsolidateTypesForObject()
		{
			var types = typeof(ObjectDto).GetTargetTypes();

			Assert.IsTrue(types.Any(t => t == typeof(ObjectDto)));
			Assert.IsTrue(types.Any(t => t == typeof(ChildDto)));
		}

		[TestMethod("Should Be Able To Consolidate For Grandchild Types")]
		public void ShouldBeAbleToConsolidateForGrandchildTypes()
		{
			var types = typeof(ObjectDto).GetTargetTypes();

			Assert.IsTrue(types.Any(t => t == typeof(ObjectDto)));
			Assert.IsTrue(types.Any(t => t == typeof(ChildDto)));
			Assert.IsTrue(types.Any(t => t == typeof(GrandChildDto)));
		}

		[TestMethod("Should Be Able To Get GenerateTs Attribute")]
		public void ShouldBeAbleToGetGenerateTsAttribute()
		{
			var attr = typeof(ObjectDto).GetCustomAttribute<GenerateTsAttribute>();
			Assert.IsNotNull(attr);
			Assert.IsTrue(attr.GetType() == typeof(GenerateTsAttribute));
		}

		[TestMethod("Should Be Able To Detect Custom Type")]
		public void ShouldBeAbleToDetectCustomType()
		{
			Assert.IsFalse(typeof(int).IsCustomType());
			Assert.IsFalse(typeof(List<int>).IsCustomType());
			Assert.IsTrue(typeof(List<ObjectDto>).IsCustomType());
			Assert.IsTrue(typeof(ObjectDto).IsCustomType());
		}

		[TestMethod("Should Be Able To Get Type Dependencies")]
		public void ShouldBeAbleToGetTypeDependencies()
		{
			var deps = typeof(ObjectDto).GetTypeDependencies();
			Assert.IsTrue(deps.Contains(typeof(ChildDto)));
		}
	}
}
