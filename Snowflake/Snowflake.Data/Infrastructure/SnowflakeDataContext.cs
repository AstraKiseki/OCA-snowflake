using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Core.Domain;

namespace Snowflake.Data.Infrastructure
{
    public class SnowflakeDataContext : DbContext
    {
        public SnowflakeDataContext() : base("Snowflake")
        {
            var ensureDllIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        //SQL Tables
        public IDbSet<Choice> Choices { get; set; }
        public IDbSet<Conversation> Conversations { get; set; }
        public IDbSet<Message> Messages { get; set; }
        public IDbSet<Participation> Participations { get; set; }
        public IDbSet<SnowflakeUser> Users { get; set; }
        public IDbSet<Thought> Thoughts { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<UserRole> UserRoles { get; set; }


        //Model Relationships
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Conversation
            modelBuilder.Entity<Conversation>()
                        .HasMany(c => c.Messages)
                        .WithRequired(m => m.Conversation)
                        .HasForeignKey(m => m.ConversationId);

            modelBuilder.Entity<Conversation>()
                        .HasMany(c => c.Participations)
                        .WithRequired(p => p.Conversation)
                        .HasForeignKey(p => p.ConversationId);

            // Participation
            modelBuilder.Entity<Participation>()
                        .HasKey(p => new { p.UserId, p.ConversationId });


            //SnowflakenUser
            modelBuilder.Entity<SnowflakeUser>()
                        .HasMany(su => su.Thoughts)
                        .WithRequired(t => t.User)
                        .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<SnowflakeUser>()
                        .HasMany(su => su.Choices)
                        .WithRequired(c => c.User)
                        .HasForeignKey(c => c.UserId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<SnowflakeUser>()
                        .HasMany(su => su.Participations)
                        .WithRequired(p => p.User)
                        .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<SnowflakeUser>()
                        .HasMany(u => u.Roles)
                        .WithRequired(ur => ur.User)
                        .HasForeignKey(ur => ur.UserId);

            // Thought
            modelBuilder.Entity<Thought>()
                        .HasMany(su => su.Choices)
                        .WithRequired(c => c.Thought)
                        .HasForeignKey(c => c.ThoughtId);


            // Role
            modelBuilder.Entity<Role>()
                        .HasMany(r => r.Users)
                        .WithRequired(ur => ur.Role)
                        .HasForeignKey(ur => ur.RoleId);

            // UserRole
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
