using System;

namespace Manifold.DependencyInjection
{
    public class AnonymousPipe<TInput, TOutput>
    {
        public Func<IPipelineContext, TInput, TOutput> Pipe { get; private set; } 

        public AnonymousPipe(Func<IPipelineContext, TInput, TOutput> pipe )
        {
            Pipe = pipe;
        }
    }
}