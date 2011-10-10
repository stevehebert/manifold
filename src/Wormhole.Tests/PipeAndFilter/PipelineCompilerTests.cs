using System;
using System.Collections.Generic;
using NUnit.Framework;
using Wormhole.DependencyInjection;
using Wormhole.PipeAndFilter;

namespace Wormhole.Tests.PipeAndFilter
{
    [TestFixture]
    public class PipelineCompilerTests
    {
        private class Oper : IOperation
        {
            private readonly Func<IResolveTypes, object, object> _func;
            public Oper(Func<IResolveTypes, object, object> func)
            {
                _func = func;
            }
            public Func<IResolveTypes, object, object> GetClosure()
            {
                return _func;
            }
        }

        [Test]
        public void execute_ordered_test()
        {
            var item = new PipelineCompiler();

            var oper = new Oper((a, o) => (int)o * 2);

            var f = item.Compile(new Queue<IOperation>(new[] { oper }));

            Assert.That(f(null, 5), Is.EqualTo(10));
        }

        [Test]
        public void execute_ordered_test2()
        {
            var item = new PipelineCompiler();

            var f = item.Compile(new Queue<IOperation>(new[] { new Oper((a, o) => (int)o * 4), new Oper((a, o) => (int)o - 2) }));

            Assert.That(f(null, 5), Is.EqualTo(18));
        }

        [Test]
        public void execute_ordered_test3()
        {
            var item = new PipelineCompiler();

            var f = item.Compile(new Queue<IOperation>(new[] { new Oper((a, o) => (int)o * 4), new Oper((a, o) => (int)o - 2), new Oper((a, o) => (int)o / 3) }));

            Assert.That(f(null, 5), Is.EqualTo(6));
        }

        [Test]
        public void execute_reverse_ordered_test3()
        {
            var item = new PipelineCompiler();

            var f = item.Compile(new Queue<IOperation>(new[] { new Oper((a, o) => (int)o / 3), new Oper((a, o) => (int)o - 2), new Oper((a, o) => (int)o * 4) }));

            Assert.That(f(null, 12), Is.EqualTo(8));
        }

        [Test]
        public void execute_reverse_ordered_test4()
        {
            var item = new PipelineCompiler();

            var f = item.Compile(new[] { new Oper((a, o) => (int)o / 3), new Oper((a, o) => (int)o - 2), new Oper((a, o) => (int)o * 4) });

            Assert.That(f(null, 12), Is.EqualTo(8));
        }

    }
}
