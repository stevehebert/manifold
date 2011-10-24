using System;
using System.Collections.Generic;
using Manifold.Configuration;
using Manifold.Configuration.Pipeline;
using Manifold.Configuration.Pipeline.Operations;
using Manifold.DependencyInjection;
using Manifold.Exceptions;

namespace Manifold.Router
{
    public class RouterCompiler : IPipeCompiler 
    {
        private readonly IPipeDefinition _pipeDefinition;

        public RouterCompiler(IPipeDefinition pipeDefinition)
        {
            _pipeDefinition = pipeDefinition;
        }

        public Func<IPipelineContext, object, object> Compile(IEnumerable<IOperation> operations)
        {
            return (injector, input) =>
            {
                foreach (var item in operations)
                {
                    if (!(item is IRoutedOperation))
                        return item.GetExecutor()(injector, input);

                    if ((item as IRoutedOperation).GetDecider()(injector, input))
                        return item.GetExecutor()(injector, input);
                }

                // if we get here, then we have a poorly formed definition
                throw new InvalidOperationException("poorly formed router definition");
            };
        }


        public Func<IPipelineContext, object, object> Compile()
        {
            if (!_pipeDefinition.Closed)
                throw new MismatchedClosingTypeDeclarationException();

            return Compile(_pipeDefinition.Operations);
        }
    }
}
