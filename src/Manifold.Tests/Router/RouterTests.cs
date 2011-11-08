using System;
using System.Linq;
using Manifold.Tests.SupportedContainers;
using NUnit.Framework;

namespace Manifold.Tests.Router
{
    [TestFixture]
    public class RouterTests
    {
        public class ConditionalRoute : IRoutingPipelineTask<int, int>
        {
            public int Execute(int input)
            {
                return input*10;
            }

            public bool CanExecute(int input)
            {
                return input > 0;
            }
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void injected_route_test(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType,
                    e =>
                    e.RegisterPipeline<int, int>()
                        .CreateRouter()
                            .BindConditional<ConditionalRoute>()
                            .Default(x => x * 100));


            // act
            var function = module.Resolve<Pipe<int, int>>();

            // assert
            Assert.That(function(10), Is.EqualTo(100));
            Assert.That(function(-10), Is.EqualTo(-1000));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void simple_single_route_test_behavior(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType, 
                    e =>
                    e.RegisterPipeline<int, int>()
                        .CreateRouter()
                            .BindConditional(i => i > 0, x => x*10)
                            .Default(x => x*100));


            // act
            var function = module.Resolve<Pipe<int, int>>();

            // assert
            Assert.That(function(10), Is.EqualTo(100));
            Assert.That(function(-10), Is.EqualTo(-1000));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void multi_path_single_route_test_behavior(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType, 
                    e =>
                    e.RegisterPipeline<int, int>()
                        .CreateRouter()
                            .BindConditional(i => i > 20, x => x * 10)
                            .BindConditional(i => i > 10, x => x * 100)
                            .Default(x => x * 1000));

            // act
            var function = module.Resolve<Pipe<int, int>>();

            // assert
            Assert.That(function(21), Is.EqualTo(210));
            Assert.That(function(11), Is.EqualTo(1100));
            Assert.That(function(9), Is.EqualTo(9000));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void test_multi_path_single_route_projection_mapping(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType, 
                    e =>
                    e.RegisterPipeline<int, int>()
                        .CreateRouter()
                            .BindConditional(i => i > 20, x => x * 10)
                            .BindConditional(i => i > 10, x => x * 100)
                            .Default(x => x * 1000));

            // act
            var function = module.Resolve<Pipe<int, int>>();

            // assert
            var output = function.fmap(new[] {21, 11, 9}).ToList();

            Assert.That(output.Count(), Is.EqualTo(3));
            Assert.That(output[0], Is.EqualTo(210));
            Assert.That(output[1], Is.EqualTo(1100));
            Assert.That(output[2], Is.EqualTo(9000));
        }


        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject, Ignored = true)]
        public void test_named_default_route_action(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType,
                                                     e =>
                                                         {
                                                             e.RegisterPipeline<int, int>()
                                                                 .CreateRouter()
                                                                 .BindConditional(i => i > 20, x => x*10)
                                                                 .BindConditional(i => i > 10, x => x*100)
                                                                 .DefaultContinue(0);

                                                             e.RegisterPipeline<int, int, int>(0)
                                                                 .Bind(x => x*1000);

                                                             e.RegisterPipeline<int, int, int>(1)
                                                                 .Bind(x => x*100);

                                                         });

            // act
            var function = module.Resolve<Pipe<int, int>>();

            // assert
            var output = function.fmap(new[] { 21, 11, 9 }).ToList();

            Assert.That(output.Count(), Is.EqualTo(3));
            Assert.That(output[0], Is.EqualTo(210));
            Assert.That(output[1], Is.EqualTo(1100));
            Assert.That(output[2], Is.EqualTo(9000));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject, Ignored=true)]
        public void test_named_alternate_route_action(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType,
                                                     e =>
                                                     {
                                                         e.RegisterPipeline<int, int>()
                                                             .CreateRouter()
                                                             .BindConditional(i => i > 20, x => x * 10)
                                                             .BindConditional(i => i > 10, x => x * 100)
                                                             .DefaultContinue(1);

                                                         e.RegisterPipeline<int, int, int>(0)
                                                             .Bind(x => x * 1000);

                                                         e.RegisterPipeline<int, int, int>(1)
                                                             .Bind(x => x * 100);
                                                     });

            // act
            var function = module.Resolve<Pipe<int, int>>();

            // assert
            var output = function.fmap(new[] { 21, 11, 9 }).ToList();

            Assert.That(output.Count(), Is.EqualTo(3));
            Assert.That(output[0], Is.EqualTo(210));
            Assert.That(output[1], Is.EqualTo(1100));
            Assert.That(output[2], Is.EqualTo(900));
        }


        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject, Ignored = true)]
        public void test_coerced_continuation_route_action(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType,
                                                     e =>
                                                         {
                                                             e.RegisterPipeline<int, int>()
                                                                 .CreateRouter()
                                                                 .BindConditional(i => i > 20, x => x*10)
                                                                 .BindConditional(i => i > 10, x => x*100)
                                                                 .DefaultContinue<int, string>(1, x => x.ToString());

                                                         e.RegisterPipeline<int, string, int>(0)
                                                             .Bind(Convert.ToInt32)
                                                             .Bind(x => x * 1000);

                                                         e.RegisterPipeline<int, string, int>(1)
                                                             .Bind(Convert.ToInt32)
                                                             .Bind(x => x * 10000);
                                                     });

            // act
            var function = module.Resolve<Pipe<int, int>>();

            // assert
            var output = function.fmap(new[] { 21, 11, 9 }).ToList();

            Assert.That(output.Count(), Is.EqualTo(3));
            Assert.That(output[0], Is.EqualTo(210));
            Assert.That(output[1], Is.EqualTo(1100));
            Assert.That(output[2], Is.EqualTo(90000));
        }


    }
}
