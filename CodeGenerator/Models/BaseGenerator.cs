using CodeGenerator.Utils;

namespace CodeGenerator.Models
{
    public abstract class BaseGenerator
    {
        private readonly Option _option;

        public BaseGenerator(Option option)
        {
            _option = option;
        }

        public abstract List<Output> GenerateMetadata();

        public virtual List<Output> Generate()
        {
            var pipeline = MakePipeline();
            var outputs = pipeline();

            return outputs;
        }

        private Func<List<Output>> MakePipeline()
        {
            var (isDryRun, baseOutputPath) = _option;

            if (isDryRun)
            {
                return () => GenerateMetadata();
            }

            return () =>
            {
                FileHandler.Clear(baseOutputPath);
                var metadata = GenerateMetadata();
                FileHandler.WriteOutputs(metadata);

                return metadata;
            };
        }
    }
}