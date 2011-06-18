using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NUnit.Framework;
using Wormhole.Autofac;
using Wormhole.Pipeline;

namespace Wormhole.Tests
{
    [TestFixture]
    public class PipelineTests
    {
        [Test]
        public void verify_simplest_construction()
        {
            var module = new PipelineModule();

            module.RegisterPipeline<IEnumerable<int>, IEnumerable<int>>()
                .Bind(a => from p in a select p/2);

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Functor<IEnumerable<int>, IEnumerable<int>>>();

            var items = new[] {10, 20, 30};

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));

            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(5));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(10));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(15));
        }

        [Test]
        public void verify_ordered_construction()
        {
            var module = new PipelineModule();

            module.RegisterPipeline<IEnumerable<int>, IEnumerable<int>>()
                .Bind(a => from p in a select p / 2)
                .Bind(a => from p in a select p + 2);

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Functor<IEnumerable<int>, IEnumerable<int>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(7));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(12));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(17));
        }

        [Test]
        public void verify_alternate_ordered_construction()
        {
            var module = new PipelineModule();

            module.RegisterPipeline<IEnumerable<int>, IEnumerable<int>>()
                .Bind(a => from p in a select p + 2)
                .Bind(a => from p in a select p/2);
                
                
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Functor<IEnumerable<int>, IEnumerable<int>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(6));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(11));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(16));
        }

        [Test]
        public void verify_type_conversion()
        {
            var module = new PipelineModule();

            module.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                .Bind(a => from p in a select p + 2)
                .Bind(a => from p in a select p / 2 )
                .Bind(a => from p in a select p.ToString());
                
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Functor<IEnumerable<int>, IEnumerable<string>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo("6"));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo("11"));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo("16"));
            
        }
        [Test]
        public void verify_incomplete_registration_error()
        {
            var module = new PipelineModule();

            module.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                .Bind(a => from p in a select p + 2)
                .Bind(a => from p in a select p/2);

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);

            Assert.Throws<MismatchedClosingTypeDeclarationException>(() => builder.Build());
        }

    }
}
