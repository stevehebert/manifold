﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using NUnit.Framework;
using Wormhole.Autofac;

namespace Wormhole.Tests
{
    [TestFixture]
    public class PipelineInjectionTests
    {
        [Test]
        public void verify_ordered()
        {
            var module = new PipelineModule();

            module.RegisterPipeline<IEnumerable<int>, IEnumerable<int>>()
                .Bind<Adder>()
                .Bind<Divider>();

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            var container = builder.Build();

            var function = container.Resolve<Func<IEnumerable<int>, IEnumerable<int>>>();

            var items = new[] { 10, 20, 30 };

            var resolvedItems = function(items);

            Assert.That(resolvedItems.Count(), Is.EqualTo(3));
            Assert.That(resolvedItems.ToArray()[0], Is.EqualTo(6));
            Assert.That(resolvedItems.ToArray()[1], Is.EqualTo(11));
            Assert.That(resolvedItems.ToArray()[2], Is.EqualTo(16));
        }

        public class Divider : IPipelineTask<IEnumerable<int>, IEnumerable<int>>
        {
            public IEnumerable<int> Execute(IEnumerable<int> input)
            {
                return from p in input select p / 2;
            }
        }

        public class Adder : IPipelineTask<IEnumerable<int>, IEnumerable<int>>
        {
            public IEnumerable<int> Execute(IEnumerable<int> input)
            {
                return from p in input select p + 2;
            }
        }
    }
}
