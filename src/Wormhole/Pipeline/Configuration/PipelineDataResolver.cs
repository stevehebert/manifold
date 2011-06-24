using System;
using System.Collections.Generic;
using System.Linq;
using Wormhole.DependencyInjection;

namespace Wormhole.Pipeline.Configuration
{
    public class PipelineDataResolver
    {
        private IDictionary<PipelineKey, Func<Tuple<IResolveTypes,object>, object, object, object>> _aggregatePipelineSets =
            new Dictionary<PipelineKey, Func<Tuple<IResolveTypes,object>, object, object, object>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDataResolver"/> class.
        /// </summary>
        /// <param name="pipelineSets">The pipeline sets.</param>
        public PipelineDataResolver(IEnumerable<IDictionary<PipelineKey, Func<Tuple<IResolveTypes,object>, object, object, object>>> pipelineSets)
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
        public Func<Tuple<IResolveTypes, object>, object, object, object> Find(PipelineKey key)
        {
            try
            {
                return _aggregatePipelineSets[key];
            }
            catch( KeyNotFoundException exception)
            {
                throw new PipelineNotLocatedException(
                    string.Format("unable to find matching pipeline key for {0}, {1}, {2}", key.Input.Name,
                                  key.Output.Name, key.Named), exception);
            }
        }   

        public bool ContainsKey(PipelineKey key)
        {
            return _aggregatePipelineSets.ContainsKey(key);
        }

    }
}
