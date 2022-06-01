using CodeGenerator.Attributes;

namespace CodeGenerator.Test.Mocks
{
    [GenerateTs]
    public class ListDto
    {
        public List<ChildDto> ChildInList { get; set; }
    }

    [GenerateTs]
    public class DictionaryDto
    {
        public Dictionary<int, ChildDto> ChildInDictionary { get; set; }
    }

    [GenerateTs]
    public class ObjectDto
    {
        public ChildDto ChildAsObject { get; set; }
    }

    [GenerateTs]
    public class ChildDto
    {
        public int Id { get; set; }

        public GrandChildDto ChildObject { get; set; }
    }

    [GenerateTs]
    public class GrandChildDto
    {
        public string Id { get; set; }
    }
}

