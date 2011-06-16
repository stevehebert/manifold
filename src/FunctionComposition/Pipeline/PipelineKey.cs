using System;

namespace Wormhole.Pipeline
{
    public class PipelineKey
    {
        public Type Input { get; set; }
        public Type Output { get; set; }
        public object Named { get; set; }

        public override bool  Equals(object obj)
        {
            if (obj == null)
                return false;

            return GetHashCode() == obj.GetHashCode();
        }

        public override int  GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string  ToString()
        {
            return Input.GetHashCode() + ":" + Output.GetHashCode() + ":" + Named.GetHashCode();
        }
    }
}