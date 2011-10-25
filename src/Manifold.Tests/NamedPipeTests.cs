using System.Collections.Generic;
using System.Linq;
using Manifold.Tests.SupportedContainers;
using NUnit.Framework;

namespace Manifold.Tests
{
    [TestFixture]
    public class NamedPipeTests
    {
        [TestCase(SupportedProviderType.Autofac, 1)]
        [TestCase(SupportedProviderType.Autofac, 2)]
        [TestCase(SupportedProviderType.Ninject, 1)]
        [TestCase(SupportedProviderType.Ninject, 2)]
        public void simple_named_pipe_resolve(SupportedProviderType supportedProviderType, int pipeId)
        {
            var module = CommonModuleProvider.Create(SupportedProviderType.Autofac,
                                               item =>
                                                   {
                                                       item.RegisterPipeline<int, IEnumerable<int>, IEnumerable<string>>
                                                           (1)
                                                           .Bind(a => from p in a select p + 2)
                                                           .Bind(a => from p in a select p/2)
                                                           .Bind(a => from p in a select p.ToString());

                                                       item.RegisterPipeline<int, IEnumerable<int>, IEnumerable<string>>(2)
                                                           .Bind(a => from p in a select p * 2)
                                                           .Bind(a => from p in a select p.ToString());
                                                   });


            var function = module.Resolve<Pipe<int, IEnumerable<int>, IEnumerable<string>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(pipeId,items);

            if (pipeId == 1)
            {
                Assert.That(resolvedItems.Count(), Is.EqualTo(3));
                Assert.That(resolvedItems.ToArray()[0], Is.EqualTo("6"));
                Assert.That(resolvedItems.ToArray()[1], Is.EqualTo("11"));
                Assert.That(resolvedItems.ToArray()[2], Is.EqualTo("16"));
            }
            else
            {
                Assert.That(resolvedItems.Count(), Is.EqualTo(3));
                Assert.That(resolvedItems.ToArray()[0], Is.EqualTo("20"));
                Assert.That(resolvedItems.ToArray()[1], Is.EqualTo("40"));
                Assert.That(resolvedItems.ToArray()[2], Is.EqualTo("60"));
            }
        }


        [TestCase(SupportedProviderType.Autofac, 1)]
        [TestCase(SupportedProviderType.Autofac, 2)]
        [TestCase(SupportedProviderType.Ninject, 1)]
        [TestCase(SupportedProviderType.Ninject, 2)]
        public void named_continuation_pipe_resolve(SupportedProviderType supportedProviderType, int pipeId)
        {
            var module = CommonModuleProvider.Create(SupportedProviderType.Autofac,
                                               item =>
                                               {
                                                   item.RegisterPipeline<string, IEnumerable<int>, IEnumerable<string>>("foo")
                                                       .Bind(a => from p in a select p.ToString());

                                                   item.RegisterPipeline<int, IEnumerable<int>, IEnumerable<string>>
                                                       (1)
                                                       .Bind(a => from p in a select p + 2)
                                                       .Bind(a => from p in a select p/2)
                                                       .ContinueWith("foo");

                                                   item.RegisterPipeline<int, IEnumerable<int>, IEnumerable<string>>(2)
                                                       .Bind(a => from p in a select p * 2)
                                                       .ContinueWith("foo");
                                                       
                                               });


            var function = module.Resolve<Pipe<int, IEnumerable<int>, IEnumerable<string>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(pipeId, items);

            if (pipeId == 1)
            {
                Assert.That(resolvedItems.Count(), Is.EqualTo(3));
                Assert.That(resolvedItems.ToArray()[0], Is.EqualTo("6"));
                Assert.That(resolvedItems.ToArray()[1], Is.EqualTo("11"));
                Assert.That(resolvedItems.ToArray()[2], Is.EqualTo("16"));
            }
            else
            {
                Assert.That(resolvedItems.Count(), Is.EqualTo(3));
                Assert.That(resolvedItems.ToArray()[0], Is.EqualTo("20"));
                Assert.That(resolvedItems.ToArray()[1], Is.EqualTo("40"));
                Assert.That(resolvedItems.ToArray()[2], Is.EqualTo("60"));
            }
        }

    
    }
}
