using System;

namespace Wormhole.DependencyInjection
{
    /// <summary>
    /// This type allows for the injection of a type that does not follow the standard pattern, but
    /// allows the user to declare a function that accesses that instance with the input instance.
    /// </summary>
    /// <typeparam name="TType">The type of the instance being injected.</typeparam>
    /// <typeparam name="TInput">The type of the input instance.</typeparam>
    /// <typeparam name="TOutput">The type of the output instance.</typeparam>
    public class CustomInjectedOperation<TType, TInput, TOutput> : IOperation where TType : class
    {
        private readonly Func<TType, TInput, TOutput> _function;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomInjectedOperation&lt;TType, TInput, TOutput&gt;"/> class.
        /// </summary>
        /// <param name="function">The function to be executed against the injected type.</param>
        public CustomInjectedOperation(Func<TType, TInput, TOutput> function )
        {
            _function = function;
        }

        /// <summary>
        /// Gets the execution closure to be built into the pipeline.
        /// </summary>
        /// <returns>a closure to be used in the execution sequence</returns>
        public Func<IResolveTypes, object, object> GetClosure()
        {
            return (injector, o) =>
                       {
                           var instance = injector.Resolve(typeof (TType)) as TType;
                           return _function(instance, (TInput) o);
                       };
        }
    }
}