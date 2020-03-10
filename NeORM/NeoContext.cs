using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace NeORM
{
    public class NeoContext
    {
        IDriver Driver { get; }
        public NeoContext(IDriver driver)
        {
            Driver = driver;
        }

        public async Task AddNode<T>(T node)
        {
            var session = Driver.AsyncSession();
            var nodeObject = new Node<T>(node);
            try
            {
                await session.RunAsync(nodeObject.CreateNodeCypherQuery(),nodeObject.GetParameters());
            }
            finally
            {
                await session.CloseAsync();
            }
        }
        public async Task AddRelation<TFromNode, TToNode>(TFromNode fromNode , TToNode toNode,string relationTypeName)
        {
            var session = Driver.AsyncSession();
            var relation = new Relation<TFromNode, TToNode>(fromNode,toNode,relationTypeName);
            try
            {
                await session.RunAsync(relation.CreateRelationCypherQuery(), relation.GetParameters());
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task AddRelation<TFromNode, TToNode,TRelation>(TFromNode fromNode,TToNode toNode,TRelation relation)
        {
            var session = Driver.AsyncSession();
            var relationDefinition = new Relation<TFromNode, TToNode,TRelation>(fromNode, toNode, relation);
            try
            {
                await session.RunAsync(relationDefinition.CreateRelationCypherQuery(), relationDefinition.GetParameters());
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}
