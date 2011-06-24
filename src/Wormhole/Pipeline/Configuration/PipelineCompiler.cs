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
        private static Func<Tuple<IResolveTypes, object>, object, object, object> CreateExecutor(Tuple<Type, Func<object, object, object, object>> stateValuePair)
        {
            return (resolver, name, value) =>
                       {
                           object item = null;

                           if (stateValuePair.Item1 != null)
                               item = resolver.Item1.Resolve(stateValuePair.Item1);

                           return stateValuePair.Item2(resolver.Item2, item, value);
                       };
        }

        /// <summary>
        /// Compiles the specified pipeline data.
        /// </summary>
        /// <param name="pipelineData">The pipeline data.</param>
        /// <returns>the lambda lifted closure</returns>
        public static Func<Tuple<IResolveTypes,object>, object, object, object> Compile(PipelineData pipelineData)
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

                function = (resolver,name, value) => item(resolver, resolver.Item2, localFunction( resolver, resolver.Item2, value));
            }

            return function;
        }
    }
}
