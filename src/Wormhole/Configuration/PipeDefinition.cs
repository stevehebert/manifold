using System;
using System.Collections.Generic;
using Manifold.Configuration.Operations;
using Manifold.DependencyInjection;
using Manifold.Router.Operators;

namespace Manifold.Configuration
{
    public interface IPipeDefinition
    {
        bool Closed { get;  }
        IEnumerable<IOperation> Operations { get; }
    }

    public class PipeDefinition : IPipeDefinition
    {
        public bool Closed { get; private set; }

        public IEnumerable<IOperation> Operations
        {
            get { return _operations; }
        }

        private readonly IList<IOperation> _operations = new List<IOperation>();
        private readonly IList<Action<IRegisterTypes>> _registrationActions;

        public PipeDefinition(IList<Action<IRegisterTypes>> registrationActions)
        {
            Closed = false;
            _registrationActions = registrationActions;
        }

        public void AddInjectedOperation<TType, TInput, TOutput>(bool closed)
            where TType : class, IPipelineTask<TInput, TOutput>
        {
            Closed = closed;
            _operations.Add(new InjectedOperation<TType, TInput, TOutput>());
            _registrationActions.Add(ctx => ctx.RegisterType<TType>());
        }

        public void AddFunctionOperation<TInput, TOutput>(Func<TInput, TOutput> function, bool closed)
        {
            Closed = closed;
            _operations.Add(new FunctionOperation<TInput, TOutput>(function));
        }

        public void AddCustomInjectedOperations<TType, TInput, TOutput>(Func<TType, TInput, TOutput> function,
                                                                        bool closed) where TType : class
        {
            Closed = closed;
            _operations.Add(new CustomInjectedOperation<TType, TInput, TOutput>(function));
            _registrationActions.Add(ctx => ctx.RegisterType<TType>());
        }

        public void AddNamedContinuation<TInput, TOutput, TNameType>(TNameType name, bool closed)
        {
            Closed = closed;
            _operations.Add(new NamedResolutionOperation<TInput, TOutput, TNameType>(name));
        }

        public void AddInjectedRouteOperation<TType, TInput, TOutput>()
            where TType : class, IRoutingPipelineTask<TInput, TOutput>
        {
            Closed = false;
            _operations.Add(new InjectedRoutedOperation<TType, TInput, TOutput>());
        }

        public void AddRouteFunctionOperation<TInput, TOutput>(Func<TInput, bool> canProcessFunction,
                                                               Func<TInput, TOutput> processFunction, bool closed)
        {
            Closed = closed;
            _operations.Add(new RoutingFunctionOperation<TInput, TOutput>(canProcessFunction, processFunction));
        }

        public PipeDefinition AddRoutingIntegrationStep()
        {
            // TODO: too big of an assumption!
            Closed = true;
            var pipeDefinition = new PipeDefinition(_registrationActions);

            _operations.Add(new RouterOperation(pipeDefinition));

            return pipeDefinition;
        }
    }
}
