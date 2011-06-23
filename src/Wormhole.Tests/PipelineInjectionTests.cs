using System.Collections.Generic;
using System.Linq; 
using Autofac;
using NUnit.Framework;
using Wormhole.Pipeline;

namespace Wormhole.Tests
{
    [TestFixture]
    public class PipelineInjectionTests
    {
        [Test]
        public void verify_ordered()
        {
            var module = new SimplePipelineModule(item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<int>>()
                                                              .Bind<Adder>()
                                                              .Bind<Divider>()
                                                              .Bind<NonConformingType>((a, input) => a.Run(input)));

            

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Functor<IEnumerable<int>, IEnumerable<int>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(12));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(22));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(32));
        }

        [Test]
        public void verify_inline_type_conversion()
        {
            var module = new SimplePipelineModule(item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                                                              .Bind<Adder, IEnumerable<int>>()
                                                              .Bind<Divider, IEnumerable<int>>()
                                                              .Bind<Stringifier>());
            
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
        public void verify_explicit_pipeline_resolution()
        {
            var module = new SimplePipelineModule(item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                                                              .Bind<Adder, IEnumerable<int>>()
                                                              .Bind<Divider, IEnumerable<int>>()
                                                              .Bind<Stringifier>());

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var pipeline = container.Resolve<Functor<IEnumerable<int>, IEnumerable<string>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = pipeline(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo("6"));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo("11"));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo("16"));
        }

        [Test]
        public void verify_malformed_type_conversion()
        {
            var module = new SimplePipelineModule(item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                                                              .Bind<Adder, IEnumerable<int>>()
                                                              .Bind<Divider, IEnumerable<int>>());

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            Assert.Throws<MismatchedClosingTypeDeclarationException>(() => builder.Build());
        }

        #region supporting injected classes
        public class Divider : IPipelineTask<IEnumerable<int>, IEnumerable<int>>
        {
            public IEnumerable<int> Execute(IEnumerable<int> input)
            {
                return from p in input select p / 2;
            }
        }

        public class NonConformingType
        {
            public IEnumerable<int> Run(IEnumerable<int> input)
            {
                return from p in input select p*2;
            }
        }

        public class Adder : IPipelineTask<IEnumerable<int>, IEnumerable<int>>
        {
            public IEnumerable<int> Execute(IEnumerable<int> input)
            {
                return from p in input select p + 2;
            }
        }

        public class Stringifier : IPipelineTask<IEnumerable<int>, IEnumerable<string>>
        {
            public IEnumerable<string> Execute(IEnumerable<int> input)
            {
                return from p in input select p.ToString();
            }
        }
        #endregion

    }
}
