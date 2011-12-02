using System.Collections.Generic;
using System.Linq;
using Manifold.Exceptions;
using Manifold.Tests.SupportedContainers;
using NUnit.Framework;

namespace Manifold.Tests
{
    [TestFixture]
    public class PipelineInjectionTests
    {
        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void verify_ordered(SupportedProviderType supportedProviderType)
        {
            var module = CommonModuleProvider.Create(supportedProviderType,
                                               item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<int>>()
                                                           .Bind<Adder>()
                                                           .Bind<Divider>()
                                                           .Bind<NonConformingType>((a, input) => a.Run(input)));

            var function = module.Resolve<Pipe<IEnumerable<int>, IEnumerable<int>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(12));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(22));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(32));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void verify_pipe_injection(SupportedProviderType supportedProviderType)
        {
            var module = CommonModuleProvider.Create(supportedProviderType,
                                               item =>
                                                   {
                                                       item.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                                                           .Bind<Pipe<IEnumerable<int>, IEnumerable<int>>, IEnumerable<int>>((p, input) => p(input))
                                                           .Bind(p => from i in p select i.ToString());

                                                       item.RegisterPipeline<IEnumerable<int>, IEnumerable<int>>()
                                                           .Bind<Adder>()
                                                           .Bind<Divider>();
                                                   });


            var function = module.Resolve<Pipe<IEnumerable<int>, IEnumerable<string>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo("6"));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo("11"));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo("16"));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void verify_inline_type_conversion(SupportedProviderType supportedProviderType)
        {
            var module = CommonModuleProvider.Create(supportedProviderType,
                                               item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                                                           .Bind<Adder, IEnumerable<int>>()
                                                           .Bind<Divider, IEnumerable<int>>()
                                                           .Bind<Stringifier>());
            
 
            var function = module.Resolve<Pipe<IEnumerable<int>, IEnumerable<string>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo("6"));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo("11"));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo("16"));
        }


        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void verify_explicit_pipeline_resolution(SupportedProviderType supportedProviderType)
        {
            var module = CommonModuleProvider.Create(supportedProviderType,
                                               item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                                                           .Bind<Adder, IEnumerable<int>>()
                                                           .Bind<Divider, IEnumerable<int>>()
                                                           .Bind<Stringifier>());

            var pipeline = module.Resolve<Pipe<IEnumerable<int>, IEnumerable<string>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = pipeline(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo("6"));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo("11"));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo("16"));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void verify_malformed_type_conversion(SupportedProviderType supportedProviderType)
        {
            var module = CommonModuleProvider.Create(supportedProviderType,
                                               item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                                                           .Bind<Adder, IEnumerable<int>>()
                                                           .Bind<Divider, IEnumerable<int>>());

            Assert.Throws<MismatchedClosingTypeDeclarationException>(() => module.Resolve<Pipe<IEnumerable<int>, IEnumerable<string>>>());
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
