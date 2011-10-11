using System;

namespace Wormhole.Exceptions
{
    public class PipelineNotLocatedException : Exception
    {
        public PipelineNotLocatedException() { }

        public PipelineNotLocatedException(string message) : base(message) { }

        public PipelineNotLocatedException(string message, Exception inner) : base(message, inner) { }

    }
}