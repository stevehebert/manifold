using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Manifold.Tests.SupportedContainers;
using NUnit.Framework;

namespace Manifold.Tests.Projector
{
    [TestFixture]
    public class ProjectorTests
    {
        public class Projector : IProjectorTask<int, int>
        {
            public IEnumerable<int> Execute(int input)
            {
                yield return input*3;
                yield return input*5;
            }
        }

        public class SlowProjector : IProjectorTask<int, int>
        {
            public IEnumerable<int> Execute(int input)
            {
                yield return input * 3;
                Thread.Sleep(10000);
                yield return input * 5;
            }
        }


        
        public class InjectedProjector
        {
            private readonly IProjectorTask<int, int> _dependency;

            public InjectedProjector(IProjectorTask<int, int> dependency )
            {
                _dependency = dependency;
            }

            public IEnumerable<int> Do(int b)
            {
                if(_dependency == null)
                    throw new NullReferenceException();

                return new []{b};
            }
        }



        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void delegate_based_projector(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType, item => item.RegisterProjector<int, int>()
                                                              .Bind(i => new[] {i*2, i*4})
                                                              .Bind(i => new[] {i*3, i*5}));

            var function = module.Resolve<Pipe<int, IEnumerable<int>>>();
            
            // act
            var resolvedItems = function(5);

            // assert
            Assert.NotNull(resolvedItems);
            Assert.That(resolvedItems.Count(), Is.EqualTo(4));

            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(10));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(20));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(15));
            Assert.That(resolvedItems.ToArray()[3], Is.EqualTo(25));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void single_injected_projector_composition(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType, item => item.RegisterProjector<int, int>()
                                                              .Bind<Projector>());

            var function = module.Resolve<Pipe<int, IEnumerable<int>>>();

            // act
            var resolvedItems = function(5);

            // assert
            Assert.NotNull(resolvedItems);
            Assert.That(resolvedItems.Count(), Is.EqualTo(2));

            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(15));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(25));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void mixed_projector_composition(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType, item => item.RegisterProjector<int, int>()
                                                                                  .Bind<Projector>()
                                                                                  .Bind(
                                                                                      input =>
                                                                                      new[] {input*100, input*1000}));

            var function = module.Resolve<Pipe<int, IEnumerable<int>>>();

            // act
            var resolvedItems = function(5);

            // assert
            Assert.NotNull(resolvedItems);
            Assert.That(resolvedItems.Count(), Is.EqualTo(4));

            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(15));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(25));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(500));
            Assert.That(resolvedItems.ToArray()[3], Is.EqualTo(5000));
        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void validate_short_circuit_capability(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType, item => item.RegisterProjector<int, int>()
                                                                                  .Bind<SlowProjector>());

            var function = module.Resolve<Pipe<int, IEnumerable<int>>>();

            var resolvedItems = function(5);

            // act
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Assert.IsTrue(resolvedItems.Any());
            stopWatch.Stop();

            // assert
            Assert.That(stopWatch.ElapsedMilliseconds, Is.LessThan(5000));

        }

        [TestCase(SupportedProviderType.Autofac)]
        [TestCase(SupportedProviderType.Ninject)]
        public void bind_should_resolve_and_call_funcs(SupportedProviderType supportedProviderType)
        {
            // arrange
            var module = CommonModuleProvider.Create(supportedProviderType, item => item.RegisterProjector<int, int>()
                                                                                  .Bind<InjectedProjector>((a,b) => a.Do(b)));
            
            module.Register<Projector, IProjectorTask<int, int>>();
            var function = module.Resolve<Pipe<int, IEnumerable<int>>>();

            var resolvedItems = function(5);

            // act
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Assert.IsTrue(resolvedItems.Any());
            stopWatch.Stop();

            // assert
            Assert.That(stopWatch.ElapsedMilliseconds, Is.LessThan(5000));

        }
    }
}
