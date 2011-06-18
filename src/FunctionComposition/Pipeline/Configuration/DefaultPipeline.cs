using System;

namespace Wormhole.Pipeline.Configuration
{
    public class DefaultPipeline<TInput, TOutput>
    {
        public Type Input { get { return typeof (TInput); } }

        public Type Output { get { return typeof (TOutput); } }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return (Input.FullName + ":" + Output.FullName);
        }
    }
}