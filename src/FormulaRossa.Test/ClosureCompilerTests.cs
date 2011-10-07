using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FormulaRossa.Test
{
    [TestFixture]
    public class ClosureCompilerTests
    {
        private class Oper : IOperation
        {
            private readonly Func<Injector, object, object> _func;
            public Oper(Func<Injector, object, object> func )
            {
                _func = func;
            }
            public Func<Injector, object, object> GetClosure()
            {
                return _func;
            }
        }

        [Test]
        public void execute_ordered_test()
        {
            var item = new ClosureCompiler();

            var oper = new Oper((a, o) => (int) o*2);

            var f = item.Compile(new Queue<IOperation>(new[] {oper}));

            Assert.That(f(null, 5), Is.EqualTo(10));
        }

        [Test]
        public void execute_ordered_test2()
        {
            var item = new ClosureCompiler();

            var f = item.Compile(new Queue<IOperation>(new[] { new Oper((a, o) => (int)o * 4), new Oper((a, o) => (int)o - 2) }));

            Assert.That(f(null, 5), Is.EqualTo(18));
        }

        [Test]
        public void execute_ordered_test3()
        {
            var item = new ClosureCompiler();

            var f = item.Compile(new Queue<IOperation>(new[] { new Oper((a, o) => (int)o * 4), new Oper((a, o) => (int)o - 2), new Oper((a, o) => (int)o /3 ) }));

            Assert.That(f(null, 5), Is.EqualTo(6));
        }

        [Test]
        public void execute_reverse_ordered_test3()
        {
            var item = new ClosureCompiler();

            var f = item.Compile(new Queue<IOperation>(new[] { new Oper((a, o) => (int)o /3), new Oper((a, o) => (int)o - 2), new Oper((a, o) => (int)o * 4) }));

            Assert.That(f(null, 12), Is.EqualTo(8));
        }

        [Test]
        public void execute_reverse_ordered_test4()
        {
            var item = new ClosureCompiler();

            var f = item.Compile(new[] { new Oper((a, o) => (int)o / 3), new Oper((a, o) => (int)o - 2), new Oper((a, o) => (int)o * 4) });

            Assert.That(f(null, 12), Is.EqualTo(8));
        }

    }
}
