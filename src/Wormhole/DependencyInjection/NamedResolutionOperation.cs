using System;
using System.Collections.Generic;
using System.Linq;
using Wormhole.Configuration;

namespace Wormhole.DependencyInjection
{
    public class NamedResolutionOperation<TInput, TOutput, TNameType> : IOperation
    {
        private readonly TNameType _name;

        public NamedResolutionOperation(TNameType name)
        {
            _name = name;
        }


        public Func<IResolveTypes, object, object> GetClosure()
        {
            return GetNamedClosure(_name);
        }

        public Func<IResolveTypes, object, object> GetNamedClosure(object name)
        {
            return (resolver, o) =>
                       {
                           var key = new PipelineKey {Input = typeof (TInput), Output = typeof (TOutput), Named = name};

                           var pipeData =
                               resolver.Resolve(
                                   typeof (IEnumerable<IDictionary<PipelineKey, Func<IResolveTypes, object, object>>>))
                               as IEnumerable<IDictionary<PipelineKey, Func<IResolveTypes, object, object>>>;

                           var targetSet = (from p in pipeData
                                            where p.ContainsKey(key)
                                            select p).FirstOrDefault();

                           return targetSet[key](resolver, o);
                       };
        }
    }
}
