using System.Collections.Generic;
using System.Linq;

namespace FormulaRossa
{
    public delegate TOutput Pipeline<in TInput, out TOutput>(TInput input);

    public delegate TOutput Pipeline<in TNameType, in TInput, out TOutput>(TNameType name, TInput input);

    public static class PipelineExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="function"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<TOutput> SelectMany<TInput, TOutput>(this Pipeline<TInput, TOutput> function,
                                                                 IEnumerable<TInput> values)
        {
            return from p in values
                   select function(p);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TNameType"></typeparam>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="function"></param>
        /// <param name="name"></param>
        /// <param name="values"></param>
        /// <returns></returns>
         public static IEnumerable<TOutput> SelectMany<TNameType, TInput, TOutput>(this Pipeline<TNameType, TInput, TOutput> function, TNameType name,
                                                                 IEnumerable<TInput> values)
        {
            return from p in values
                   select function(name, p);
        }
    }

}
