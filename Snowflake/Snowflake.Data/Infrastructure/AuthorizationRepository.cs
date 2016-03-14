using Microsoft.AspNet.Identity;
using Snowflake.Core.Domain;
using Snowflake.Core.Infrastructure;
using Snowflake.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Data.Infrastructure
{
    public class AuthorizationRepository : IAuthorizationRepository, IDisposable
    {
        private readonly IUserStore<SnowflakeUser, int> _userStore;
        private readonly IDatabaseFactory _databaseFactory;
        private readonly UserManager<SnowflakeUser, int> _userManager;

        private SnowflakeDataContext db;
        protected SnowflakeDataContext Db => db ?? (db = _databaseFactory.GetDataContext());

        public AuthorizationRepository(IDatabaseFactory databaseFactory, IUserStore<SnowflakeUser, int> userStore)
        {
            _userStore = userStore;
            _databaseFactory = databaseFactory;
            _userManager = new UserManager<SnowflakeUser, int>(userStore);
        }

        public async Task<IdentityResult> RegisterUser(RegistrationModel model)
        {
            var snowflakeUser = new SnowflakeUser
            {
                UserName = model.Username,
                Email = model.EmailAddress,
            };

            var result = await _userManager.CreateAsync(snowflakeUser, model.Password);

            return result;
        }

        public async Task<SnowflakeUser> FindUser(string username, string password)
        {
            return await _userManager.FindAsync(username, password);
        }

        public void Dispose()
        {
            _userManager.Dispose();
        }
    }
}
