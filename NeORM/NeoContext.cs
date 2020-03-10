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
            try
            {
                await session.RunAsync("",);
            }
            finally
            {
                await session.CloseAsync();
            }
        }



    }
}
