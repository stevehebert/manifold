using Autofac;
using Wormhole.Autofac.Configuration;
using Wormhole.Pipeline;

namespace Wormhole.Autofac
{
    public abstract class PipelineModule : Module
    {
        private readonly PipelineCreator<AutofacTypeResolver> _pipelineCreator = new PipelineCreator<AutofacTypeResolver>();

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
            _pipelineCreator.PerformRegistration(new AutofacTypeRegistrar(builder));
        }
    }
}
