using System;
using Wormhole.DependencyInjection;

namespace Wormhole.Pipeline
{
    public class PipelineConfigurator<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        private readonly PipelineData _registrarData;
        private readonly IRegisterTypes _builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineConfigurator&lt;TInput, TOutput&gt;"/> class.
        /// </summary>
        /// <param name="registrarData">The registrar data.</param>
        /// <param name="builder">The builder.</param>
        public PipelineConfigurator(PipelineData registrarData, IRegisterTypes builder)
        {
            _registrarData = registrarData;
            _builder = builder;
        }


        /// <summary>
        /// Binds the specified function.
        /// </summary>
        /// <typeparam name="TType">The type of the type.</typeparam>
        /// <typeparam name="TOutputType">The type of the output type.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns></returns>
        public PipelineConfigurator<TOutputType, TOutput> Bind<TType, TOutputType>(Func<TType, TInput, TOutputType> function)
            where TOutputType : class
            where TType : class
        {
            _builder.RegisterType<TType>();

            _registrarData.Add(function, typeof(TOutputType) == typeof(TOutput));
            return new PipelineConfigurator<TOutputType, TOutput>(_registrarData, _builder);
        }



        public PipelineConfigurator<TOutput, TOutput> Bind<TType>(Func<TType, TInput, TOutput> function) where TType : class
        {
            _builder.RegisterType<TType>();

            _registrarData.Add(function, true);

            return new PipelineConfigurator<TOutput, TOutput>(_registrarData, _builder);
        }

        public PipelineConfigurator<TInput, TOutput> Bind<TType>() where TType : class, IWormholeTask<TInput, TOutput>
        {
            _builder.RegisterType<TType>();

            _registrarData.Add<TType, TInput, TOutput>((a, b) => a.Execute(b), true);

            return new PipelineConfigurator<TInput, TOutput>(_registrarData, _builder);
        }

        public PipelineConfigurator<TOutputType, TOutput> Bind<TType, TOutputType>()
            where TType : class, IWormholeTask<TInput, TOutputType>
            where TOutputType : class
        {
            _builder.RegisterType<TType>();

            _registrarData.Add<TType, TInput, TOutputType>((a, b) => a.Execute(b), typeof(TOutputType) == typeof(TOutput));

            return new PipelineConfigurator<TOutputType, TOutput>(_registrarData, _builder);
        }

        public PipelineConfigurator<TOutputType, TOutput> Bind<TOutputType>(Func<TInput, TOutputType> function) where TOutputType : class
        {
            _registrarData.Add(function, typeof(TOutputType) == typeof(TOutput));

            return new PipelineConfigurator<TOutputType, TOutput>(_registrarData, _builder);
        }


        /// <summary>
        /// Chains the bind operation to an external pipeline
        /// </summary>
        /// <typeparam name="TOutputType">The type of the output type.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns></returns>
        public PipelineConfigurator<TOutputType, TOutput> ContinueWith<TOutputType>(Func<IFunctionExecutor<TInput, TOutputType>> function)
            where TOutputType : class
        {
            // TODO
            //return Bind(function);
            return null;
        }
    }
}
