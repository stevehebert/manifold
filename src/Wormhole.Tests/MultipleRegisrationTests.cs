using Autofac;
using NUnit.Framework;

namespace Wormhole.Tests
{
    [TestFixture]
    public class MultipleRegisrationTests
    {
        [Test] 
        public void verify_usage_across_multiple_registration_modules()
        {
            var module1 = new SimplePipelineModule(item =>
            {
                item.RegisterPipeline<MultipipelineTests.In, MultipipelineTests.Out>()
                    .Bind<MultipipelineTests.InOutTranslator>();

                item.RegisterPipeline<string, int, string>("foo")
                    .Bind(p => (p * 3).ToString());

            });

            var module2 = new SimplePipelineModule(item => item.RegisterPipeline<int, string>()
                                                               .Bind(p => (p * 2).ToString()));


            var builder = new ContainerBuilder();
            builder.RegisterModule(module1);
            builder.RegisterModule(module2);

            var ctx = builder.Build();

            var value1 = ctx.Resolve<Functor<MultipipelineTests.In, MultipipelineTests.Out>>()(new MultipipelineTests.In()).NewValue;
            var value2 = ctx.Resolve<Functor<int, string>>()(10);
            var value3 = ctx.Resolve<Functor<string, int, string>>()("foo", 10);

            Assert.That(value1, Is.EqualTo(20));
            Assert.That(value2, Is.EqualTo("20"));
            Assert.That(value3, Is.EqualTo("30"));

        }
    }
}
