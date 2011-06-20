using Autofac;
using NUnit.Framework;
using Wormhole.Autofac;
using Wormhole.Pipeline;

namespace Wormhole.Tests
{
    [TestFixture]
    public class MultipipelineTests
    {
        [Test]
        public void verify_multipipelines()
        {
            var item = new PipelineModule();

            item.RegisterPipeline<In, Out>()
                .Bind<InOutTranslator>();

            item.RegisterPipeline<int, string>()
                .Bind(p => (p*2).ToString());

            var builder = new ContainerBuilder();
            builder.RegisterModule(item);

            var ctx = builder.Build();

            var value1 = ctx.Resolve<Functor<In, Out>>()(new In());
            var value2 = ctx.Resolve<Functor<int, string>>()(10);

            Assert.That(value1.NewValue, Is.EqualTo(20));
            Assert.That(value2, Is.EqualTo("20"));
        }

        public class InOutTranslator : IPipelineTask<In, Out>
        {
            public Out Execute(In input)
            {
                return new Out {NewValue = input.Value*2};
            }
        }

        public class In
        {
            public int Value
            {
                get { return 10;
            }
        }
        }

        public class Out
        {
            public int NewValue { get; set; }
        }
    }
}
