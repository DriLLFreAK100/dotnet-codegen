using System.Reflection;
using CodeGenerator.Attributes;
using CodeGenerator.Models;
using CodeGenerator.Utils;

namespace CodeGenerator.Test
{
    [TestClass]
    public class TypeScriptTestBase
    {
        protected TypeScript _absolutePathGenerator;
        protected TypeScript _generator;
        protected TypeScript _dryRunGenerator;
        protected List<Type> _types;
        protected List<TypeMetadata> _metadata;
        protected Dictionary<Type, TypeMetadata> _metadataDict;
        protected PrivateObject _po;

        [TestInitialize]
        public void Initialize()
        {
            _absolutePathGenerator = new TypeScript(new Option()
            {
                AbsoluteBaseOutputPath = $"{AppDomain.CurrentDomain.BaseDirectory}/Outputs",
                TargetAssemblies = new()
                {
                    Assembly.GetAssembly(typeof(TypeScriptTest)),
                }
            });

            _generator = new TypeScript(new Option()
            {
                RelativeBaseOutputPath = "./Outputs",
                TargetAssemblies = new()
                {
                    Assembly.GetAssembly(typeof(TypeScriptTest)),
                }
            });

            _dryRunGenerator = new TypeScript(new Option()
            {
                IsDryRun = true,
                TargetAssemblies = new()
                {
                    Assembly.GetAssembly(typeof(TypeScriptTest)),
                }
            });

            _po = new(_dryRunGenerator);
            _types = _dryRunGenerator.GetAnnotatedTypes<GenerateTsAttribute>();
            _metadata = (List<TypeMetadata>)_po.Invoke("GetTypeMetadatas", _types);
            _metadataDict = _metadata.ToDictionary(x => x.Type, x => x);
        }

        [TestCleanup]
        public void Cleanup()
        {
            var outputPath = $"{Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)}/Outputs";
            FileHelper.Clear(outputPath);
        }
    }
}

