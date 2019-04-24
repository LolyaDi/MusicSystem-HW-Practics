namespace MusicSystem.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Descriptions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Length = c.Time(nullable: false, precision: 7),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Songs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description_Id = c.Guid(),
                        Group_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Descriptions", t => t.Description_Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Description_Id)
                .Index(t => t.Group_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Songs", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Songs", "Description_Id", "dbo.Descriptions");
            DropIndex("dbo.Songs", new[] { "Group_Id" });
            DropIndex("dbo.Songs", new[] { "Description_Id" });
            DropIndex("dbo.Songs", new[] { "Name" });
            DropIndex("dbo.Groups", new[] { "Name" });
            DropTable("dbo.Songs");
            DropTable("dbo.Groups");
            DropTable("dbo.Descriptions");
        }
    }
}
