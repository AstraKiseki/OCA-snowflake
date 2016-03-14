using Snowflake.Core.Domain;
using Snowflake.Core.Repository;
using Snowflake.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Data.Repository
{
    public class ThoughtRepository : Repository<Thought>, IThoughtRepository
    {
        public ThoughtRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
