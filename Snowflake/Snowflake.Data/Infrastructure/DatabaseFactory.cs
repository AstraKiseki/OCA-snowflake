using Snowflake.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private readonly SnowflakeDataContext _dataContext;

        public SnowflakeDataContext GetDataContext()
        {
            return _dataContext ?? new SnowflakeDataContext();
        }

        public DatabaseFactory()
        {
            _dataContext = new SnowflakeDataContext();
        }

        protected override void DisposeCore()
        {
            if (_dataContext != null) _dataContext.Dispose();
        }
    }
}