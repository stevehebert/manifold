using System.Collections.Generic;

namespace Manifold.DependencyInjection
{
    public class PipelineContext : IPipelineContext
    {
        private readonly ITypeResolver _typeResolver;
        private readonly IDictionary<int, object> _dictionary = new Dictionary<int, object>();

        public PipelineContext(ITypeResolver typeResolver)
        {
            _typeResolver = typeResolver;
            
        }


        public ITypeResolver TypeResolver
        {
            get { return _typeResolver; }
        }

        public object this[int id]
        {
            get
            {
                return _dictionary.ContainsKey(id) ? _dictionary[id] : null;
            }
            set
            {
                if (_dictionary.ContainsKey(id)) 
                    _dictionary[id] = value;
                else
                    _dictionary.Add(id, value);
            }
        }
    }
}