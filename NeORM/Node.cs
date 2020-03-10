using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NeORM
{
    internal class Node<T>
    {
        internal T NodeObject { get; }
        List<PropertyInfo> Properties { get; }
        internal Node(T nodeObject)
        {
            NodeObject = nodeObject;
            Properties = typeof(T).GetProperties().Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string)).ToList();
        }
        private Dictionary<string,object> PrepareParameters()
        {
            var parameterCache = new Dictionary<string, object>();
            foreach (var property in Properties)
            {
                parameterCache.Add(property.Name, property.GetValue(NodeObject));
            }
            return parameterCache;
        }

        private Dictionary<string, object> PrepareParameters(int variableAfterFix)
        {
            var parameterCache = new Dictionary<string, object>();
            foreach (var property in Properties)
            {
                parameterCache.Add(property.Name + variableAfterFix, property.GetValue(NodeObject));
            }
            return parameterCache;
        }

        internal virtual IDictionary<string,object> GetParameters(int variableAfterFix)
        {
            return PrepareParameters(variableAfterFix);
        }
        internal virtual IDictionary<string, object> GetParameters()
        {
            return PrepareParameters();
        }

        internal virtual MatchQuery FindNodeCypherQuery(int variableAfterFix)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"MATCH (n{variableAfterFix}:{typeof(T).Name}");
            stringBuilder.Append("{");
            for (int i = 0; i < Properties.Count; i++)
            {
                stringBuilder.Append($" {Properties[i].Name} : ${Properties[i].Name}{variableAfterFix} ");
                if (i != Properties.Count - 1)
                {
                    stringBuilder.Append(" , ");
                }
            }
            stringBuilder.Append("})");
            return new MatchQuery() { MatchVariable = "n" + variableAfterFix, Query = stringBuilder.ToString() };
        }

        internal virtual string CreateNodeCypherQuery()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"CREATE (n:{typeof(T).Name}");
            stringBuilder.Append("{");
            for(int i = 0; i< Properties.Count;i++)
            {
                stringBuilder.Append($" {Properties[i].Name} : ${Properties[i].Name} ");
                if(i != Properties.Count - 1)
                {
                    stringBuilder.Append(" , ");
                }
            }
            stringBuilder.Append("})");
            return stringBuilder.ToString();
        }
    }
}
