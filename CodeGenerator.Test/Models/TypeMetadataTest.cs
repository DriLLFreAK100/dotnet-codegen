using CodeGenerator.Attributes;
using CodeGenerator.Models;

namespace CodeGenerator.Test.Models
{
	[GenerateTs]
	public class MetadataMock
	{
    }

	[GenerateTs(Path = "mockPath")]
	public class MetadataMockPath
	{
	}

	[GenerateTs(FileName = "some-name")]
	public class MetadataMockFileName
	{
	}

	[GenerateTs(Path = "mockPath", FileName = "some-name")]
	public class MetadataMockMixed
	{
	}

	[TestClass]
    public class TypeMetadataTest
	{
		private readonly TypeMetadata _metadata = new(typeof(MetadataMock), "./");

		[TestMethod("Should Be Able To Check Is Annotated")]
		public void ShouldBeAbleToCheckIsAnnotated()
		{
			Assert.IsTrue(_metadata.IsAnnotated);
		}

		[TestMethod("Should Be Able To Derive Type Name")]
		public void ShouldBeAbleToDeriveTypeName()
		{
			Assert.IsTrue(_metadata.OutputName == "MetadataMock");
		}

		[TestMethod("Should Be Able To Derive Full File Name")]
		public void ShouldBeAbleToDeriveFullFileName()
		{
			Assert.IsTrue(_metadata.FullOutputPath == $".//metadata-mock.ts");

			TypeMetadata metaPath = new(typeof(MetadataMockPath), "./");
			Assert.IsTrue(metaPath.FullOutputPath == $".//mockPath/metadata-mock-path.ts");

			TypeMetadata metaFileName = new(typeof(MetadataMockFileName), "./");
			Assert.IsTrue(metaFileName.FullOutputPath == $".//some-name.ts");

			TypeMetadata metaMixed = new(typeof(MetadataMockMixed), "./");
			Assert.IsTrue(metaMixed.FullOutputPath == $".//mockPath/some-name.ts");
		}
	}
}

