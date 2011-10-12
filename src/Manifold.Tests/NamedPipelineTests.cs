using NUnit.Framework;

namespace Manifold.Tests
{
    [TestFixture]
    public class NamedPipelineTests
    {
        //[Test]
        //public void verify_fallback_to_default_named_pipelines()
        //{
        //    var module = new SimplePipelineModule(item =>
        //                                              {
        //                                                  item.RegisterPipeline<int, string>()
        //                                                      .Alternate<string>()
        //                                                      .Bind(Convert.ToDouble)
        //                                                      .ContinueWith<string>();

        //                                                  item.RegisterPipeline<double, string>()
        //                                                      .Bind(p => p.ToString());

        //                                                  item.RegisterPipeline<string, double, string>("foo")
        //                                                      .Bind(p => (p*2).ToString());
        //                                              });
              

        //    var builder = new ContainerBuilder();
        //    builder.RegisterModule(module);

        //    var ctx = builder.Build();

        //    var value2 = ctx.Resolve<Functor<int, string>>()(10);
        //    var value3 = ctx.Resolve<Functor<string, int, string>>()("foo", 10);

        //    Assert.That(value2, Is.EqualTo("10"));
        //    Assert.That(value3, Is.EqualTo("20"));
        //}

        //[Test]
        //public void verify_failed_call_on_default_when_not_defined()
        //{
        //    var module = new SimplePipelineModule(item =>
        //    {
        //        item.RegisterPipeline<int, string>()
        //            .Alternate<string>()
        //            .Bind(Convert.ToDouble)
        //            .ContinueWith<string>();

        //        item.RegisterPipeline<string, double, string>("foo")
        //            .Bind(p => (p * 2).ToString());
        //    });


        //    var builder = new ContainerBuilder();
        //    builder.RegisterModule(module);

        //    var ctx = builder.Build();

        //    Assert.Throws<PipelineNotLocatedException>(() => ctx.Resolve<Functor<int, string>>()(10));
        //    var value3 = ctx.Resolve<Functor<string, int, string>>()("foo", 10);

        //    Assert.That(value3, Is.EqualTo("20"));
        //}

    }
}
