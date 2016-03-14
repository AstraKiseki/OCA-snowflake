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
    public class ChoiceRepository : Repository<Choice>, IChoiceRepository
    {
        public ChoiceRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
