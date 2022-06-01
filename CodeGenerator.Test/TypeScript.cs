using CodeGenerator.Models;

namespace CodeGenerator.Test;

[TestClass]
public class TypeScript
{
    [TestMethod("Should be able to generate metadata")]
    public void ShouldBeAbleToGenerateMetadata()
    {
        var metadata = new CodeGenerator.TypeScript(new Option()
        {
            IsDryRun = true,
        }).Generate();

        Assert.IsNotNull(metadata);
    }
}