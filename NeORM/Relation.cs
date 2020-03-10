using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NeORM
{
    internal class Relation<TNode, TNode2>
    {
        protected Node<TNode> Node { get; }
        protected Node<TNode2> Node2 { get; }
        internal int VariableCounter { get; }
        internal string RelationName { get; }
        internal Relation(TNode node, TNode2 node2, string relationName)
        {
            Node = new Node<TNode>(node);
            Node2 = new Node<TNode2>(node2);
            RelationName = relationName;
        }

        protected Relation(TNode node, TNode2 node2)
        {
            Node = new Node<TNode>(node);
            Node2 = new Node<TNode2>(node2);
        }

        internal virtual IDictionary<string,object> GetParameters()
        {
            var parametersNode1 = Node.GetParameters(0);
            var parametersNode2 = Node2.GetParameters(1);
            return parametersNode1.MergeParameterDictionaries(parametersNode2);
        }

        internal virtual string CreateRelationCypherQuery()
        {
            var findQueryNode1 = Node.FindNodeCypherQuery(0);
            var findQueryNode2 = Node2.FindNodeCypherQuery(1);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(findQueryNode1.Query);
            stringBuilder.Append(" , ");
            stringBuilder.AppendLine(findQueryNode2.Query);
            stringBuilder.AppendLine($"({findQueryNode1.MatchVariable})-[:{RelationName}]->({findQueryNode2.MatchVariable})");
            return stringBuilder.ToString();
        }
    }

    internal class Relation<TNode, TNode2, TRelation> : Relation<TNode, TNode2>
    {
        List<PropertyInfo> Properties { get; }
        TRelation RelationObject { get; }
        internal Relation(TNode node, TNode2 node2, TRelation relation) : base(node, node2)
        {
            RelationObject = relation;
            Properties = typeof(TRelation).GetProperties().Where(p => p.PropertyType.IsValueType || p.PropertyType == typeof(string)).ToList();
        }
        internal override IDictionary<string, object> GetParameters()
        {
            var parametersNode1 = Node.GetParameters(0);
            var parametersNode2 = Node2.GetParameters(1);
            var merge1 = parametersNode1.MergeParameterDictionaries(parametersNode2);
            return merge1.MergeParameterDictionaries(GetRelationParameters(2));
        }

        private Dictionary<string,object> GetRelationParameters(int afterFix)
        {
            var parameters = new Dictionary<string, object>();
            foreach (var property in Properties)
            {
                parameters.Add(property.Name + afterFix, property.GetValue(RelationObject));
            }
            return parameters;
        }

        internal override string CreateRelationCypherQuery()
        {
            var findQueryNode1 = Node.FindNodeCypherQuery(0);
            var findQueryNode2 = Node2.FindNodeCypherQuery(1);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(findQueryNode1.Query);
            stringBuilder.Append(" , ");
            stringBuilder.AppendLine(findQueryNode2.Query);
            stringBuilder.Append($"({findQueryNode1.MatchVariable})-");
            stringBuilder.Append($"[:{typeof(TRelation).Name}");
            stringBuilder.Append(" {");
            for (int i = 0; i < Properties.Count; i++)
            {
                stringBuilder.Append($" {Properties[i].Name} : ${Properties[i].Name}2");
                if (i != Properties.Count - 1)
                {
                    stringBuilder.Append(" , ");
                }
            }
            stringBuilder.Append("}]");
            stringBuilder.Append($"->({ findQueryNode2.MatchVariable})");
            return stringBuilder.ToString();
        }

    }
}
