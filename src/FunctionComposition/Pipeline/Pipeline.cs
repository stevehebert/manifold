namespace Wormhole.Pipeline
{
    /// <summary>
    /// This resolver is used for default (or unnamed) pipelines
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    public class Pipeline<TInput, TOutput> : IPipeline<TInput, TOutput> where TOutput : class
    {
        private readonly NamedResolver<TInput, TOutput> _namedResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline&lt;TInput, TOutput&gt;"/> class.
        /// </summary>
        /// <param name="namedResolver">The named resolver.</param>
        public Pipeline(NamedResolver<TInput, TOutput> namedResolver)
        {
            _namedResolver = namedResolver;
        }

        /// <summary>
        /// Executes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>returns the desired output</returns>
        public TOutput Execute(TInput input)
        {
            return _namedResolver.Execute(new DefaultPipeline<TInput,TOutput>(), input);
        }
    }

    public class Pipeline<TNameType, TInput, TOutput> : IPipeline<TNameType, TInput,TOutput>
    {
        private readonly NamedResolver<TInput, TOutput> _namedResolver;

        public Pipeline(NamedResolver<TInput, TOutput> namedResolver)
        {
            _namedResolver = namedResolver;
        }

        public TOutput Execute(TNameType name, TInput input)
        {
            return _namedResolver.Execute(name, input);
        }
    }
}