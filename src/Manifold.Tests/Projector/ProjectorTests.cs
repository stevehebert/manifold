using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autofac;
using NUnit.Framework;

namespace Manifold.Tests.Projector
{
    [TestFixture]
    public class ProjectorTests
    {
        [Test]
        public void Test1()
        {
            var module = new SimplePipelineModule(item => item.RegisterProjector<int, int>()
                                                              .Bind(i => new[] {i*2, i*4})
                                                              .Bind(i => new[] {i*3, i*5}));



            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Pipe<int, IEnumerable<int>>>();
            
            var resolvedItems = function(5);

            Assert.NotNull(resolvedItems);
            Assert.That(resolvedItems.Count(), Is.EqualTo(4));

            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(10));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(20));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(15));
            Assert.That(resolvedItems.ToArray()[3], Is.EqualTo(25));
        }
    }
}
