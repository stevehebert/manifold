using System;
using System.Collections.Generic;
using System.Linq;
using Wormhole.Pipeline.Configuration;

namespace Wormhole.DependencyInjection
{
    public class NamedResolutionOperation<TInput, TOutput, TNameType> : IOperation
    {
        private TNameType _name;

        public NamedResolutionOperation(TNameType name)
        {
            _name = name;
        }


        public Func<IResolveTypes, object, object> GetClosure()
        {
            return (resolver, o) =>
                       {
                           var key = new PipelineKey {Input = typeof (TInput), Output = typeof (TOutput), Named = _name};

                           var pipeData = resolver.Resolve(typeof(IEnumerable<IDictionary<PipelineKey, Func<IResolveTypes, object, object>>>)) as IEnumerable<IDictionary<PipelineKey, Func<IResolveTypes, object, object>>>;

                           var targetSet = (from p in pipeData
                                            where p.ContainsKey(key)
                                            select p).FirstOrDefault();

                           return targetSet[key];
                       };
        }
    }
}
