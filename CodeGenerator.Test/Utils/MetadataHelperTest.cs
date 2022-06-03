using CodeGenerator.Attributes;
using CodeGenerator.Common;
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

		[TestMethod("Should Be Able To Check If It Is Nullable")]
		public void ShouldBeAbleToCheckIfItIsNullable()
		{
			Assert.IsTrue(typeof(int?).IsNullable());
			Assert.IsFalse(typeof(string).IsNullable());
			Assert.IsFalse(typeof(ObjectDto).IsNullable());
			Assert.IsFalse(typeof(List<int>).IsNullable());
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

		[TestMethod("Should Be Able To Get Type Dependencies In List")]
		public void ShouldBeAbleToGetTypeDependenciesInList()
		{
			var deps = typeof(ListDto).GetTypeDependencies();
			Assert.IsTrue(deps.Contains(typeof(ChildDto)));
		}

		[TestMethod("Should Be Able To Get Type Dependencies In Dictionary")]
		public void ShouldBeAbleToGetTypeDependenciesInDictionary()
		{
			var deps = typeof(DictionaryDto).GetTypeDependencies();
			Assert.IsTrue(deps.Contains(typeof(ChildDto)));
		}

		[TestMethod("Should Be Able To Get Dependency Target Types")]
		public void ShouldBeAbleToGetDependencyTargetTypes()
		{
			Assert.IsTrue(typeof(ObjectDto).GetDependencyTargetTypes().Contains(typeof(ObjectDto)));

			Assert.IsTrue(typeof(List<ObjectDto>).GetDependencyTargetTypes().Contains(typeof(ObjectDto)));

			Assert.IsTrue(typeof(Dictionary<int, ObjectDto>).GetDependencyTargetTypes().Contains(typeof(ObjectDto)));

			Assert.IsTrue(typeof(Dictionary<ObjectDto, int>).GetDependencyTargetTypes().Contains(typeof(ObjectDto)));

			var multiTypes = typeof(Dictionary<ObjectDto, ListDto>).GetDependencyTargetTypes();
			Assert.IsTrue(multiTypes.Contains(typeof(ObjectDto)));
			Assert.IsTrue(multiTypes.Contains(typeof(ListDto)));
		}

		[TestMethod("Should Be Get Built-in Ts Types")]
		public void ShouldBeAbleToGetBuiltInTsTypes()
		{
			Assert.IsTrue(typeof(DictionaryDto).GetBuiltInTsType() == TsType.Any);
			Assert.IsTrue(typeof(object).GetBuiltInTsType() == TsType.Any);

			Assert.IsTrue(typeof(byte).GetBuiltInTsType() == TsType.Number);
			Assert.IsTrue(typeof(sbyte).GetBuiltInTsType() == TsType.Number);
			Assert.IsTrue(typeof(decimal).GetBuiltInTsType() == TsType.Number);
			Assert.IsTrue(typeof(double).GetBuiltInTsType() == TsType.Number);
			Assert.IsTrue(typeof(float).GetBuiltInTsType() == TsType.Number);
			Assert.IsTrue(typeof(int).GetBuiltInTsType() == TsType.Number);
			Assert.IsTrue(typeof(uint).GetBuiltInTsType() == TsType.Number);
			Assert.IsTrue(typeof(nint).GetBuiltInTsType() == TsType.Number);
			Assert.IsTrue(typeof(nuint).GetBuiltInTsType() == TsType.Number);
			Assert.IsTrue(typeof(long).GetBuiltInTsType() == TsType.Number);
			Assert.IsTrue(typeof(ulong).GetBuiltInTsType() == TsType.Number);
			Assert.IsTrue(typeof(short).GetBuiltInTsType() == TsType.Number);
			Assert.IsTrue(typeof(ushort).GetBuiltInTsType() == TsType.Number);

			Assert.IsTrue(typeof(bool).GetBuiltInTsType() == TsType.Boolean);

			Assert.IsTrue(typeof(char).GetBuiltInTsType() == TsType.String);
			Assert.IsTrue(typeof(string).GetBuiltInTsType() == TsType.String);
			Assert.IsTrue(typeof(DateTime).GetBuiltInTsType() == TsType.String);
		}
	}
}
