using System;
using System.Collections.Generic;

namespace FormulaRossa
{
    public class RouterCompiler
    {
        public Func<Injector, object, object> Compile(IEnumerable<IOperation> operations )
        {
            return (injector, input) =>
                       {
                           foreach (var item in operations)
                           {
                               if (!(item is IRoutedOperation))
                                   return item.GetClosure();

                               if ((item as IRoutedOperation).GetDecider()(injector, input))
                                   return item.GetClosure();
                           }

                           // if we get here, then we have a poorly formed definition
                           throw new InvalidOperationException("poorly formed router definition");
                       };
        }
    }
}