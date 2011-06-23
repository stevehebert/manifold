using System;
using System.Collections.Generic;
using Wormhole.DependencyInjection;

namespace Wormhole.Pipeline.Configuration
{
    public class NamedResolver<TInput, TOutput>
    {
        private readonly IResolveTypes _typeResolver;
        private readonly IDictionary<PipelineKey, Func<IResolveTypes, object, object>> _pipelineSets;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedResolver&lt;TInput, TOutput&gt;"/> class.
        /// </summary>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="pipelineSets">The pipeline sets.</param>
        public NamedResolver( IResolveTypes typeResolver, 
                              IDictionary<PipelineKey, Func<IResolveTypes, object, object>> pipelineSets)
        {
            _typeResolver = typeResolver;
            _pipelineSets = pipelineSets;
        }

        /// <summary>
        /// Executes the specified named context.
        /// </summary>
        /// <param name="namedContext">The named context.</param>
        /// <param name="input">The input.</param>
        /// <returns>the required output</returns>
        public TOutput Execute(object namedContext, TInput input)
        {
            var key = new PipelineKey { Input = typeof (TInput), Output = typeof (TOutput), Named = namedContext };

            if(_pipelineSets.ContainsKey(key))
                return (TOutput) _pipelineSets[key](_typeResolver, input);

            var type = typeof (Functor<,,>);
            throw new ArgumentException();
        }
    }
}
