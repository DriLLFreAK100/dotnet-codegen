using System.Reflection;
using CodeGenerator.Attributes;
using CodeGenerator.Models;

namespace CodeGenerator.Test.Utils
{
    [GenerateTs]
    public class AnnotatedType
    {
        public string Id { get; set; }
    }

    public class DummyGenerator : BaseGenerator
    {
        public DummyGenerator(Option option) : base(option) { }

        public override List<Output> GenerateMetadata()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class BaseGeneratorTest
    {
        [TestMethod("Should be able to get annotated types")]
        public void ShouldBeAbleToGetAnnotatedTypes()
        {
            var types = new DummyGenerator(new Option()
            {
                IsDryRun = true,
                TargetAssemblies = new()
                {
                    Assembly.GetAssembly(typeof(TypeScript)),
                }
            }).GetAnnotatedTypes<GenerateTsAttribute>();

            Assert.IsTrue(types.Any());
        }
    }
}

