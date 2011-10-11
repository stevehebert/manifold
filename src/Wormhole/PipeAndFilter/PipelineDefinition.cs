using System;
using System.Collections.Generic;
using Wormhole.DependencyInjection;
using Wormhole.Pipeline;

namespace Wormhole.PipeAndFilter
{
    public interface IPipelineDefinition
    {
        IEnumerable<IOperation> Operations { get; }
    }

    public class PipelineDefinition : IPipelineDefinition
    {
        public IEnumerable<IOperation> Operations {get { return _operations; }}

        private readonly IList<IOperation> _operations = new List<IOperation>();
        private readonly IList<Action<IRegisterTypes>> _registrationActions;

        public PipelineDefinition(IList<Action<IRegisterTypes>> registrationActions )
        {
            _registrationActions = registrationActions;
        }

        public void AddInjectedOperation<TType, TInput, TOutput>() where TType : class, IPipelineTask<TInput, TOutput>
        {
            _operations.Add(new InjectedOperation<TType,TInput,TOutput>());
            _registrationActions.Add(ctx => ctx.RegisterType<TType>());
        }

        public void AddFunctionOperation<TInput, TOutput>(Func<TInput, TOutput> function )
        {
            _operations.Add(new FunctionOperation<TInput,TOutput>(function));
        }

        public void AddCustomInjectedOperations<TType, TInput, TOutput>(Func<TType, TInput, TOutput >  function) where TType : class
        {
            _operations.Add(new CustomInjectedOperation<TType, TInput, TOutput>(function));
            _registrationActions.Add(ctx => ctx.RegisterType<TType>());
        }

        public void AddNamedContinuation<TInput, TOutput, TNameType>(TNameType name)
        {
            _operations.Add(new NamedResolutionOperation<TInput, TOutput, TNameType>(name));
        }
    }
}
