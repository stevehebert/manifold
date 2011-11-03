using System;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Pipeline.Operations
{
    public class AnonymousResolutionOperation<TInput, TOutput> : IOperation
    {
        private AnonymousPipe<TInput, TOutput> _pipe; 

        public Func<IPipelineContext, object, object> GetExecutor()
        {
            return (resolver, o) =>
                       {
                           if (_pipe == null)
                               _pipe =
                                   resolver.TypeResolver.Resolve(
                                       typeof (AnonymousPipe<TInput, TOutput>)) as
                                   AnonymousPipe<TInput, TOutput>;

                           return _pipe.Pipe(resolver, (TInput) o);
                       };

        }
    }
}