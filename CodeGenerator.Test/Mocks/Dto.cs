namespace CodeGenerator.Test.Mocks
{
    public class ListDto
    {
        public List<ChildDto> ChildInList { get; set; }
    }

    public class DictionaryDto
    {
        public Dictionary<int, ChildDto> ChildInDictionary { get; set; }
    }

    public class ObjectDto
    {
        public ChildDto ChildAsObject { get; set; }
    }

    public class ChildDto
    {
        public int Id { get; set; }

        public GrandChildDto ChildObject { get; set; }
    }

    public class GrandChildDto
    {
        public string Id { get; set; }
    }
}

