# Code Generator
This project is intended to create an easy to use CodeGenerator for C#. 

Currently, this is only targeting C# to TypeScript as the development use case.

## Basic Usage
Simply annotate the C# classes that you would like to generate their `TypeScript interfaces`.

```
[GenerateTs]
public class YourClass
{
    public int SomeProp { get; set; }
}
```

Create a generator instance and invoke its `Generate()` method. You can configure the generator via its `Option` param.

```
var generator = new TypeScript(new Option()
{
    RelativeBaseOutputPath = "./Outputs",
    TargetAssemblies = new()
    {
        Assembly.GetAssembly(typeof(Dummy)),
    }
});

generator.Generate();
```

## Nested Types
By default if child type is not annotated, the corresponding output TS type will be `any`.

If you would like the child type to be generated as well, just mark the child type with `GenerateTs` attribute as well.

```
[GenerateTs]
public class YourClass
{
    public List<YourChildClass> SomeProp { get; set; }
}

[GenerateTs]
public class YourChildClass
{
    public int SomeProp { get; set; }
}
```

## Dry Run
You can also perform a dry run to check what are the outputs from the generation by specifying `IsDryRun` as `true` in the option

```
var generator = new TypeScript(new Option()
{
    IsDryRun = true,
    RelativeBaseOutputPath = "./Outputs",
    TargetAssemblies = new()
    {
        Assembly.GetAssembly(typeof(Dummy)),
    }
});

generator.Generate();
```

## ASP.NET Web API, Console, etc. Integrations
It is totally up to your creativity. Just create the generator and invoke its `Generate()` method.

For ASP.NET Web API, a good place to do this might be your `Configure()` method at the app `startup`. That way, you can generate the interfaces everytime you make changes and restart your dev server. However, remember to include condition to generate only when it is `dev` environment. This is to prevent unnecessary code generations in `production` environment.

For this to work like an on-demand code generation tool, you can also import it into a `Console app` and run it on-demand. 

Really, it's just about configuring the generator and call the `Generate()` method as you deem fit.

## Options
For the full list of options, refer to `Option.cs` at https://github.com/DriLLFreAK100/cs-codegen/blob/main/CodeGenerator/Models/Option.cs 

Basically,
- `IsDryRun` - whether to generate physical files or only metadata for checking purpose
- `AbsoluteBaseOutputPath` - absolute path to generate physical files. It takes precedence over `RelativeBaseOutputPath`
- `RelativeBaseOutputPath` - path to generate physical files, relative from the program execution path
- `LineSeparator` - next line (default to follow environment)
- `TargetAssemblies` - target assemblies to scan for code generation. All classes marked by `GenerateTs` within the assemblies will be generated

## Thoughts
Any thoughts and inputs are always welcomed!