using System;

namespace Manifold.DependencyInjection
{
    public class NamedPipe<TType, TInput, TOutput>
    {
        public TType Name { get; private set; }

        public Func<IPipelineContext, TInput, TOutput> Pipe { get; private set; } 

        public NamedPipe(TType name, Func<IPipelineContext, TInput, TOutput> pipe )
        {
            Name = name;
            Pipe = pipe;
        }
    }
}
