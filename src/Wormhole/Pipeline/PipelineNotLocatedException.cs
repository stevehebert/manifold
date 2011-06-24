using System;

namespace Wormhole.Pipeline
{
    public class PipelineNotLocatedException : Exception
    {
        public PipelineNotLocatedException() { }

        public PipelineNotLocatedException(string message) : base(message) { }

        public PipelineNotLocatedException(string message, Exception inner) : base(message, inner) { }

    }
}