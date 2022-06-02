using System.Reflection;
using CodeGenerator.Models;
using CodeGenerator.Test.Mocks;

namespace CodeGenerator.Test;

[TestClass]
public class TypeScriptTest
{
    [TestMethod("Should Be Able To Generate Metadata")]
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

    [TestMethod("Should Be Able To Construct Imports Content")]
    public void ShouldBeAbleToConstructImportsContent()
    {
        var generator = new TypeScript(new Option()
        {
            IsDryRun = true,
            TargetAssemblies = new()
            {
                Assembly.GetAssembly(typeof(TypeScriptTest)),
            }
        });

        PrivateObject po = new(generator);
        var relativeRoot = "./Outputs";

        var importsContent = (string)po.Invoke(
            "ConstructImports",
            new TypeMetadata(typeof(ImportRoot), relativeRoot)
            {
            },
            new List<TypeMetadata>()
            {
                new TypeMetadata(typeof(ImportChild1), relativeRoot),
                new TypeMetadata(typeof(ImportChild2), relativeRoot),
                new TypeMetadata(typeof(ImportChild3), relativeRoot),
                new TypeMetadata(typeof(ImportChild4), relativeRoot),
                new TypeMetadata(typeof(ImportChild5), relativeRoot),
            });

        var expected = new string[]
        {
            "import { ImportChild1 } from '../../import-child-1.ts';",
            "import { ImportChild2 } from '../../other/import-child-2.ts';",
            "import { ImportChild3 } from '../import-child-3.ts';",
            "import { ImportChild4 } from './import-child-4.ts';",
            "import { ImportChild5 } from './layer/import-child-5.ts';",
        };

        Assert.IsTrue(importsContent == string.Join(generator.GetOption().LineSeparator, expected));
    }
}