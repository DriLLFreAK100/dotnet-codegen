using System.Reflection;
using CodeGenerator.Attributes;
using CodeGenerator.Models;
using CodeGenerator.Test.Mocks;

namespace CodeGenerator.Test
{
	[TestClass]
    public class TypeScriptPropGenTest
	{
        private TypeScript _dryRunGenerator;
        private List<Type> _types;
        private List<TypeMetadata> _metadata;
        private Dictionary<Type, TypeMetadata> _metadataDict;
        private PrivateObject _po;

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

        [TestMethod("Should Be Able To Get Ts Type For Built-In List")]
        public void ShouldBeAbleToGetTsTypeForBuiltInList()
        {
            var number = (string)_po.Invoke(
                "GetTsTypeForList",
                typeof(List<int>),
                _metadataDict);

            Assert.IsTrue(number == "number[]");
        }

        [TestMethod("Should Be Able To Get Ts Type For Object List")]
        public void ShouldBeAbleToGetTsTypeForObjectList()
        {
            var objectDtos = (string)_po.Invoke(
                "GetTsTypeForList",
                typeof(List<ObjectDto>),
                _metadataDict);

            Assert.IsTrue(objectDtos == "ObjectDto[]");
        }

        [TestMethod("Should Be Able To Get Ts Type For Built-In Dictionary")]
        public void ShouldBeAbleToGetTsTypeForBuiltInDictionary()
        {
            var dicts = (string)_po.Invoke(
                "GetTsTypeForDictionary",
                typeof(Dictionary<int, string>),
                _metadataDict);

            Assert.IsTrue(dicts == "{ [key: number]: string }");
        }

        [TestMethod("Should Be Able To Get Ts Type For Dictionary List")]
        public void ShouldBeAbleToGetTsTypeForDictionaryList()
        {
            var dicts = (string)_po.Invoke(
                "GetTsTypeForDictionary",
                typeof(List<Dictionary<int, ObjectDto>>),
                _metadataDict);

            Assert.IsTrue(dicts == "{ [key: number]: ObjectDto }[]");
        }
    }
}

