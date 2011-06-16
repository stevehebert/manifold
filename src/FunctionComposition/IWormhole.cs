using System;
using Wormhole.Pipeline;

namespace Wormhole
{
    public interface IWormhole<in TInput, out TOutput>
    {
        TOutput Execute(TInput input);

        TOutput Execute(object name, TInput input);
    }

    public class Wormhole<TInput, TOutput> : IWormhole<TInput, TOutput> where TOutput : class
    {
        private NamedResolver<TInput, TOutput> _namedResolver;
        
        public Wormhole(NamedResolver<TInput, TOutput> namedResolver)
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
