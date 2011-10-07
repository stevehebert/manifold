using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using System.Diagnostics;

namespace FormulaRossa.Test
{
    [TestFixture]
    public class ProjectorCompilerTests
    {
        private class Projector : IProjector<int, string>
        {
            readonly Func<int, IEnumerable<string>> _processor;

            public Projector(Func<int, IEnumerable<string>> processor )
            {
                _processor = processor;
            }

            public IEnumerable<string> Process(int input)
            {
                return _processor(input);
            }
        }

        private class TimedProjector : IProjector<int,string>
        {
            private int _tickCount;
            public TimedProjector(int tickCount)
            {
                _tickCount = tickCount;
            }
            public IEnumerable<string> Process(int input)
            {
                yield return string.Format("fast {0}", input);
                Thread.Sleep(_tickCount);
                yield return string.Format("slow {0}", input);
            }
        }

        [Test]
        public void verify_optimized_calling()
        {
            var compiler = new ProjectorCompiler();
            
            var fn = compiler.Compile(new[] {new TimedProjector(5000)});
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            var found = fn(null, 4).Any();
            stopWatch.Stop();

            Assert.That(stopWatch.ElapsedMilliseconds, Is.LessThan(4000));
        }

        [Test]
        public void verify_single_object_projection()
        {
            var compiler = new ProjectorCompiler();

            var fn = compiler.Compile(new[] { new TimedProjector(0) });
            var items = fn(null, 4);

            Assert.That(items.Count(), Is.EqualTo(2));
        }

        [Test]
        public void verify_multi_object_projection()
        {
            var compiler = new ProjectorCompiler();

            var fn = compiler.Compile(new[] { new TimedProjector(0), new TimedProjector(0) });
            var items = fn(null, 4);

            Assert.That(items.Count(), Is.EqualTo(4));
        }
    }
}
