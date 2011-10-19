using System;
using Manifold.Configuration;
using Manifold.Configuration.Pipeline;
using Manifold.Configuration.Pipeline.Operations;
using Manifold.DependencyInjection;
using Manifold.Exceptions;
using Manifold.Router;
using Moq;
using NUnit.Framework;

namespace Manifold.Tests.Router
{
    [TestFixture]
    public class RouterCompilerTests
    {
        private class Oper : IOperation
        {
            private readonly Func<IResolveTypes, object, object> _func;

            public Oper(Func<IResolveTypes, object, object> func)
            {
                _func = func;
            }
            public Func<IResolveTypes, object, object> GetExecutor()
            {
                return _func;
            }
        }

        private class RoutedOper : Oper, IRoutedOperation
        {
            private readonly Func<IResolveTypes, object, bool> _decider;
            public RoutedOper(Func<IResolveTypes, object, object> closure, Func<IResolveTypes, object, bool> decider)
                : base(closure)
            {
                _decider = decider;
            }


            public Func<IResolveTypes, object, bool> GetDecider()
            {
                return _decider;
            }
        }

        private int GetRoutedValue(Func<IResolveTypes, object, object> operation, int value)
        {
            return (int)operation(null, value);
        }

        [Test]
        public void routed_test_1()
        {
            // arrange
            var mockDefinition = new Mock<IPipeDefinition>();
            mockDefinition.SetupGet(e => e.Operations).Returns(new[]
                                                                   {
                                                                       new RoutedOper(
                                                                           (injector, input) => (int) input*100,
                                                                           (injector, input) => (int) input == 4),
                                                                       new Oper((injector, input) => (int) input*10)
                                                                   });
            mockDefinition.SetupGet(e => e.Closed).Returns(true);

            var compiler = new RouterCompiler(mockDefinition.Object);

            // act
            var fn = compiler.Compile();

            // assert
            Assert.That(GetRoutedValue(fn, 4), Is.EqualTo(400));
            Assert.That(GetRoutedValue(fn, 2), Is.EqualTo(20));
        }

        [Test]
        public void multi_route_test()
        {
            // arrange
            var mockDefinition = new Mock<IPipeDefinition>();
            mockDefinition.SetupGet(e => e.Operations).Returns(new[]
                                                                   {
                                                                       new RoutedOper(
                                                                           (injector, input) => (int) input*100,
                                                                           (injector, input) => (int) input == 4),
                                                                       new RoutedOper(
                                                                           (injector, input) => (int) input*1000,
                                                                           (injector, input) => (int) input == 3),
                                                                       new Oper((injector, input) => (int) input*10)
                                                                   });
            mockDefinition.SetupGet(e => e.Closed).Returns(true);
            
            var compiler = new RouterCompiler(mockDefinition.Object);

            // act
            var fn = compiler.Compile();

            // assert
            Assert.That(GetRoutedValue(fn, 4), Is.EqualTo(400));
            Assert.That(GetRoutedValue(fn, 2), Is.EqualTo(20));
            Assert.That(GetRoutedValue(fn, 3), Is.EqualTo(3000));
        }

        [Test]
        public void short_circuit_on_default_test()
        {
            // arrange
            var mockDefinition = new Mock<IPipeDefinition>();
            mockDefinition.SetupGet(e => e.Operations).Returns(new[]
                                                                   {
                                                                       new Oper((injector, input) => (int) input*10),
                                                                       new RoutedOper(
                                                                           (injector, input) => (int) input*100,
                                                                           (injector, input) => (int) input == 4),
                                                                       new RoutedOper(
                                                                           (injector, input) => (int) input*1000,
                                                                           (injector, input) => (int) input == 3)
                                                                   });
            mockDefinition.SetupGet(e => e.Closed).Returns(true);
            var compiler = new RouterCompiler(mockDefinition.Object);

            // act
            var fn = compiler.Compile();

            // assert
            Assert.That(GetRoutedValue(fn, 4), Is.EqualTo(40));
            Assert.That(GetRoutedValue(fn, 2), Is.EqualTo(20));
            Assert.That(GetRoutedValue(fn, 3), Is.EqualTo(30));
        }

        [Test]
        public void verify_throws_on_unclosed_composition()
        {
            // arrange
            var mockDefinition = new Mock<IPipeDefinition>();
            mockDefinition.SetupGet(e => e.Closed).Returns(false);
            var compiler = new RouterCompiler(mockDefinition.Object);

            // act & assert
            Assert.Throws<MismatchedClosingTypeDeclarationException>( () =>  compiler.Compile());
        }
    }
}
