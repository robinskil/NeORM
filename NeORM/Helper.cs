using System;
using System.Collections.Generic;
using System.Text;

namespace NeORM
{
    public static class Helper
    {
        public static IDictionary<string,object> MergeParameterDictionaries(this IDictionary<string,object> params1 , IDictionary<string,object> params2)
        {
            var allParams = new Dictionary<string, object>();
            foreach (var param1 in params1)
            {
                allParams.Add(param1.Key, param1.Value);
            }
            foreach (var param2 in params2)
            {
                allParams.Add(param2.Key, param2.Value);
            }
            return allParams;
        }
    }
}
