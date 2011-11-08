using System;
using System.Collections.Generic;
using System.Linq;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Pipeline.Operations
{
    public class FunctionalNamedResolutionOperation<TInput, TOutput, TAlternateInput, TNameType> : IOperation
    {
        private readonly TNameType _name;
        private readonly Func<TInput, TAlternateInput> _coercionFunction;
        private IEnumerable<NamedPipe<TNameType, TAlternateInput, TOutput>> _namedPipes;

        public FunctionalNamedResolutionOperation(TNameType name, Func<TInput, TAlternateInput> coercionFunction   )
        {
            _name = name;
            _coercionFunction = coercionFunction;
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
                                       typeof(IEnumerable<NamedPipe<TNameType, TAlternateInput, TOutput>>)) as
                                   IEnumerable<NamedPipe<TNameType, TAlternateInput, TOutput>>;

                           var pipe = (from p in _namedPipes where name.Equals(p.Name) select p).First();

                           var coercedInput = _coercionFunction((TInput) o);

                           return pipe.Pipe(resolver, coercedInput);
                       };
        }
    }
}