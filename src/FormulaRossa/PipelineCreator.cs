using System;
using System.Collections.Generic;

namespace FormulaRossa
{
    public interface IPipelineCreator
    {
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>();
        PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name);


    }
    class PipelineCreator :  IPipelineCreator
    {
        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TInput, TOutput>()
        {
            throw new NotImplementedException();
        }



        public PipelineConfigurator<TInput, TOutput> RegisterPipeline<TNameType, TInput, TOutput>(TNameType name)
        {
            throw new NotImplementedException();
        }
    }

    public class PipelineConfigurator<TInput, TOutput>
    {
        private readonly IList<IOperation> _operations;
        private readonly IList<Type> _registrationTypes;

        public PipelineConfigurator(IList<IOperation> operations, IList<Type> registrationTypes)
        {
            _operations = operations;
            _registrationTypes = registrationTypes;
        }

        public PipelineConfigurator<TOutputType, TOutput> Bind<TType, TOutputType>()  where TType:class, IPipelineTask<TInput,TOutputType>
        {
            _registrationTypes.Add(typeof(TType));
            _operations.Add(new InjectedOperation<TType, TInput, TOutputType>());
            return new PipelineConfigurator<TOutputType, TOutput>(_operations, _registrationTypes);
        }

        public PipelineConfigurator<TInput, TOutput>  Bind<TType>()  where TType:class, IPipelineTask<TInput, TOutput>
        {
            _registrationTypes.Add(typeof(TType));
            _operations.Add(new InjectedOperation<TType, TInput, TOutput>() );
            return new PipelineConfigurator<TInput, TOutput>(_operations, _registrationTypes);
        }
    }
}
