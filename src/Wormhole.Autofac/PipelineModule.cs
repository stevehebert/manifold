using Autofac;
using Wormhole.Autofac.Configuration;
using Wormhole.Configuration;
using Wormhole.DependencyInjection;
using Wormhole.PipeAndFilter;


namespace Wormhole.Autofac
{
    public abstract class PipelineModule : Module
    {
        private readonly IPipeCreator _pipeCreator;

        public PipelineModule() : base ()
        {
            _pipeCreator = new PipeCreator(new PipelineAggregator<AutofacTypeResolver>());
        }

        /// <summary>
        /// Registers the pipelines.
        /// </summary>
        /// <param name="pipeCreator">The pipeline creator.</param>
        public abstract void RegisterPipelines(IPipeCreator pipeCreator);
        
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
            RegisterPipelines(_pipeCreator);
            _pipeCreator.Compile(new AutofacTypeRegistrar(builder) );
        }
    }
}
