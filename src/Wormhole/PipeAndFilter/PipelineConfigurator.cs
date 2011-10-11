using System;
using Wormhole.Configuration;
using Wormhole.Pipeline;

namespace Wormhole.PipeAndFilter
{
    public class PipelineConfigurator<TInput, TOutput>
    {
        private readonly PipelineDefinition _pipelineDefinition;

        public PipelineConfigurator(PipelineDefinition pipelineDefiniton )
        {
            _pipelineDefinition = pipelineDefiniton;
        }

        /// <summary>
        /// Bind an injected class that implements an intermediate output type
        /// </summary>
        /// <typeparam name="TType">The type of the injected instance. It must be of type IPipelineTask...</typeparam>
        /// <typeparam name="TOutputType">The intermediate output type</typeparam>
        /// <returns>a pipeline configurator that allows the configuration process to continue</returns>
        public PipelineConfigurator<TOutputType, TOutput> Bind<TType, TOutputType>() where TType : class, IPipelineTask<TInput, TOutputType>
        {
            _pipelineDefinition.AddInjectedOperation<TType, TInput, TOutputType>(typeof(TOutput)==typeof(TOutputType));

            return new PipelineConfigurator<TOutputType, TOutput>(_pipelineDefinition);
        }

        /// <summary>
        /// Bind an injected class that implements the required input and output types.
        /// </summary>
        /// <typeparam name="TType">The type of the injected instance. It must be of type IPipelineTask...</typeparam>
        /// <returns>a pipeline configurator that allows the configuration process to continue</returns>
        public PipelineConfigurator<TInput, TOutput> Bind<TType>() where TType : class, IPipelineTask<TInput, TOutput>
        {
            _pipelineDefinition.AddInjectedOperation<TType, TInput, TOutput>(true);

            return new PipelineConfigurator<TInput, TOutput>(_pipelineDefinition);
        }

        /// <summary>
        /// Binds a specified function that returns an intermediate output instance.
        /// </summary>
        /// <typeparam name="TOutputType">The intermediate output instance type.</typeparam>
        /// <param name="function">The function to be executed.</param>
        /// <returns>a pipeline configurator that allows the configuration process to continue</returns>
        public PipelineConfigurator<TOutputType, TOutput>Bind<TOutputType>(Func<TInput, TOutputType> function)
        {
            _pipelineDefinition.AddFunctionOperation(function, typeof(TOutput) == typeof(TOutputType));
            return new PipelineConfigurator<TOutputType, TOutput>(_pipelineDefinition);
        }

        /// <summary>
        /// Binds a specified function that implements the required input and output types.
        /// </summary>
        /// <param name="function">The function to be executed.</param>
        /// <returns>a pipeline configurator that allows the configuration process to continue</returns>
        public PipelineConfigurator<TOutput,TOutput> Bind(Func<TInput,TOutput> function )
        {
            _pipelineDefinition.AddFunctionOperation(function, true);
            return new PipelineConfigurator<TOutput, TOutput>(_pipelineDefinition);
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
            _pipelineDefinition.AddCustomInjectedOperations(function, typeof(TOutput) == typeof(TOutputType));
            return new PipelineConfigurator<TOutputType, TOutput>(_pipelineDefinition);
        }

        /// <summary>
        /// Binds a custom injected type with a delegate using the required input and output types.
        /// </summary>
        /// <typeparam name="TType">The type of the injected class.</typeparam>
        /// <param name="function">The function to be executed.</param>
        /// <returns>a pipeline configurator that allows the configuration process to continue</returns>
        public PipelineConfigurator<TOutput, TOutput> Bind<TType>(Func<TType, TInput, TOutput> function ) where TType : class
        {
            _pipelineDefinition.AddCustomInjectedOperations(function, true);
            return new PipelineConfigurator<TOutput, TOutput>(_pipelineDefinition);
        }

        public PipelineConfigurator<TOutput, TOutput> ContinueWith()
        {
            _pipelineDefinition.AddNamedContinuation<TInput, TOutput, DefaultPipeline<TInput, TOutput>>(new DefaultPipeline<TInput,TOutput>(), true);
            return new PipelineConfigurator<TOutput, TOutput>(_pipelineDefinition);
        }

        public PipelineConfigurator<TOutputType, TOutput> ContinueWith<TOutputType>()
        {
            _pipelineDefinition.AddNamedContinuation<TInput, TOutputType, DefaultPipeline<TInput, TOutput>>(new DefaultPipeline<TInput, TOutput>(), typeof(TOutput) == typeof(TOutputType));
            return new PipelineConfigurator<TOutputType, TOutput>(_pipelineDefinition);
        }

        public PipelineConfigurator<TOutputType, TOutput> ContinueWith<TNameType, TOutputType>(TNameType name)
        {
            _pipelineDefinition.AddNamedContinuation<TInput, TOutputType, TNameType>(name, typeof(TOutput) == typeof(TOutputType));
            return new PipelineConfigurator<TOutputType, TOutput>(_pipelineDefinition);
        }

        public PipelineConfigurator<TOutput, TOutput> ContinueWith<TNameType>(TNameType name)
        {
            _pipelineDefinition.AddNamedContinuation<TInput, TOutput, TNameType>(name, true);
            return new PipelineConfigurator<TOutput, TOutput>(_pipelineDefinition);
        }
    }
}