using System;
using NUnit.Framework;

namespace FormulaRossa.Test
{
    [TestFixture]
    public class RouterCompilerTests
    {
        private class Oper : IOperation
        {
            private readonly Func<Injector, object, object> _func;

            public Oper(Func<Injector, object, object> func)
            {
                _func = func;
            }
            public Func<Injector, object, object> GetClosure()
            {
                return _func;
            }
        }

        private class RoutedOper : Oper, IRoutedOperation
        {
            private readonly Func<Injector, object, bool> _decider;
            public RoutedOper(Func<Injector, object, object> closure, Func<Injector, object, bool> decider) : base (closure)
            {
                _decider = decider;
            }


            public Func<Injector, object, bool> GetDecider()
            {
                return _decider;
            }
        }

        private int GetRoutedValue(Func<Injector, object, object> operation, int value )
        {
            return (int) operation(null, value);
        }

        [Test]
        public void routed_test_1()
        {
            var compiler = new RouterCompiler();

            var fn = compiler.Compile(new[]
                                          {
                                              new RoutedOper((injector, input) => (int) input*100,
                                                             (injector, input) => (int) input == 4),
                                              new Oper((injector, input) => (int) input*10)
                                          });

            Assert.That(GetRoutedValue(fn, 4), Is.EqualTo(400));
            Assert.That(GetRoutedValue(fn, 2), Is.EqualTo(20));
        }

        [Test]
        public void multi_route_test()
        {
            var compiler = new RouterCompiler();

            var fn = compiler.Compile(new[]
                                          {
                                              new RoutedOper((injector, input) => (int) input*100,
                                                             (injector, input) => (int) input == 4),
                                              new RoutedOper((injector, input) => (int) input*1000,
                                                             (injector, input) => (int) input == 3),
                                              new Oper((injector, input) => (int) input*10)
                                          });

            Assert.That(GetRoutedValue(fn, 4), Is.EqualTo(400));
            Assert.That(GetRoutedValue(fn, 2), Is.EqualTo(20));
            Assert.That(GetRoutedValue(fn, 3), Is.EqualTo(3000));
        }

        [Test]
        public void short_circuit_on_default_test()
        {
            var compiler = new RouterCompiler();

            var fn = compiler.Compile(new[]
                                          {
                                              new Oper((injector, input) => (int) input*10),
                                              new RoutedOper((injector, input) => (int) input*100,
                                                             (injector, input) => (int) input == 4),
                                              new RoutedOper((injector, input) => (int) input*1000,
                                                             (injector, input) => (int) input == 3)
                                          });

            Assert.That(GetRoutedValue(fn, 4), Is.EqualTo(40));
            Assert.That(GetRoutedValue(fn, 2), Is.EqualTo(20));
            Assert.That(GetRoutedValue(fn, 3), Is.EqualTo(30));
        }
    }
}
