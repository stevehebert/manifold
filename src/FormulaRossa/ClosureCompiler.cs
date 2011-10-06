using System;
using System.Collections.Generic;

namespace FormulaRossa
{
    public class ClosureCompiler
    {
        public Func<Injector, object, object> Compile(Stack<IOperation> operations)
        {
            Func<Injector, object, object> functionDefinition = operations.Pop().GetClosure();
            while( operations.Count > 0)
            {
                var localFn = functionDefinition;
                var fn = operations.Pop().GetClosure();

                functionDefinition = (a, b) => fn(a, localFn(a, b));
            }

            return functionDefinition;
        }
    }
}
