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

        public Option GetOption()
        {
            return _option;
        }

        public List<Type> GetAnnotatedTypes<T>()
        {
            return _option.TargetAssemblies
                .SelectMany(assembly =>
                {
                    return assembly
                        .GetTypes()
                        .Where(type => type.IsDefined(typeof(T), false));
                }).ToList();
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
                FileHelper.Clear(baseOutputPath);
                var metadata = GenerateMetadata();
                FileHelper.WriteOutputs(metadata);

                return metadata;
            };
        }
    }
}