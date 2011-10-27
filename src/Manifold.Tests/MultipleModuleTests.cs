using System;
using Manifold.Tests.SupportedContainers;
using NUnit.Framework;

namespace Manifold.Tests
{
    [TestFixture]
    public class MultipleModuleTests
    {
        [Test]
        public void two_modules()
        {
            var module = new AutofacModule(p => p.RegisterPipeline<string, int, int>("foo").Bind(p1 => p1*2).
                                                    ContinueWith("bar"));
                
           
            var module2 = new AutofacModule(p => p.RegisterPipeline<string, int, int>("bar").Bind(p1 => p1 + 2));

            module2.AddAlternateModules(new[]{module});

            var function = module2.Resolve<Pipe<string, int, int>>();

            var value = function("foo", 15);

            Assert.That(value, Is.EqualTo(32));
        }
    }
}
