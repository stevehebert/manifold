using System;
using NUnit.Framework;
using Wormhole.Pipeline;

namespace Wormhole.Tests.Wormhole.Pipeline
{
    [TestFixture]
    public class PipelineNotLocatedExceptionTests 
    {
        [Test]
        public void verify_exception_consistency()
        {
            Assert.That(new PipelineNotLocatedException().InnerException, Is.Null);
            Assert.That(new PipelineNotLocatedException("hello").Message, Is.EqualTo("hello"));
            Assert.That(new PipelineNotLocatedException("hello", new InvalidOperationException()).InnerException, Is.TypeOf<InvalidOperationException>());
        }
    }
}
