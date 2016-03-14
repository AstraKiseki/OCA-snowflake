namespace Snowflake.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Choices",
                c => new
                    {
                        ChoiceId = c.Int(nullable: false, identity: true),
                        ThoughtId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Chosen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ChoiceId)
                .ForeignKey("dbo.Thoughts", t => t.ThoughtId, cascadeDelete: true)
                .ForeignKey("dbo.SnowflakeUsers", t => t.UserId)
                .Index(t => t.ThoughtId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Thoughts",
                c => new
                    {
                        ThoughtId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Text = c.String(),
                        NumberOfFlags = c.Int(),
                        Sentiment = c.Int(nullable: false),
                        Language = c.String(),
                    })
                .PrimaryKey(t => t.ThoughtId)
                .ForeignKey("dbo.SnowflakeUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SnowflakeUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Email = c.String(),
                        NumberOfFlags = c.Int(),
                        ConnectionOpened = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Participations",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ConversationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ConversationId })
                .ForeignKey("dbo.Conversations", t => t.ConversationId, cascadeDelete: true)
                .ForeignKey("dbo.SnowflakeUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ConversationId);
            
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        ConversationId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.ConversationId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ConversationId = c.Int(nullable: false),
                        MessageOrder = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.SnowflakeUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Conversations", t => t.ConversationId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ConversationId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.SnowflakeUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Thoughts", "UserId", "dbo.SnowflakeUsers");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.SnowflakeUsers");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Participations", "UserId", "dbo.SnowflakeUsers");
            DropForeignKey("dbo.Participations", "ConversationId", "dbo.Conversations");
            DropForeignKey("dbo.Messages", "ConversationId", "dbo.Conversations");
            DropForeignKey("dbo.Messages", "UserId", "dbo.SnowflakeUsers");
            DropForeignKey("dbo.Choices", "UserId", "dbo.SnowflakeUsers");
            DropForeignKey("dbo.Choices", "ThoughtId", "dbo.Thoughts");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "ConversationId" });
            DropIndex("dbo.Messages", new[] { "UserId" });
            DropIndex("dbo.Participations", new[] { "ConversationId" });
            DropIndex("dbo.Participations", new[] { "UserId" });
            DropIndex("dbo.Thoughts", new[] { "UserId" });
            DropIndex("dbo.Choices", new[] { "UserId" });
            DropIndex("dbo.Choices", new[] { "ThoughtId" });
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Messages");
            DropTable("dbo.Conversations");
            DropTable("dbo.Participations");
            DropTable("dbo.SnowflakeUsers");
            DropTable("dbo.Thoughts");
            DropTable("dbo.Choices");
        }
    }
}
