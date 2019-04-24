namespace MusicSystem.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddDescription : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Songs", "Description_Id", "dbo.Descriptions");
            DropIndex("dbo.Songs", new[] { "Description_Id" });
            AddColumn("dbo.Songs", "Length", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Songs", "Rating", c => c.Int(nullable: false));
            DropColumn("dbo.Songs", "Description_Id");
            DropTable("dbo.Descriptions");
        }
        
        public override void Down()
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
            
            AddColumn("dbo.Songs", "Description_Id", c => c.Guid());
            DropColumn("dbo.Songs", "Rating");
            DropColumn("dbo.Songs", "Length");
            CreateIndex("dbo.Songs", "Description_Id");
            AddForeignKey("dbo.Songs", "Description_Id", "dbo.Descriptions", "Id");
        }
    }
}
