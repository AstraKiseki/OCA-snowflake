using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Core.Models;

namespace Snowflake.Core.Domain
{
    public class SnowflakeUser : IUser<int>
    {
        // Primary Keys
        public int Id { get; set; }

        // Foreign Keys

        // Properties
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string Email { get; set; }
        public int? NumberOfFlags { get; set; }
        public bool ConnectionOpened { get; set; }

        // Relationships
        public virtual ICollection<Thought> Thoughts { get; set; }
        public virtual ICollection<Choice> Choices { get; set; }
        public virtual ICollection<Participation> Participations { get; set; }
        public virtual ICollection<UserRole> Roles { get; set; }

        public void Update(SnowflakeUserModel modelSnowflakeUser)
        {
            Id = modelSnowflakeUser.Id;
            UserName = modelSnowflakeUser.UserName;
            PasswordHash = modelSnowflakeUser.PasswordHash;
            SecurityStamp = modelSnowflakeUser.SecurityStamp;
            Email = modelSnowflakeUser.Email;
            NumberOfFlags = modelSnowflakeUser.NumberOfFlags;
            ConnectionOpened = modelSnowflakeUser.ConnectionOpened;
        }
    }
}
