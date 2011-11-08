﻿using System;
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

        public void DefaultContinue<TNameType>(TNameType name)
        {
            PipeDefinition.AddNamedContinuation<TInput, TOutput, TNameType>(name, true);
        }

        public void DefaultContinue<TNameType, TAlternateInput>(TNameType name, Func<TInput, TAlternateInput> function )
        {
            PipeDefinition.AddCoercedContinuation<TNameType, TInput, TAlternateInput, TOutput>(name, function, true);
        }

        public RouterConfigurator<TInput, TOutput> BindConditional(Func<TInput, bool> canProcessFunction, Func<TInput,TOutput> processFunction   )
        {
            PipeDefinition.AddRouteFunctionOperation(canProcessFunction, processFunction, true);
            return this;
        } 
        
        public void Default(Func<TInput, TOutput> function)
        {
            PipeDefinition.AddFunctionOperation(function, true);
        }
    }
}
