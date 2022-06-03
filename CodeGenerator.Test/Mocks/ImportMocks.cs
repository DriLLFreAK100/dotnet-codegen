using CodeGenerator.Attributes;

namespace CodeGenerator.Test.Mocks
{
    [GenerateTs(Path = "entity/domain")]
    public class ImportRoot
    {
        public ImportChild1 Child1 { get; set; }

        public ImportChild2 Child2 { get; set; }

        public ImportChild3 Child3 { get; set; }

        public ImportChild4 Child4 { get; set; }

        public ImportChild5 Child5 { get; set; }

        public ImportEnum EnumChild { get; set; }
    }

    [GenerateTs]
    public enum ImportEnum
    {
    }

    [GenerateTs]
    public class ImportChild1
    {
    }

    [GenerateTs(Path = "other")]
    public class ImportChild2
    {
    }

    [GenerateTs(Path = "entity")]
    public class ImportChild3
    {
    }

    [GenerateTs(Path = "entity/domain")]
    public class ImportChild4
    {
    }

    [GenerateTs(Path = "entity/domain/layer")]
    public class ImportChild5
    {
    }
}
