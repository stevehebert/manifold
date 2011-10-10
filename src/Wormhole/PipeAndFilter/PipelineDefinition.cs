using System;
using System.Collections.Generic;
using Wormhole.DependencyInjection;
using Wormhole.Pipeline;

namespace Wormhole.PipeAndFilter
{
    public class PipelineDefinition
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

    }
}
