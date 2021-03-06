using CodeGenerator.Attributes;
using CodeGenerator.Models;
using CodeGenerator.Test.Mocks;

namespace CodeGenerator.Test;

[TestClass]
public class TypeScriptTest : TypeScriptTestBase
{
    [TestMethod("Should Be Able To Output Physical Files")]
    public void ShouldBeAbleToOutput()
    {
        _generator.Generate();

        var outputPath = _generator.GetOption().ActiveBaseOutputPath;

        Assert.IsTrue(Directory.Exists(outputPath));
        Assert.IsTrue(Directory.EnumerateFiles(outputPath).Any(f => f.EndsWith(".ts")));
    }

    [TestMethod("Should Be Able To Output Physical Files For Absolute Path")]
    public void ShouldBeAbleToOutputPhysicalFilesForAbsolutePath()
    {
        _absolutePathGenerator.Generate();

        var outputPath = _absolutePathGenerator.GetOption().ActiveBaseOutputPath;

        Assert.IsTrue(Directory.Exists(outputPath));
        Assert.IsTrue(Directory.EnumerateFiles(outputPath).Any(f => f.EndsWith(".ts")));
    }


    [TestMethod("Should Be Able To Generate Output Metadata")]
    public void ShouldBeAbleToGenerateOutputMetadata()
    {
        var metadata = _dryRunGenerator.Generate();
        Assert.IsNotNull(metadata);

        Assert.IsTrue(metadata.All(m =>
            !string.IsNullOrEmpty(m.Content)
            && !string.IsNullOrEmpty(m.Path)));
    }

    [TestMethod("Should Be Able To Construct Imports By Metadata")]
    public void ShouldBeAbleToConstructImportsByMetadata()
    {
        PrivateObject po = new(_dryRunGenerator);
        var relativeRoot = "/Outputs";

        var importsContent = (string)po.Invoke(
            "ConstructImportsByMetadata",
            new TypeMetadata(typeof(ImportRoot), relativeRoot) { },
            new List<TypeMetadata>()
            {
                new TypeMetadata(typeof(ImportChild1), relativeRoot),
                new TypeMetadata(typeof(ImportChild2), relativeRoot),
                new TypeMetadata(typeof(ImportChild3), relativeRoot),
                new TypeMetadata(typeof(ImportChild4), relativeRoot),
                new TypeMetadata(typeof(ImportChild5), relativeRoot),
                new TypeMetadata(typeof(ImportEnum), relativeRoot),
            });

        var expected = string.Join(_dryRunGenerator.GetOption().LineSeparator, new string[]
        {
            "import { ImportChild1 } from '../../import-child-1';",
            "import { ImportChild2 } from '../../other/import-child-2';",
            "import { ImportChild3 } from '../import-child-3';",
            "import { ImportChild4 } from './import-child-4';",
            "import { ImportChild5 } from './layer/import-child-5';",
            "import { ImportEnum } from '../../import-enum';",
        });

        Assert.IsTrue(importsContent == expected);
    }

    [TestMethod("Should Be Able To Generate Interface")]
    public void ShouldBeAbleToGenerateInterface()
    {
        var types = _dryRunGenerator.GetAnnotatedTypes<GenerateTsAttribute>();

        PrivateObject po = new(_dryRunGenerator);

        var metadata = (List<TypeMetadata>)po.Invoke("GetTypeMetadatas", types);

        var interfaceContent = (string)po.Invoke(
            "GetInterfaceContent",
            new TypeMetadata(typeof(InterfaceRoot), string.Empty) { },
            metadata.ToDictionary(x => x.Type, x => x));

        var expected = string.Join(_dryRunGenerator.GetOption().LineSeparator, new string[]
        {
            "export interface InterfaceRoot {",
            "  id: number;",
            "  name: string;",
            "  createdTime: string;",
            "  nonAnnotatedChild: any;",
            "  annotatedChild: InterfaceChild;",
            "  childIds: number[];",
            "  unknownChildren: any[];",
            "  children: InterfaceChild[];",
            "  builtInDict: { [key: number]: string };",
            "  nullableInt: number | null;",
            "  dateTime: string;",
            "  nullableDateTime: string | null;",
            "  someEnum: InterfaceEnum;",
            "}",
        });

        Assert.IsTrue(interfaceContent == expected);
    }

    [TestMethod("Should Be Able To Generate Enum")]
    public void ShouldBeAbleToGenerateEnum()
    {
        PrivateObject po = new(_dryRunGenerator);

        var enumContent = (string)po.Invoke(
            "GetEnumContent",
            new TypeMetadata(typeof(LanguageType), string.Empty) { });

        var expected = string.Join(_dryRunGenerator.GetOption().LineSeparator, new string[]
        {
            "export enum LanguageType {",
            "  CSharp = 1,",
            "  Java = 2,",
            "  JavaScript = 4,",
            "  Others = 8,",
            "}",
        });

        Assert.IsTrue(enumContent == expected);
    }
}