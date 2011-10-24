using System;
using System.Collections.Generic;
using System.Linq;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Pipeline.Operations
{
    public class NamedResolutionOperation<TInput, TOutput, TNameType> : IOperation
    {
        private readonly TNameType _name;

        public NamedResolutionOperation(TNameType name)
        {
            _name = name;
        }


        public Func<IPipelineContext, object, object> GetExecutor()
        {
            return GetNamedClosure(_name);
        }

        public Func<IPipelineContext, object, object> GetNamedClosure(object name)
        {
            return (resolver, o) =>
                       {
                           var key = new PipelineKey {Input = typeof (TInput), Output = typeof (TOutput), Named = _name};

                           var pipeData =
                               resolver.TypeResolver.Resolve(
                                   typeof (IEnumerable<IDictionary<PipelineKey, Func<IPipelineContext, object, object>>>))
                               as IEnumerable<IDictionary<PipelineKey, Func<IPipelineContext, object, object>>>;

                           var targetSet = (from p in pipeData
                                            where p.ContainsKey(key)
                                            select p).FirstOrDefault();

                           return targetSet[key](resolver, o);
                       };
        }
    }
}
