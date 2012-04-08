using System;

namespace Manifold.Workflow.Compiler.Configuration
{
    public class WorkflowTypeConfigurator<TWorkflow, TState, TTrigger, TContext>
    {
        private readonly Action<TWorkflow, TState, IConfigurationAction> _configurationRecorder;

        internal WorkflowTypeConfigurator(Action<TWorkflow, TState, IConfigurationAction> configurationRecorder)
        {
            _configurationRecorder = configurationRecorder;
        }

        public WorkflowTypeConfigurator2<TWorkflow, TState, TTrigger, TContext> For(TWorkflow workflow)
        {
            return new WorkflowTypeConfigurator2<TWorkflow, TState, TTrigger, TContext>(workflow, _configurationRecorder);
        }
    }
}
