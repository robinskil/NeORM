using System;
using System.Collections.Generic;
using System.Text;

namespace NeORM
{
    public class NeoQuery
    {
        public IDictionary<string,object> Parameters { get; }
        public NeoQuery(Type type, object value)
        {
            Parameters = new Dictionary<string, object>();
        }
    }
}
