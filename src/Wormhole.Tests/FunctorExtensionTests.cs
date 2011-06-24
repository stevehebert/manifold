using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Wormhole.Tests
{
    [TestFixture]
    public class FunctorExtensionTests
    {
        [Test]
        public void verify_unary_functor_behavior()
        {
            var func = new Functor<int, string>(a => a.ToString());

            var output = func.fmap(new[] {1, 2}).ToList();

            Assert.That(output.Count(), Is.EqualTo(2));
            Assert.That(output[0], Is.EqualTo("1"));
            Assert.That(output[1], Is.EqualTo("2"));

        }

        [Test]
        public void verify_named_unary_functor_behavior()
        {
            var func = new Functor<string, int, string>((n,a) => a.ToString());

            var output = func.fmap("foo", new[] { 1, 2 }).ToList();

            Assert.That(output.Count(), Is.EqualTo(2));
            Assert.That(output[0], Is.EqualTo("1"));
            Assert.That(output[1], Is.EqualTo("2"));

        }


        
    }
}
