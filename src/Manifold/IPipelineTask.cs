using System.Collections.Generic;

namespace Manifold
{
    public interface IPipelineTask<in TInput, out TOutput>
    {
        TOutput Execute(TInput input);
    }

    public interface IProjectorTask<in TInput, out TOutput> : IPipelineTask<TInput, IEnumerable<TOutput>>
    {}
}
