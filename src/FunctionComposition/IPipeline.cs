using Wormhole.Pipeline;

namespace Wormhole
{
    public interface IPipeline<TInput, TOutput>
    {
        TOutput Execute(TInput input);

        TOutput Execute(object name, TInput input);
    }

    public class Pipeline<TInput, TOutput> : IPipeline<TInput, TOutput> where TOutput : class
    {
        private readonly NamedResolver<TInput, TOutput> _namedResolver;
        
        public Pipeline(NamedResolver<TInput, TOutput> namedResolver)
        {
            _namedResolver = namedResolver;
        }

        public TOutput Execute(TInput input)
        {
            return _namedResolver.Execute(new DefaultPipeline<TInput,TOutput>(), input);
        }

        public TOutput Execute(object name, TInput input)
        {
            return _namedResolver.Execute(name, input);

        }
    }
}
