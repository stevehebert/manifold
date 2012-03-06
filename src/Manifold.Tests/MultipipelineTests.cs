﻿using Manifold.Tests.SupportedContainers;
using NUnit.Framework;

namespace Manifold.Tests
{
    [TestFixture]
    public class MultipipelineTests
    {
        [TestCase(SupportedProviderType.Autofac)]
        //[TestCase(SupportedProviderType.Ninject, Ignore=true)]
        public void verify_multipipelines(SupportedProviderType supportedProviderType)
        {
            var module = CommonModuleProvider.Create(supportedProviderType, item =>
                                                      {
                                                          item.RegisterPipeline<In, Out>()
                                                              .Bind<InOutTranslator>();

                                                          item.RegisterPipeline<int, string>()
                                                              .Bind(p => (p*2).ToString());

                                                          item.RegisterPipeline<string, int, string>("foo")
                                                              .Bind(p => (p*3).ToString());

                                                      });

           
            var value1 = module.Resolve<Pipe<In, Out>>()(new In()).NewValue;
            var value2 = module.Resolve<Pipe<int, string>>()(10);
            var value3 = module.Resolve<Pipe<string, int, string>>()("foo", 10);

            Assert.That(value1, Is.EqualTo(20));
            Assert.That(value2, Is.EqualTo("20"));
            Assert.That(value3, Is.EqualTo("30"));
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
