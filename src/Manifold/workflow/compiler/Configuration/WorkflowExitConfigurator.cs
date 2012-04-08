using System;

namespace Manifold.Workflow.Compiler.Configuration
{
    public class WorkflowExitConfigurator<TWorkflow, TState, TTrigger, TContext>
    {
        private readonly TWorkflow _workflow;
        private readonly TState _state;
        private readonly Action<TWorkflow, TState, IConfigurationAction> _configurationAction;

        internal WorkflowExitConfigurator(TWorkflow workflow, TState state, Action<TWorkflow, TState, IConfigurationAction> configurationAction)
        {
            _workflow = workflow;
            _state = state;
            _configurationAction = configurationAction;
        }

        public WorkflowTypeConfigurator2<TWorkflow, TState, TTrigger, TContext> For(TWorkflow workflow)
        {
            return new WorkflowTypeConfigurator2<TWorkflow, TState, TTrigger, TContext>(workflow, _configurationAction);
        }

        public WorkflowStateConfigurator<TWorkflow, TState, TTrigger, TContext> Configure(TState state)
        {
            return new WorkflowStateConfigurator<TWorkflow, TState, TTrigger, TContext>(_workflow, state, _configurationAction);
        }

        public WorkflowExitConfigurator<TWorkflow, TState, TTrigger, TContext> Do<TType>()
        {
            _configurationAction(_workflow, _state, null);
            return this;
        }
    }
}