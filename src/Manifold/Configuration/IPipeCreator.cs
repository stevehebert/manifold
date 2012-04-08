using Manifold.DependencyInjection;
using Manifold.PipeAndFilter;
using Manifold.Projector;
using Manifold.Workflow;

namespace Manifold.Configuration
{
    public interface IPipeCreator
    {
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>();
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name);

        ProjectorConfigurator<TInput, TOutput> RegisterProjector<TInput, TOutput>();
        ProjectorConfigurator<TInput, TOutput> RegisterProjector<TNameType, TInput, TOutput>(TNameType name);

        WorkflowConfigurator<TWorkflow, TState, TTrigger, TTriggerContext> RegisterWorkflow
            <TWorkflow, TState, TTrigger, TTriggerContext>();

        void Compile(IRegisterTypes typeRegistrar);
    }
}
