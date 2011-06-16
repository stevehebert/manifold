using System;

namespace Wormhole.Pipeline
{
    public class DefaultPipeline<TInput, TOutput>
    {
        public Type Input { get { return typeof (TInput); } }

        public Type Output { get { return typeof (TOutput); } }

        public override int GetHashCode()
        {
            return (Input.FullName + ":" + Output.FullName).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is DefaultPipeline<TInput, TOutput>)
                return obj.GetHashCode() == GetHashCode();

            return false;
        }
    }
}