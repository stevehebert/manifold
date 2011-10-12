using System.Collections.Generic;
using System.Linq;
using Autofac;
using NUnit.Framework;

namespace Manifold.Tests
{
    [TestFixture]
    public class ContinuationTests
    {
        [Test]
        public void verify_start_up_continue_operation()
        {
            var module = new SimplePipelineModule(item =>
                                                      {
                                                          item.RegisterPipeline<IEnumerable<int>, IEnumerable<int>>()
                                                              .Bind<PipelineInjectionTests.Adder, IEnumerable<int>>()
                                                              .Bind<PipelineInjectionTests.Divider, IEnumerable<int>>();

                                                          item.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                                                              .ContinueWith<IEnumerable<int>>()
                                                              .Bind<PipelineInjectionTests.Stringifier>();
                                                      });

            

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Pipe<IEnumerable<int>, IEnumerable<string>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo("6"));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo("11"));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo("16"));
        }


        [Test]
        public void verify_named_start_up_continue_operation()
        {
            var module = new SimplePipelineModule(item =>
                                                      {
                                                          item.RegisterPipeline<IEnumerable<int>, IEnumerable<int>>()
                                                              .Bind<PipelineInjectionTests.Adder, IEnumerable<int>>()
                                                              .Bind<PipelineInjectionTests.Divider, IEnumerable<int>>();

                                                          item.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                                                              .ContinueWith<IEnumerable<int>>()
                                                              .Bind<PipelineInjectionTests.Stringifier>();
                                                      });

            

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Pipe<IEnumerable<int>, IEnumerable<string>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo("6"));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo("11"));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo("16"));
        }

        [Test]
        public void verify_ending_continue_operation()
        {
            var module = new SimplePipelineModule(item =>
            {
                item.RegisterPipeline<IEnumerable<int>, IEnumerable<string>>()
                    .Bind<PipelineInjectionTests.Adder, IEnumerable<int>>()
                    .Bind<PipelineInjectionTests.Divider, IEnumerable<int>>()
                    .ContinueWith("foo");


                item.RegisterPipeline<string, IEnumerable<int>, IEnumerable<string>>("foo")
                    .Bind<PipelineInjectionTests.Stringifier>();
            });

            

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Pipe<IEnumerable<int>, IEnumerable<string>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo("6"));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo("11"));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo("16"));
        }

    }
}
