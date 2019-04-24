namespace MusicSystem.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLyrics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "Lyrics", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Songs", "Lyrics");
        }
    }
}
