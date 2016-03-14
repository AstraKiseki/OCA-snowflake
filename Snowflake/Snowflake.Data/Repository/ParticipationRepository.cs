﻿using Snowflake.Core.Domain;
using Snowflake.Core.Repository;
using Snowflake.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Data.Repository
{
    public class ParticipationRepository : Repository<Participation>, IParticipationRepository
    {
        public ParticipationRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
