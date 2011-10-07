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



    }
}
