using System;
using System.Collections.Generic;
using System.Linq;
using Wormhole.DependencyInjection;

namespace Wormhole.Pipeline.Configuration
{
    public class PipelineDataResolver
    {
        private IDictionary<PipelineKey, Func<IResolveTypes, object, object>> _aggregatePipelineSets =
            new Dictionary<PipelineKey, Func<IResolveTypes, object, object>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDataResolver"/> class.
        /// </summary>
        /// <param name="pipelineSets">The pipeline sets.</param>
        public PipelineDataResolver(IEnumerable<IDictionary<PipelineKey, Func<IResolveTypes, object, object>>> pipelineSets)
        {
            // this is a one time operation as this class is a singleton in the container, making it immutable as well.
            foreach (var pair in pipelineSets.SelectMany(item => item))
                _aggregatePipelineSets.Add(pair.Key, pair.Value);
        }

        /// <summary>
        /// Finds the specified pipeline.
        /// </summary>
        /// <param name="key">The pipeline key.</param>
        /// <returns></returns>
        public Func<IResolveTypes, object, object> Find(PipelineKey key)
        {
            return _aggregatePipelineSets[key];
        }   
    }
}
