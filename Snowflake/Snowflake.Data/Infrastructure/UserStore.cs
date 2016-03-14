using Microsoft.AspNet.Identity;
using Snowflake.Core.Domain;
using Snowflake.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Data.Infrastructure
{
    public class UserStore : Disposable,
                             IUserPasswordStore<SnowflakeUser, int>,
                             IUserSecurityStampStore<SnowflakeUser, int>,
                             IUserRoleStore<SnowflakeUser, int>
    {
        private readonly IDatabaseFactory _databaseFactory;

        private SnowflakeDataContext _dataContext;
        protected SnowflakeDataContext DataContext
        {
            get
            {
                return _dataContext ?? (_dataContext = _databaseFactory.GetDataContext());
            }
        }

        public UserStore(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region IUserPasswordStore
        public Task CreateAsync(SnowflakeUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.Factory.StartNew(() => {
                DataContext.Users.Add(user);
                DataContext.SaveChanges();
            });
        }

        public Task DeleteAsync(SnowflakeUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.Factory.StartNew(() => {
                DataContext.Users.Remove(user);
                DataContext.SaveChanges();
            });
        }

        public Task<SnowflakeUser> FindByIdAsync(int userId)
        {
            return Task.Factory.StartNew(() => {
                return DataContext.Users.FirstOrDefault(u => u.Id == userId);
            });
        }

        public Task<SnowflakeUser> FindByNameAsync(string userName)
        {
            return Task.Factory.StartNew(() =>
            {
                return DataContext.Users.FirstOrDefault(u => u.UserName == userName);
            });
        }

        public Task<string> GetPasswordHashAsync(SnowflakeUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.Factory.StartNew(() =>
            {
                return user.PasswordHash;
            });
        }

        public Task<bool> HasPasswordAsync(SnowflakeUser user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(SnowflakeUser user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task UpdateAsync(SnowflakeUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.Factory.StartNew(() =>
            {
                DataContext.Users.Attach(user);
                DataContext.Entry(user).State = EntityState.Modified;

                DataContext.SaveChanges();
            });
        }
        #endregion

        #region ISecurityStampStore
        public Task<string> GetSecurityStampAsync(SnowflakeUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(SnowflakeUser user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }
        #endregion

        #region IUserRoleStore
        public Task AddToRoleAsync(SnowflakeUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            return Task.Factory.StartNew(() =>
            {
                if (!DataContext.Roles.Any(r => r.Name == roleName))
                {
                    DataContext.Roles.Add(new Role
                    {
                        Name = roleName
                    });
                }

                DataContext.UserRoles.Add(new UserRole
                {
                    Role = DataContext.Roles.FirstOrDefault(r => r.Name == roleName),
                    User = user
                });

                DataContext.SaveChanges();
            });
        }

        public Task RemoveFromRoleAsync(SnowflakeUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            return Task.Factory.StartNew(() =>
            {
                var userRole = user.Roles.FirstOrDefault(r => r.Role.Name == roleName);

                if (userRole == null)
                {
                    throw new InvalidOperationException("User does not have that role");
                }

                DataContext.UserRoles.Remove(userRole);

                DataContext.SaveChanges();
            });
        }

        public Task<IList<string>> GetRolesAsync(SnowflakeUser user)
        {
            return Task.Factory.StartNew(() =>
            {
                return (IList<string>)user.Roles.Select(ur => ur.Role.Name);
            });
        }

        public Task<bool> IsInRoleAsync(SnowflakeUser user, string roleName)
        {
            return Task.Factory.StartNew(() =>
            {
                return user.Roles.Any(r => r.Role.Name == roleName);
            });
        }
        #endregion
    }
}
