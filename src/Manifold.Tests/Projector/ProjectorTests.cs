using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Autofac;
using NUnit.Framework;

namespace Manifold.Tests.Projector
{
    [TestFixture]
    public class ProjectorTests
    {
        public class Projector : IPipelineTask<int, IEnumerable<int>>
        {
            public IEnumerable<int> Execute(int input)
            {
                yield return input*3;
                yield return input*5;
            }
        }

        public class SlowProjector : IPipelineTask<int, IEnumerable<int>>
        {
            public IEnumerable<int> Execute(int input)
            {
                yield return input * 3;
                Thread.Sleep(10000);
                yield return input * 5;
            }
        }



        [Test]
        public void delegate_based_projector()
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

        [Test]
        public void single_injected_projector_composition()
        {
            var module = new SimplePipelineModule(item => item.RegisterProjector<int, int>()
                                                              .Bind<Projector>());

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Pipe<int, IEnumerable<int>>>();

            var resolvedItems = function(5);

            Assert.NotNull(resolvedItems);
            Assert.That(resolvedItems.Count(), Is.EqualTo(2));

            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(15));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(25));
        }

        [Test]
        public void mixed_projector_composition()
        {
            var module = new SimplePipelineModule(item => item.RegisterProjector<int, int>()
                                                              .Bind<Projector>()
                                                              .Bind(input => new [] {input*100, input*1000}));

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Pipe<int, IEnumerable<int>>>();

            var resolvedItems = function(5);

            Assert.NotNull(resolvedItems);
            Assert.That(resolvedItems.Count(), Is.EqualTo(4));

            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(15));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(25));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(500));
            Assert.That(resolvedItems.ToArray()[3], Is.EqualTo(5000));
        }

        [Test]
        public void validate_short_circuit_capability()
        {
            var module = new SimplePipelineModule(item => item.RegisterProjector<int, int>()
                                                              .Bind<SlowProjector>());

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Pipe<int, IEnumerable<int>>>();

            var resolvedItems = function(5);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Assert.IsTrue(resolvedItems.Any());
            stopWatch.Stop();

            Assert.That(stopWatch.ElapsedMilliseconds, Is.LessThan(5000));

        }

    }
}
