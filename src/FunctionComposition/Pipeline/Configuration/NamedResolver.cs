using System;
using System.Collections.Generic;
using Wormhole.DependencyInjection;

namespace Wormhole.Pipeline.Configuration
{
    public class NamedResolver<TInput, TOutput>
    {
        private readonly IResolveTypes _typeResolver;
        private readonly IDictionary<PipelineKey, Func<IResolveTypes, object, object>> _pipelineSets;

        public NamedResolver( IResolveTypes typeResolver, 
                              IDictionary<PipelineKey, Func<IResolveTypes, object, object>> pipelineSets)
        {
            _typeResolver = typeResolver;
            _pipelineSets = pipelineSets;
        }

        public TOutput Execute(object namedContext, TInput input)
        {
            var key = new PipelineKey {Input = typeof (TInput), Output = typeof (TOutput), Named = namedContext};

            return (TOutput) _pipelineSets[key](_typeResolver, input);
        }
    }
}
