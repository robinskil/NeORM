using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NeORM
{
    public interface INeqQueryable
    {

    }
    public class NeoQueryable
    {

    }
    public class NeoQueryable<T>
    {
        
    }
    public class Node
    {

    }

    public class Node<T> : Node where T : class
    {
        public T QuerySingle()
        {

        }
        public NeoQueryable<T> InsertNode(T node)
        {

        }
    }

    public class Relation
    {
        public string RelationType { get; set; }
        public Type RelationData { get; set; }
        public Node Parent { get; set; }
        public Node Child { get; set; }
        public Relation(Node parent, Node child,Type relationData , string relationType)
        {
            Parent = parent;
            Child = child;
            RelationData = relationData;
            RelationType = relationType;
        }
    }

    public class Relation<T> : Relation 
    {
        public Relation(Node parent, Node child, string relationType) : base(parent,child,typeof(T),relationType)
        {

        }
    }

    public class GraphContext
    {
        public GraphContext()
        {
            EdgeBuilder(new EdgeBuilder());
        }
        protected virtual void EdgeBuilder(EdgeBuilder edgeBuilder)
        {

        }
    }

    public class EdgeBuilder
    {
        public void ConnectNodes<TParent,TChild,TRelation>(Expression<Func<TParent, TChild>> mapping, Node<TParent> parent , Node<TChild> child, Relation<TRelation> relation) where TParent : class where TChild : class
        {

        }
    }

    public class ExampleContext : GraphContext
    {
        public Node<Person> Person { get; set; }
        public Node<Work> Work { get; set; }
        public Relation<WorksAt> WorksAt { get; set; }

        protected override void EdgeBuilder(EdgeBuilder edgeBuilder)
        {
            edgeBuilder.ConnectNodes();
        }
    }

    public class WorksAt
    {
    }
}
