using Snowflake.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Snowflake.Core.Repository;

namespace Snowflake.Infrastructure
{
    public class BaseApiController : ApiController
    {
        protected readonly ISnowflakeUserRepository _snowflakeUserRepository;

        public BaseApiController(ISnowflakeUserRepository snowflakeUserRepository)
        {
            _snowflakeUserRepository = snowflakeUserRepository;
        }

        protected SnowflakeUser CurrentUser
        {
            get
            {
                return _snowflakeUserRepository.GetFirstOrDefault(u => u.UserName == User.Identity.Name);
            }
        }
    }
}
