using System;
using Manifold.Configuration;
using Manifold.Configuration.Pipeline;

namespace Manifold.Router
{
    public class RouterConfigurator<TInput, TOutput>
    {
        protected readonly PipeDefinition PipeDefinition;

        public RouterConfigurator(PipeDefinition pipeDefinition)
        {
            PipeDefinition = pipeDefinition;
        }

        public RouterConfigurator<TInput, TOutput> BindConditional<TType>() where TType : class, IRoutingPipelineTask<TInput, TOutput>
        {
            PipeDefinition.AddInjectedRouteOperation<TType, TInput, TOutput>();
            return this;
        }

        public void Default<TType>() where TType : class, IPipelineTask<TInput, TOutput>
        {
            PipeDefinition.AddInjectedOperation<TType, TInput, TOutput>(true);
        }

        public RouterConfigurator<TInput, TOutput> BindConditional(Func<TInput, bool> canProcessFunction, Func<TInput,TOutput> processFunction   )
        {
            PipeDefinition.AddRouteFunctionOperation<TInput, TOutput>(canProcessFunction, processFunction, true);
            return this;
        } 
        
        public void Default(Func<TInput, TOutput> function)
        {
            PipeDefinition.AddFunctionOperation(function, true);
        }
    }
}
