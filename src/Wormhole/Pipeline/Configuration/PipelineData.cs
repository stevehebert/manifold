using System;
using System.Collections.Generic;

namespace Wormhole.Pipeline.Configuration
{
    public class PipelineData
    {
        public bool IsClosed { get; private set; }
        readonly Queue<Tuple<Type, Func<object, object, object>>> _functionList 
            = new Queue<Tuple<Type, Func<object, object, object>>>();

        /// <summary>
        /// Adds the specified function to the resolution list
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="isComplete">if set to <c>true</c> [is complete].</param>
        public void Add<TType, TInput, TOutput>(Func<TType, TInput, TOutput> function, bool isComplete)
        {
            IsClosed = isComplete;
            _functionList.Enqueue(new Tuple<Type, Func<object, object, object>>(typeof(TType), (type, inparam) => function((TType)type, (TInput)inparam )));
        }

        /// <summary>
        /// Adds the specified function.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="isComplete">if set to <c>true</c> [is complete].</param>
        public void Add<TInput, TOutput>(Func<TInput, TOutput> function, bool isComplete)
        {
            IsClosed = isComplete;
            _functionList.Enqueue(new Tuple<Type, Func<object, object, object>>(null, (a, b) => function((TInput) b)));
        }

        /// <summary>
        /// Gets the function list.
        /// </summary>
        /// <value>The function list.</value>
        public Queue<Tuple<Type, Func<object, object, object>>> FunctionList { get { return _functionList; } }
    }
}
