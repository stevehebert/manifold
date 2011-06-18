using System;
using Wormhole.DependencyInjection;

namespace Wormhole.Pipeline.Configuration
{
    public class PipelineCompiler
    {
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


        public static Func<IResolveTypes, object, object> Compile(PipelineData pipelineData)
        {
            if (!pipelineData.IsClosed)
                throw new MismatchedClosingTypeDeclarationException();

            // here we roll the composition of function into an y=h(g(f(x))) delegate
            // started with an Expression that would be built and compiled, but this is cleaner and faster

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
