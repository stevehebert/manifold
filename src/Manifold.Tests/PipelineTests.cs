using System.Collections.Generic;
using System.Linq;
using Manifold.Exceptions;
using Manifold.Tests.SupportedContainers;
using NUnit.Framework;

namespace Manifold.Tests
{
    [TestFixture]
    public class PipelineTests
    {
        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void verify_simplest_construction(SupportedProviderType supportedProviderType)
        {
            var module = ModuleProvider.Create(supportedProviderType,
                                               item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<int>>()
                                                           .Bind(a => from p in a select p/2));

           
            var function = module.Resolve<Pipe<IEnumerable<int>, IEnumerable<int>>>();

            var items = new[] {10, 20, 30};

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));

            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(5));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(10));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(15));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void verify_ordered_construction(SupportedProviderType supportedProviderType)
        {
            var module = ModuleProvider.Create(supportedProviderType, item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<int>>()
                                                              .Bind(a => from p in a select p / 2)
                                                              .Bind(a => from p in a select p + 2));

            
            var function = module.Resolve<Pipe<IEnumerable<int>, IEnumerable<int>>>();
            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(7));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(12));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(17));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void verify_alternate_ordered_construction(SupportedProviderType supportedProviderType)
        {
            var module = ModuleProvider.Create(supportedProviderType, item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<int>>()
                                                              .Bind(a => from p in a select p + 2)
                                                              .Bind(a => from p in a select p / 2));
            
            var function = module.Resolve<Pipe<IEnumerable<int>, IEnumerable<int>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(6));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(11));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(16));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void verify_type_conversion(SupportedProviderType supportedProviderType)
        {
            var module = ModuleProvider.Create(supportedProviderType,
                                               item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                                                           .Bind(a => from p in a select p + 2)
                                                           .Bind(a => from p in a select p/2)
                                                           .Bind(a => from p in a select p.ToString()));
                

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
        public void verify_incomplete_registration_error(SupportedProviderType supportedProviderType)
        {
            var module = ModuleProvider.Create(supportedProviderType,
                                               item => item.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                                                           .Bind(a => from p in a select p + 2)
                                                           .Bind(a => from p in a select p/2));

            Assert.Throws<MismatchedClosingTypeDeclarationException>(module.Build);
        }

    }
}
