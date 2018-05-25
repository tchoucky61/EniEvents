namespace EniEvents.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration011 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Utilisateurs", "ApplicationUserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Utilisateurs", "ApplicationUserId");
        }
    }
}
