using System;
using System.Collections.Generic;
using Wormhole.Configuration;
using Wormhole.Configuration.Operations;
using Wormhole.DependencyInjection;
using Wormhole.Exceptions;

namespace Wormhole.Router
{
    public class RouterCompiler : IPipeCompiler 
    {
        private readonly IPipeDefinition _pipeDefinition;

        public RouterCompiler(IPipeDefinition pipeDefinition)
        {
            _pipeDefinition = pipeDefinition;
        }

        public Func<IResolveTypes, object, object> Compile(IEnumerable<IOperation> operations)
        {
            return (injector, input) =>
            {
                foreach (var item in operations)
                {
                    if (!(item is IRoutedOperation))
                        return item.GetClosure()(injector, input);

                    if ((item as IRoutedOperation).GetDecider()(injector, input))
                        return item.GetClosure()(injector, input);
                }

                // if we get here, then we have a poorly formed definition
                throw new InvalidOperationException("poorly formed router definition");
            };
        }


        public Func<IResolveTypes, object, object> Compile()
        {
            if (!_pipeDefinition.Closed)
                throw new MismatchedClosingTypeDeclarationException();

            return Compile(_pipeDefinition.Operations);
        }
    }
}
