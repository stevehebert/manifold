using System.Collections.Generic;
using System.Linq;

namespace Wormhole
{
    public delegate TOutput Functor<in TInput, out TOutput>(TInput input);

    public delegate TOutput Functor<in TNameType, in TInput, out TOutput>(TNameType name, TInput input);

    public static class FunctorExtensions
    {
        /// <summary>
        /// classic fmap - given a function of type [a] and returning type [b], given an enumeration of [a]
        /// I will return an enumeration of [b]
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IEnumerable<TOutput> fmap<TInput, TOutput>(this Functor<TInput, TOutput> function, 
                                                                 IEnumerable<TInput> values)
        {
            return from p in values
                   select function(p);
        }


        /// <summary>
        /// strattling the lines between correctness and usability here.  It's handy to think of named functors
        /// as being part of the definition, but it's technically incorrect.
        /// 
        /// the method as defined is causing a lookup on each call.  this isn't entirely necessary if we think
        /// of the problem differently.  Name resolution could be a unary functor itself - given a name, retrieve
        /// me a functor.
        ///  
        /// var functor = locatorFunctor(name);
        /// var value[] = functor.fmap((input[]);
        /// --- or the user could do what we intrisically do here which is---
        /// var value[] = locatorFunctor(name)(input[]);
        /// --- as opposed to ---
        /// var value[] = locatingFunctor(name, input[]);
        /// </summary>
        /// <typeparam name="TNameType">The type of the name type.</typeparam>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="function">The function.</param>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        /// 
        /// Functor<string, Functor<int, model>>
        public static IEnumerable<TOutput> fmap<TNameType, TInput, TOutput>(this Functor<TNameType, TInput, TOutput> function, TNameType name,
                                                                 IEnumerable<TInput> values)
        {
            return from p in values
                   select function(name, p);
        }
    }
}
