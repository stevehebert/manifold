using System;
using System.Collections.Generic;
using System.Linq;

namespace FormulaRossa
{
    public class ProjectorCompiler
    {
        public Func<Injector, TInput, IEnumerable<TOutput>> Compile<TInput, TOutput>(IEnumerable<IProjector<TInput, TOutput>> projectors )
        {
            return (injector, input) =>
                   from p in projectors
                   from q in p.Process(input)
                   select q;
        }
    }
}
