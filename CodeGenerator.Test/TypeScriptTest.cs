using System.Reflection;
using CodeGenerator.Models;

namespace CodeGenerator.Test;

[TestClass]
public class TypeScriptTest
{
    [TestMethod("Should be able to generate metadata")]
    public void ShouldBeAbleToGenerateMetadata()
    {
        var metadata = new TypeScript(new Option()
        {
            IsDryRun = true,
            TargetAssemblies = new()
            {
                Assembly.GetAssembly(typeof(TypeScriptTest)),
            }
        }).Generate();

        Assert.IsNotNull(metadata);
    }
}