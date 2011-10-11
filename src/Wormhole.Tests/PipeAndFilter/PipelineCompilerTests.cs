using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Wormhole.Configuration;
using Wormhole.Configuration.Operations;
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
            public Func<IResolveTypes, object, object> GetExecutor()
            {
                return _func;
            }
        }

        [Test]
        public void execute_ordered_test()
        {
            var mockPipelineDefinition = new Mock<IPipeDefinition>();
            var oper = new Oper((a, o) => (int)o * 2);
            mockPipelineDefinition.SetupGet(e => e.Closed).Returns(true);

            mockPipelineDefinition.SetupGet(e => e.Operations).Returns(new[] {oper});
            
            var item = new PipelineCompiler(mockPipelineDefinition.Object);


            var f = item.Compile();

            Assert.That(f(null, 5), Is.EqualTo(10));
        }

        [Test]
        public void execute_ordered_test2()
        {
            // arrange
            var mockPipelineDefinition = new Mock<IPipeDefinition>();
            mockPipelineDefinition.SetupGet(e => e.Operations).Returns(new[] {new Oper((a, o) => (int)o * 4), new Oper((a, o) => (int)o - 2) });
            var item = new PipelineCompiler(mockPipelineDefinition.Object);

            // act
            var f = item.Compile(new Queue<IOperation>(new[] { new Oper((a, o) => (int)o * 4), new Oper((a, o) => (int)o - 2) }));

            // assert
            Assert.That(f(null, 5), Is.EqualTo(18));
        }

        [Test]
        public void execute_ordered_test3()
        {

            // arrange
            var mockPipelineDefinition = new Mock<IPipeDefinition>();
            mockPipelineDefinition.SetupGet(e => e.Operations).Returns(new[] { new Oper((a, o) => (int)o * 4), new Oper((a, o) => (int)o - 2) });
            var item = new PipelineCompiler(mockPipelineDefinition.Object);


            var f = item.Compile(new Queue<IOperation>(new[] { new Oper((a, o) => (int)o * 4), new Oper((a, o) => (int)o - 2), new Oper((a, o) => (int)o / 3) }));

            Assert.That(f(null, 5), Is.EqualTo(6));
        }

        [Test]
        public void execute_reverse_ordered_test3()
        {
            // arrange
            var mockPipelineDefinition = new Mock<IPipeDefinition>();
            mockPipelineDefinition.SetupGet(e => e.Closed).Returns(true);
            mockPipelineDefinition.SetupGet(e => e.Operations).Returns(new[] { new Oper((a, o) => (int)o / 3), new Oper((a, o) => (int)o - 2), new Oper((a, o) => (int)o * 4) });
            var item = new PipelineCompiler(mockPipelineDefinition.Object);

            // act
            var f = item.Compile();

            // assert
            Assert.That(f(null, 12), Is.EqualTo(8));
        }

        [Test]
        public void execute_reverse_ordered_test4()
        {
            // arrange
            var mockPipelineDefinition = new Mock<IPipeDefinition>();
            mockPipelineDefinition.SetupGet(e => e.Operations).Returns(new[] { new Oper((a, o) => (int)o / 3), new Oper((a, o) => (int)o - 2), new Oper((a, o) => (int)o * 4) });
            mockPipelineDefinition.SetupGet(e => e.Closed).Returns(true);
            var item = new PipelineCompiler(mockPipelineDefinition.Object);

            // act
            var f = item.Compile();

            // assert
            Assert.That(f(null, 12), Is.EqualTo(8));
        }

    }
}
