using System;
using System.Collections.Generic;
using System.Linq;
using Manifold.Configuration.Pipeline;
using Manifold.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Manifold.Tests.PipeAndFilter
{
    [TestFixture]
    public class PipelineDefinitionTests
    {
        public class TestClass : IPipelineTask<int, string>
        {
            public string Execute(int input)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void verify_adding_injected_operation_behavior()
        {
            // arrange
            var mockRegistrar = new Mock<IRegisterTypes>();

            var list = new List<Action<IRegisterTypes>>();
            var item = new PipeDefinition(list);

            // act
            item.AddInjectedOperation<TestClass, int, string>(true);

            // assert
            Assert.That(item.Operations.Count(), Is.EqualTo(1));
            Assert.That(list.Count(), Is.EqualTo(1));

            foreach (var value in list)
                value(mockRegistrar.Object);

            mockRegistrar.Verify(e=> e.RegisterType<TestClass>(false), Times.Once());
        }

        [Test]
        public void verify_adding_function_operation_behavior()
        {
            // arrange
            var mockRegistrar = new Mock<IRegisterTypes>();

            var list = new List<Action<IRegisterTypes>>();
            var item = new PipeDefinition(list);

            // act
            item.AddFunctionOperation<int, string>(i => i.ToString(), true);

            // assert
            Assert.That(item.Operations.Count(), Is.EqualTo(1));
            Assert.That(list.Count(), Is.EqualTo(0));
        }
    }
}
