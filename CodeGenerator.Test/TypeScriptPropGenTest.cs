using CodeGenerator.Test.Mocks;

namespace CodeGenerator.Test
{
    [TestClass]
    public class TypeScriptPropGenTest : TypeScriptTestBase
    {
        [TestMethod("Should Be Able To Get Ts Type For List With Built-In Types")]
        public void ShouldBeAbleToGetTsTypeForListWithBuiltInTypes()
        {
            var list = (string)_po.Invoke(
                "GetTsTypeForObject",
                typeof(List<int>),
                _metadataDict);

            Assert.IsTrue(list == "number[]");
        }

        [TestMethod("Should Be Able To Get Ts Type For List With Object")]
        public void ShouldBeAbleToGetTsTypeForListWithObject()
        {
            var list = (string)_po.Invoke(
                "GetTsTypeForObject",
                typeof(List<ObjectDto>),
                _metadataDict);

            Assert.IsTrue(list == "ObjectDto[]");
        }

        [TestMethod("Should Be Able To Get Ts Type For List With Built-In Dictionary")]
        public void ShouldBeAbleToGetTsTypeForListWithBuiltInDictionary()
        {
            var list = (string)_po.Invoke(
                "GetTsTypeForObject",
                typeof(List<Dictionary<int, string>>),
                _metadataDict);

            Assert.IsTrue(list == "{ [key: number]: string }[]");
        }

        [TestMethod("Should Be Able To Get Ts Type For List With Custom Dictionary")]
        public void ShouldBeAbleToGetTsTypeForListWithCustomDictionary()
        {
            var list = (string)_po.Invoke(
                "GetTsTypeForObject",
                typeof(List<Dictionary<int, ObjectDto>>),
                _metadataDict);

            Assert.IsTrue(list == "{ [key: number]: ObjectDto }[]");
        }

        [TestMethod("Should Be Able To Get Ts Type For List With Deep Objects")]
        public void ShouldBeAbleToGetTsTypeForListWithDeepObjects()
        {
            var list = (string)_po.Invoke(
                "GetTsTypeForObject",
                typeof(List<Dictionary<ObjectDto, List<Dictionary<int, ChildDto>>>>),
                _metadataDict);

            Assert.IsTrue(list == "{ [key: ObjectDto]: { [key: number]: ChildDto }[] }[]");
        }

        [TestMethod("Should Be Able To Get Ts Type For Dictionary With Built-In Types")]
        public void ShouldBeAbleToGetTsTypeForDictionaryWithBuiltInTypes()
        {
            var dicts = (string)_po.Invoke(
                "GetTsTypeForObject",
                typeof(Dictionary<int, string>),
                _metadataDict);

            Assert.IsTrue(dicts == "{ [key: number]: string }");
        }

        [TestMethod("Should Be Able To Get Ts Type For Dictionary With Single Object")]
        public void ShouldBeAbleToGetTsTypeForDictionaryWithSingleObject()
        {
            var dicts = (string)_po.Invoke(
                "GetTsTypeForObject",
                typeof(Dictionary<int, ObjectDto>),
                _metadataDict);

            Assert.IsTrue(dicts == "{ [key: number]: ObjectDto }");

            dicts = (string)_po.Invoke(
               "GetTsTypeForObject",
               typeof(Dictionary<ObjectDto, int>),
               _metadataDict);

            Assert.IsTrue(dicts == "{ [key: ObjectDto]: number }");
        }

        [TestMethod("Should Be Able To Get Ts Type For Dictionary With Double Object")]
        public void ShouldBeAbleToGetTsTypeForDictionaryWithDoubleObject()
        {
            var dicts = (string)_po.Invoke(
                "GetTsTypeForObject",
                typeof(Dictionary<ObjectDto, ChildDto>),
                _metadataDict);

            Assert.IsTrue(dicts == "{ [key: ObjectDto]: ChildDto }");
        }

        [TestMethod("Should Be Able To Get Ts Type For Dictionary With List")]
        public void ShouldBeAbleToGetTsTypeForDictionaryWithList()
        {
            var dicts = (string)_po.Invoke(
                "GetTsTypeForObject",
                typeof(Dictionary<ObjectDto, List<ChildDto>>),
                _metadataDict);

            Assert.IsTrue(dicts == "{ [key: ObjectDto]: ChildDto[] }");
        }

        [TestMethod("Should Be Able To Get Ts Type For Dictionary With Deep Objects")]
        public void ShouldBeAbleToGetTsTypeForDictionaryWithDeepObjects()
        {
            var dicts = (string)_po.Invoke(
                "GetTsTypeForObject",
                typeof(Dictionary<ObjectDto, List<Dictionary<int, ChildDto>>>),
                _metadataDict);

            Assert.IsTrue(dicts == "{ [key: ObjectDto]: { [key: number]: ChildDto }[] }");
        }

        [TestMethod("Should Be Able To Generate Nullable")]
        public void ShouldBeAbleToGenerateNullable()
        {
            var nullableInt = (string)_po.Invoke(
                "GetTsTypeForObject",
                typeof(int?),
                _metadataDict);

            Assert.IsTrue(nullableInt == "number | null");
        }
    }
}

