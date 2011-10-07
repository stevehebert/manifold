using System.Collections.Generic;

namespace FormulaRossa
{
    public interface IProjector<in TInput, out TOutput>
    {
        IEnumerable<TOutput> Process(TInput input);
    }
}