﻿using System;
using System.Collections.Generic;
using Manifold.Configuration.Pipeline.Operations;
using Manifold.DependencyInjection;

namespace Manifold.Configuration.Projector.Operations
{
    /// <summary>
    /// This type allows for the injection of a type that does not follow the standard pattern, but
    /// allows the user to declare a function that accesses that instance with the input instance.
    /// </summary>
    /// <typeparam name="TType">The type of the instance being injected.</typeparam>
    /// <typeparam name="TInput">The type of the input instance.</typeparam>
    /// <typeparam name="TOutput">The type of the output instance.</typeparam>
    public class CustomInjectedProjectorOperation<TType, TInput, TOutput> : IProjectorOperation<TInput, TOutput> where TType : class
    {
        private readonly Func<TType, TInput, IEnumerable<TOutput>> _function;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomInjectedOperation&lt;TType, TInput, TOutput&gt;"/> class.
        /// </summary>
        /// <param name="function">The function to be executed against the injected type.</param>
        public CustomInjectedProjectorOperation(Func<TType, TInput, IEnumerable<TOutput>> function)
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
                return _function(instance, o);
            };
        }
    }
}
