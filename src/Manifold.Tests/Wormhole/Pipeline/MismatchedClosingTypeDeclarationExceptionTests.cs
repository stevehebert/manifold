using System;
using Manifold.Exceptions;
using NUnit.Framework;

namespace Manifold.Tests.Wormhole.Pipeline
{
    [TestFixture]
    public class MismatchedClosingTypeDeclarationExceptionTests
    {
        [Test]
        public void verify_exception_consistency()
        {
            Assert.That(new MismatchedClosingTypeDeclarationException("hello").Message, Is.EqualTo("hello"));
            Assert.That(new MismatchedClosingTypeDeclarationException("hello", new InvalidOperationException()).InnerException, Is.TypeOf<InvalidOperationException>());
        }
    }
}
