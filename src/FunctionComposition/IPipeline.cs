using System;

namespace Wormhole
{
    public interface IPipeline<TNameType, TInput, TOutput >
    {
        /// <summary>
        /// Executes the specified named function.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        TOutput Execute(TNameType name, TInput input);
    }


    public interface IPipeline<TInput, TOutput>
    {
        /// <summary>
        /// Executes the specified function.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The output.</returns>
        TOutput Execute(TInput input);
    }
}
