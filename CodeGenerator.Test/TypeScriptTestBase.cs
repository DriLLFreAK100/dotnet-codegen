using System.Reflection;
using CodeGenerator.Attributes;
using CodeGenerator.Models;

namespace CodeGenerator.Test
{
    [TestClass]
    public class TypeScriptTestBase
	{
        protected TypeScript _dryRunGenerator;
        protected List<Type> _types;
        protected List<TypeMetadata> _metadata;
        protected Dictionary<Type, TypeMetadata> _metadataDict;
        protected PrivateObject _po;

        [TestInitialize]
        public void Initialize()
        {
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
    }
}

