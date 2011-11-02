using System.Collections.Generic;
using System.Linq;
using Manifold.Tests.SupportedContainers;
using NUnit.Framework;

namespace Manifold.Tests.Projector
{
    [TestFixture]
    class NamedProjectorTests
    {
        [TestCase(SupportedProviderType.Autofac)]
        //[TestCase(SupportedProviderType.Ninject)]
        public void delegate_based_named_projector(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType, item =>
                                                                                 {
                                                                                     item.RegisterProjector
                                                                                         <string, int, int>("foo")
                                                                                         .Bind(i => new[] {i*2, i*4})
                                                                                         .Bind(i => new[] {i*3, i*5});

                                                                                     item.RegisterProjector
                                                                                         <string, int, int>("bar")
                                                                                         .Bind(i => new[] {i*20, i*40})
                                                                                         .Bind(i => new[] {i*30, i*50});

                                                                                 });

            var function = module.Resolve<Pipe<string, int, IEnumerable<int>>>();

            // act
            var resolvedItems = function("foo", 5);

            // assert
            Assert.NotNull(resolvedItems);
            Assert.That(resolvedItems.Count(), Is.EqualTo(4));

            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(10));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(20));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(15));
            Assert.That(resolvedItems.ToArray()[3], Is.EqualTo(25));
        }

        [TestCase(SupportedProviderType.Autofac)]
        //[TestCase(SupportedProviderType.Ninject)]
        public void delegate_based_named_projector_alternate(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType, item =>
            {
                item.RegisterProjector
                    <string, int, int>("foo")
                    .Bind(i => new[] { i * 2, i * 4 })
                    .Bind(i => new[] { i * 3, i * 5 });

                item.RegisterProjector
                    <string, int, int>("bar")
                    .Bind(i => new[] { i * 20, i * 40 })
                    .Bind(i => new[] { i * 30, i * 50 });

            });

            var function = module.Resolve<Pipe<string, int, IEnumerable<int>>>();

            // act
            var resolvedItems = function("bar", 5);

            // assert
            Assert.NotNull(resolvedItems);
            Assert.That(resolvedItems.Count(), Is.EqualTo(4));

            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(100));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(200));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(150));
            Assert.That(resolvedItems.ToArray()[3], Is.EqualTo(250));
        }
    }
}
