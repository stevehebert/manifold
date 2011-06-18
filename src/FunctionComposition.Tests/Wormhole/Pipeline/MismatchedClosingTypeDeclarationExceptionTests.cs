using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Wormhole.Pipeline;

namespace Wormhole.Tests.Wormhole.Pipeline
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
