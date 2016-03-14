using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        SnowflakeDataContext GetDataContext();
    }
}
