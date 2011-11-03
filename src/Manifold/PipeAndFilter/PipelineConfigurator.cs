using System;
using Manifold.Configuration.Pipeline;
using Manifold.Router;

namespace Manifold.PipeAndFilter
{
    public class PipelineConfigurator<TInput, TOutput>
    {
        private readonly PipeDefinition _pipeDefinition;

        public PipelineConfigurator(PipeDefinition pipeDefiniton )
        {
            _pipeDefinition = pipeDefiniton;
        }

        /// <summary>
        /// Bind an injected class that implements an intermediate output type
        /// </summary>
        /// <typeparam name="TType">The type of the injected instance. It must be of type IPipelineTask...</typeparam>
        /// <typeparam name="TOutputType">The intermediate output type</typeparam>
        /// <returns>a pipeline configurator that allows the configuration process to continue</returns>
        public PipelineConfigurator<TOutputType, TOutput> Bind<TType, TOutputType>() where TType : class, IPipelineTask<TInput, TOutputType>
        {
            _pipeDefinition.AddInjectedOperation<TType, TInput, TOutputType>(typeof(TOutput)==typeof(TOutputType));

            return new PipelineConfigurator<TOutputType, TOutput>(_pipeDefinition);
        }

        /// <summary>
        /// Bind an injected class that implements the required input and output types.
        /// </summary>
        /// <typeparam name="TType">The type of the injected instance. It must be of type IPipelineTask...</typeparam>
        /// <returns>a pipeline configurator that allows the configuration process to continue</returns>
        public PipelineConfigurator<TInput, TOutput> Bind<TType>() where TType : class, IPipelineTask<TInput, TOutput>
        {
            _pipeDefinition.AddInjectedOperation<TType, TInput, TOutput>(true);

            return new PipelineConfigurator<TInput, TOutput>(_pipeDefinition);
        }

        /// <summary>
        /// Binds a specified function that returns an intermediate output instance.
        /// </summary>
        /// <typeparam name="TOutputType">The intermediate output instance type.</typeparam>
        /// <param name="function">The function to be executed.</param>
        /// <returns>a pipeline configurator that allows the configuration process to continue</returns>
        public PipelineConfigurator<TOutputType, TOutput>Bind<TOutputType>(Func<TInput, TOutputType> function)
        {
            _pipeDefinition.AddFunctionOperation(function, typeof(TOutput) == typeof(TOutputType));
            return new PipelineConfigurator<TOutputType, TOutput>(_pipeDefinition);
        }

        /// <summary>
        /// Binds a specified function that implements the required input and output types.
        /// </summary>
        /// <param name="function">The function to be executed.</param>
        /// <returns>a pipeline configurator that allows the configuration process to continue</returns>
        public PipelineConfigurator<TOutput,TOutput> Bind(Func<TInput,TOutput> function )
        {
            _pipeDefinition.AddFunctionOperation(function, true);
            return new PipelineConfigurator<TOutput, TOutput>(_pipeDefinition);
        }

        /// <summary>
        /// Binds a custom injected type with a delegate that returns an intermediate output instance.
        /// </summary>
        /// <typeparam name="TType">The type of the injected class.</typeparam>
        /// <typeparam name="TOutputType">The type of the intermediate output type.</typeparam>
        /// <param name="function">The function to be executed.</param>
        /// <returns>a pipeline configurator that allows the configuration process to continue</returns>
        public PipelineConfigurator<TOutputType, TOutput> Bind<TType, TOutputType>(Func<TType, TInput, TOutputType> function ) where TType : class
        {
            _pipeDefinition.AddCustomInjectedOperations(function, typeof(TOutput) == typeof(TOutputType));
            return new PipelineConfigurator<TOutputType, TOutput>(_pipeDefinition);
        }

        /// <summary>
        /// Binds a custom injected type with a delegate using the required input and output types.
        /// </summary>
        /// <typeparam name="TType">The type of the injected class.</typeparam>
        /// <param name="function">The function to be executed.</param>
        /// <returns>a pipeline configurator that allows the configuration process to continue</returns>
        public PipelineConfigurator<TOutput, TOutput> Bind<TType>(Func<TType, TInput, TOutput> function ) where TType : class
        {
            _pipeDefinition.AddCustomInjectedOperations(function, true);
            return new PipelineConfigurator<TOutput, TOutput>(_pipeDefinition);
        }

        public PipelineConfigurator<TOutput, TOutput> ContinueWith()
        {
            _pipeDefinition.AddAnonymousContinuation<TInput, TOutput>(true);
            return new PipelineConfigurator<TOutput, TOutput>(_pipeDefinition);
        }

        public PipelineConfigurator<TOutputType, TOutput> ContinueWith<TOutputType>()
        {

            _pipeDefinition.AddAnonymousContinuation<TInput, TOutputType>(typeof(TOutput) == typeof(TOutputType));
            return new PipelineConfigurator<TOutputType, TOutput>(_pipeDefinition);
        }

        public PipelineConfigurator<TOutputType, TOutput> ContinueWith<TNameType, TOutputType>(TNameType name)
        {
            _pipeDefinition.AddNamedContinuation<TInput, TOutputType, TNameType>(name, typeof(TOutput) == typeof(TOutputType));
            return new PipelineConfigurator<TOutputType, TOutput>(_pipeDefinition);
        }

        public PipelineConfigurator<TOutput, TOutput> ContinueWith<TNameType>(TNameType name)
        {
            _pipeDefinition.AddNamedContinuation<TInput, TOutput, TNameType>(name, true);
            return new PipelineConfigurator<TOutput, TOutput>(_pipeDefinition);
        }

        //public PipelineConfigurator<TOutput, TOutput> ContextualContinuation<TType>()
        //{
        //    _builder.Add(a => a.Register<Functor<TType, TInput, TOutput>>(c =>
        //    {
        //        var item =
        //            c.Resolve(typeof(NamedResolver<TInput, TOutput>)) as
        //            NamedResolver<TInput, TOutput>;

        //        return
        //            (clarifier, input) => item.Execute(clarifier, input);
        //    }));

        //    return this;
        //}

        public PipelineRouterConfigurator<TInput, TOutput, TOutput> CreateRouter()
        {
            return new PipelineRouterConfigurator<TInput, TOutput, TOutput>(_pipeDefinition);
        }

        public PipelineRouterConfigurator<TInput, TOutputType, TOutput> CreateRouter<TOutputType>()
        {
            return new PipelineRouterConfigurator<TInput, TOutputType, TOutput>(_pipeDefinition);
        }
    }
}