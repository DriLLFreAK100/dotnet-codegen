using CodeGenerator.Attributes;

namespace CodeGenerator.Test.Mocks
{
    [GenerateTs]
    public class InterfaceRoot
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedTime { get; set; }

        public InterfaceNonAnnotatedChild NonAnnotatedChild { get; set; }

        public InterfaceChild AnnotatedChild { get; set; }

        public List<int> ChildIds { get; set; }

        public List<InterfaceNonAnnotatedChild> UnknownChildren { get; set; }

        public List<InterfaceChild> Children { get; set; }

        public Dictionary<int, string> BuiltInDict { get; set; }

        public int? NullableInt { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime? NullableDateTime { get; set; }
    }

    public class InterfaceNonAnnotatedChild
    {
        public int Id { get; set; }
    }

    [GenerateTs]
    public class InterfaceChild
    {
        public int Id { get; set; }
    }
}
