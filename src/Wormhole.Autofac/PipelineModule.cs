using Autofac;
using Wormhole.Autofac.Configuration;
using Wormhole.Pipeline;
using Wormhole.Pipeline.Configuration;

namespace Wormhole.Autofac
{
    public class PipelineModule : Module, IPipelineCreator
    {
        private readonly PipelineAggregator<TypeResolver> _pipelineAggregator = new PipelineAggregator<TypeResolver>();

        internal virtual void RegisterPipelines(){}

        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        protected override void Load(ContainerBuilder builder)
        {
            RegisterPipelines();
            _pipelineAggregator.PerformRegistration(new TypeRegistrar(builder));
        }

        /// <summary>
        /// Registers an unnamed pipeline
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <returns>a configurator for fluently configuring the pipeline</returns>
        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>()
        {
            return _pipelineAggregator.RegisterPipeline<TInput, TOutput>();
        }

        /// <summary>
        /// Registers a named pipeline.
        /// </summary>
        /// <typeparam name="TNameType">The type of the name type.</typeparam>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="name">The pipeline name.</param>
        /// <returns>a configurator for fluently configuring the pipeline</returns>
        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name)
        {
            return _pipelineAggregator.RegisterPipeline<TNameType, TInput, TOutput>(name);
        }
    }
}
