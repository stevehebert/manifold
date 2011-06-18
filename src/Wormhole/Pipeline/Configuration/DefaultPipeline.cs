using System;

namespace Wormhole.Pipeline.Configuration
{
    public class DefaultPipeline<TInput, TOutput>
    {
        /// <summary>
        /// Gets the input type
        /// </summary>
        /// <value>The input type</value>
        public Type Input { get { return typeof (TInput); } }

        /// <summary>
        /// Gets the output type
        /// </summary>
        /// <value>The output type</value>
        public Type Output { get { return typeof (TOutput); } }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return (Input.FullName + ":" + Output.FullName);
        }
    }
}