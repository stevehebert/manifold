using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wormhole.Configuration;
using Wormhole.Configuration.Operations;
using Wormhole.DependencyInjection;

namespace Wormhole.Router.Operators
{
    public class RouterOperation : IOperation
    {
        private readonly PipeDefinition _pipeDefinition;

        public RouterOperation(PipeDefinition pipeDefinition)
        {
            _pipeDefinition = pipeDefinition;

        }
        public Func<IResolveTypes, object, object> GetExecutor()
        {
            var compiler = new RouterCompiler(_pipeDefinition);

            var function = compiler.Compile();

            return function;
        }
    }
}
