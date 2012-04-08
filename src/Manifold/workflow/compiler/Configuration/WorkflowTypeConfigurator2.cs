using System;

namespace Manifold.Workflow.Compiler.Configuration
{
    public class WorkflowTypeConfigurator2<TWorkflow, TState, TTrigger, TContext>
    {
        private readonly TWorkflow _workflow;
        private readonly Action<TWorkflow, TState, IConfigurationAction> _configurationRecorder;

        internal WorkflowTypeConfigurator2(TWorkflow workflow, Action<TWorkflow, TState, IConfigurationAction> configurationRecorder)
        {
            _workflow = workflow;
            _configurationRecorder = configurationRecorder;
        }

        public WorkflowTypeConfigurator2<TWorkflow, TState, TTrigger, TContext> For(TWorkflow workflow)
        {
            return new WorkflowTypeConfigurator2<TWorkflow, TState, TTrigger, TContext>(workflow, _configurationRecorder);
        }

        public WorkflowStateConfigurator<TWorkflow, TState, TTrigger, TContext> Configure(TState state)
        {
            return new WorkflowStateConfigurator<TWorkflow, TState, TTrigger, TContext>(_workflow, state,
                                                                                        _configurationRecorder);
        }
    }
}