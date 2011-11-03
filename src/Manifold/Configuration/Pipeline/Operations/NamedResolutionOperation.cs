using System;
using System.Collections.Generic;
using System.Linq;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Pipeline.Operations
{
    public class NamedResolutionOperation<TInput, TOutput, TNameType> : IOperation
    {
        private readonly TNameType _name;
        private IEnumerable<NamedPipe<TNameType, TInput, TOutput>> _namedPipes; 

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
                           if (_namedPipes == null)
                               _namedPipes =
                                   resolver.TypeResolver.Resolve(
                                       typeof (IEnumerable<NamedPipe<TNameType, TInput, TOutput>>)) as
                                   IEnumerable<NamedPipe<TNameType, TInput, TOutput>>;

                           var pipe = (from p in _namedPipes where name.Equals(p.Name) select p).First();

                           return pipe.Pipe(resolver, (TInput)o);
                       };
        }
    }
}
