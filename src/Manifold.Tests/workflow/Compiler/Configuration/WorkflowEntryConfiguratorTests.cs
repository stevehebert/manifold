using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Manifold.Workflow.Compiler.Configuration;
using NUnit.Framework;

namespace Manifold.Tests.Workflow.Compiler.Configuration
{
    [TestFixture]
    public class WorkflowEntryConfiguratorTests
    {
        [Test]
        public void verify_exit_definition_call()
        {
            // arrange 
            var callCount = 0;
            var item = new WorkflowEntryConfigurator<int, string, int, int>(1, "2", (a, b, c) => callCount++);

            // act
            var value = item.OnExit();

            Assert.That(value, Is.Not.Null);
            Assert.That(callCount, Is.EqualTo(0));
        }

    }
}
