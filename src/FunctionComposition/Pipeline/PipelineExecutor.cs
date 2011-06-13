using System;
using Wormhole.Core;
using Wormhole.DependencyInjection;

namespace Wormhole.Pipeline
{
    public class PipelineExecutor : IStateMonad<IResolveTypes,Func<Type, object, object>>
    {
        public Tuple<IResolveTypes, Func<Type, object, object>> Execute(Tuple<IResolveTypes, Func<Type, object, object>> stateValuePair)
        {
            var tuple = new Tuple<IResolveTypes,Func<Type, object, object>>(stateValuePair.Item1, (type, b) =>
                                                                                                      {
                                                                                                          object item = null;
                                                                                                          if(type != null)
                                                                                                              item =
                                                                                                                  stateValuePair
                                                                                                                      .
                                                                                                                      Item1
                                                                                                                      .
                                                                                                                      Resolve
                                                                                                                      (type);

                                                                                                          return
                                                                                                              item.
                                                                                                                  Execute
                                                                                                                  (b);
                                                                                                      })
            
        }
    }
}
