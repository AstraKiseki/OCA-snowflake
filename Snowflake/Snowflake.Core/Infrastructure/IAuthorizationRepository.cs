using Microsoft.AspNet.Identity;
using Snowflake.Core.Domain;
using Snowflake.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Core.Infrastructure
{
    public interface IAuthorizationRepository : IDisposable
    {
        Task<SnowflakeUser> FindUser(string username, string password);
        Task<IdentityResult> RegisterUser(RegistrationModel model);
    }
}
