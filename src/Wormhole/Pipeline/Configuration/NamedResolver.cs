using Wormhole.DependencyInjection;

namespace Wormhole.Pipeline.Configuration
{
    public class NamedResolver<TInput, TOutput>
    {
        private readonly IResolveTypes _typeResolver;
        private readonly PipelineDataResolver _pipelineSets;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedResolver&lt;TInput, TOutput&gt;"/> class.
        /// </summary>
        /// <param name="typeResolver">The type resolver.</param>
        /// <param name="pipelineSets">The pipeline sets.</param>
        public NamedResolver( IResolveTypes typeResolver, 
                              PipelineDataResolver pipelineSets)
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

            var pipeline = _pipelineSets.Find(key);

            return (TOutput) pipeline(_typeResolver, input);
        }
    }
}
