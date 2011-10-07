using System;
using System.Collections.Generic;

namespace FormulaRossa
{
    public class ClosureCompiler
    {
        public Func<Injector, object, object> Compile(IEnumerable<IOperation> operations )
        {
            return Compile(new Queue<IOperation>(operations));
        }
        
        public Func<Injector, object, object> Compile(Queue<IOperation> operations)
        {
            Func<Injector, object, object> functionDefinition = operations.Dequeue().GetClosure();
            while( operations.Count > 0)
            {
                var localFn = functionDefinition;
                var fn = operations.Dequeue().GetClosure();

                functionDefinition = (a, b) => fn(a, localFn(a, b));
            }

            return functionDefinition;
        }
    }
}
