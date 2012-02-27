using System;
using System.Collections.Generic;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Projector.Operations
{
    class CustomInjectedOperation<TType, TInput, TOutput>:IProjectorOperation<TInput,TOutput> where TType : class
    {
        private readonly Func<TType, TInput, IEnumerable<TOutput>> _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomInjectedOperation&lt;TType, TInput, TOutput&gt;"/> class.
        /// </summary>
        /// <param name="function">The function to be executed against the injected type.</param>
        public CustomInjectedOperation(Func<TType, TInput, IEnumerable<TOutput>> function)
        {
            _function = function;
        }

        /// <summary>
        /// Gets the execution closure to be built into the pipeline.
        /// </summary>
        /// <returns>a closure to be used in the execution sequence</returns>
        public Func<IPipelineContext, TInput, IEnumerable<TOutput>> GetExecutor()
        {
            return (injector, o) =>
            {
                var instance = injector.TypeResolver.Resolve(typeof(TType)) as TType;
                return _function(instance, (TInput)o);
            };
        }
    }
}
