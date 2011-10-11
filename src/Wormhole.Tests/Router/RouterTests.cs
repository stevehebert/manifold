using System.Linq;
using Autofac;
using NUnit.Framework;

namespace Wormhole.Tests.Router
{
    [TestFixture]
    public class RouterTests
    {
        [Test]
        public void simple_single_route_test_behavior()
        {
            // arrange
            var module =
                new SimplePipelineModule(
                    e =>
                    e.RegisterPipeline<int, int>()
                        .CreateRouter()
                            .BindConditional(i => i > 0, x => x*10)
                            .Default(x => x*100));

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            // act
            var function = container.Resolve<Pipe<int, int>>();

            // assert
            Assert.That(function(10), Is.EqualTo(100));
            Assert.That(function(-10), Is.EqualTo(-1000));
        }

        [Test]
        public void multi_path_single_route_test_behavior()
        {
            // arrange
            var module =
                new SimplePipelineModule(
                    e =>
                    e.RegisterPipeline<int, int>()
                        .CreateRouter()
                            .BindConditional(i => i > 20, x => x * 10)
                            .BindConditional(i => i > 10, x => x * 100)
                            .Default(x => x * 1000));

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            // act
            var function = container.Resolve<Pipe<int, int>>();

            // assert
            Assert.That(function(21), Is.EqualTo(210));
            Assert.That(function(11), Is.EqualTo(1100));
            Assert.That(function(9), Is.EqualTo(9000));
        }

        [Test]
        public void test_multi_path_single_route_projection_mapping()
        {
            // arrange
            var module =
                new SimplePipelineModule(
                    e =>
                    e.RegisterPipeline<int, int>()
                        .CreateRouter()
                            .BindConditional(i => i > 20, x => x * 10)
                            .BindConditional(i => i > 10, x => x * 100)
                            .Default(x => x * 1000));

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            // act
            var function = container.Resolve<Pipe<int, int>>();

            // assert
            var output = function.fmap(new[] {21, 11, 9}).ToList();

            Assert.That(output.Count(), Is.EqualTo(3));
            Assert.That(output[0], Is.EqualTo(210));
            Assert.That(output[1], Is.EqualTo(1100));
            Assert.That(output[2], Is.EqualTo(9000));
        }
    }
}
