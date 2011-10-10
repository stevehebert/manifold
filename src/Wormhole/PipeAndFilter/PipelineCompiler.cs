using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wormhole.DependencyInjection;

namespace Wormhole.PipeAndFilter
{
    public class PipelineCompiler
    {
        public Func<IResolveTypes, object, object> Compile(IEnumerable<IOperation> operations)
        {
            return Compile(new Queue<IOperation>(operations));
        }

        public Func<IResolveTypes, object, object> Compile(Queue<IOperation> operations)
        {
            var functionDefinition = operations.Dequeue().GetClosure();
            while (operations.Count > 0)
            {
                var localFn = functionDefinition;
                var fn = operations.Dequeue().GetClosure();

                functionDefinition = (a, b) => fn(a, localFn(a, b));
            }

            return functionDefinition;
        }
    }
}
