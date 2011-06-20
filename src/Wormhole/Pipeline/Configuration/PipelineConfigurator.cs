using System;
using System.Collections.Generic;
using Wormhole.DependencyInjection;

namespace Wormhole.Pipeline.Configuration
{
    public class PipelineConfigurator<TInput, TOutput>
        where TInput : class
        where TOutput : class
    {
        private readonly PipelineData _registrarData;
        private readonly IList<Action<IRegisterTypes>> _builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineConfigurator&lt;TInput, TOutput&gt;"/> class.
        /// </summary>
        /// <param name="registrarData">The registrar data.</param>
        /// <param name="builder">The builder.</param>
        public PipelineConfigurator(PipelineData registrarData, IList<Action<IRegisterTypes>> builder)
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
            _builder.Add(a => a.RegisterType<TType>());

            _registrarData.Add(function, typeof(TOutputType) == typeof(TOutput));
            return new PipelineConfigurator<TOutputType, TOutput>(_registrarData, _builder);
        }

        public PipelineConfigurator<TOutput, TOutput> Bind<TType>(Func<TType, TInput, TOutput> function) where TType : class
        {
            _builder.Add( a=> a.RegisterType<TType>());

            _registrarData.Add(function, true);

            return new PipelineConfigurator<TOutput, TOutput>(_registrarData, _builder);
        }

        public PipelineConfigurator<TInput, TOutput> Bind<TType>() where TType : class, IPipelineTask<TInput, TOutput>
        {
            _builder.Add(a => a.RegisterType<TType>());

            _registrarData.Add<TType, TInput, TOutput>((a, b) => a.Execute(b), true);

            return new PipelineConfigurator<TInput, TOutput>(_registrarData, _builder);
        }

        
        public PipelineConfigurator<TOutputType, TOutput> Bind<TType, TOutputType>()
            where TType : class, IPipelineTask<TInput, TOutputType>
            where TOutputType : class
        {
            _builder.Add(a => a.RegisterType<TType>());

            _registrarData.Add<TType, TInput, TOutputType>((a, b) => a.Execute(b), typeof(TOutputType) == typeof(TOutput));

            return new PipelineConfigurator<TOutputType, TOutput>(_registrarData, _builder);
        }

        public PipelineConfigurator<TOutput, TOutput> Bind(Func<TInput, TOutput> function)
        {
            _registrarData.Add(function, true);

            return new PipelineConfigurator<TOutput, TOutput>(_registrarData, _builder);
        }

        public PipelineConfigurator<TOutputType, TOutput> Bind<TOutputType>(Func<TInput, TOutputType> function) where TOutputType : class
        {
            _registrarData.Add(function, typeof(TOutputType) == typeof(TOutput));

            return new PipelineConfigurator<TOutputType, TOutput>(_registrarData, _builder);
        }


        // chains the bind operation to an external named pipeline
        public PipelineConfigurator<TOutputType, TOutput> ContinueWith<TOutputType>(object name)
            where TOutputType : class
        {
            return
                Bind<NamedResolver<TInput, TOutputType>, TOutputType>(
                    (a, value) => a.Execute(name, value));
        }

        /// <summary>
        /// Chains the bind operation to an external default pipeline
        /// </summary>
        /// <typeparam name="TOutputType">The type of the output type.</typeparam>
        /// <returns></returns>
        public PipelineConfigurator<TOutputType, TOutput> ContinueWith<TOutputType>()
            where TOutputType : class
        {
            return ContinueWith<TOutputType>(new DefaultPipeline<TInput, TOutputType>());
        }

        // chains the bind operation to an external named pipeline
        public PipelineConfigurator<TOutput, TOutput> ContinueWith<TNameType>(TNameType name)
        {
            return
                Bind<NamedResolver<TInput, TOutput>, TOutput>(
                    (a, value) => a.Execute(name, value));
        }
    }
}
