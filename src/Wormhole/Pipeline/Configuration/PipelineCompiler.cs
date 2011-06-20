using System;
using Wormhole.DependencyInjection;

namespace Wormhole.Pipeline.Configuration
{
    public class PipelineCompiler
    {
        /// <summary>
        /// Creates the executor which can resolve a passed type and hand it
        /// to the closure
        /// </summary>
        /// <param name="stateValuePair">The state value pair.</param>
        /// <returns>a lambda lifted closure</returns>
        private static Func<IResolveTypes, object, object> CreateExecutor(Tuple<Type, Func<object, object, object>> stateValuePair)
        {
            return (resolver, value) =>
                       {
                           object item = null;

                           if (stateValuePair.Item1 != null)
                               item = resolver.Resolve(stateValuePair.Item1);

                           return stateValuePair.Item2(item, value);
                       };
        }

        /// <summary>
        /// Compiles the specified pipeline data.
        /// </summary>
        /// <param name="pipelineData">The pipeline data.</param>
        /// <returns>the lambda lifted closure</returns>
        public static Func<IResolveTypes, object, object> Compile(PipelineData pipelineData)
        {
            if (!pipelineData.IsClosed)
                throw new MismatchedClosingTypeDeclarationException();

            //// here we roll the composition of function into an y=h(g(f(x))) delegate
            //// this is a bit more contrived than we ultimately want, but it was more
            //// entertaining to write it this way for now.

            var function = CreateExecutor(pipelineData.FunctionList.Dequeue());

            while(pipelineData.FunctionList.Count > 0)
            {
                var item = CreateExecutor(pipelineData.FunctionList.Dequeue());
                var localFunction = function;

                function = (resolver, value) => item(resolver, localFunction(resolver, value));
            }

            return function;
        }
    }
}
