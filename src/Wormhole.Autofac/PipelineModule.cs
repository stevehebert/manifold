using Autofac;
using Wormhole.Autofac.Configuration;
using Wormhole.DependencyInjection;
using Wormhole.PipeAndFilter;


namespace Wormhole.Autofac
{
    public abstract class PipelineModule : Module
    {
        private readonly IPipelineCreator _pipelineCreator;

        public PipelineModule() : base ()
        {
            _pipelineCreator = new PipelineCreator(new PipelineAggregator<AutofacTypeResolver>());
        }

        /// <summary>
        /// Registers the pipelines.
        /// </summary>
        /// <param name="pipelineCreator">The pipeline creator.</param>
        public abstract void RegisterPipelines(IPipelineCreator pipelineCreator);
        
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
            RegisterPipelines(_pipelineCreator);
            _pipelineCreator.Compile(new AutofacTypeRegistrar(builder) );
        }
    }
}
