using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wormhole.DependencyInjection;

namespace Wormhole.Router
{
    public class RouterCompiler
    {
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
    }
}
